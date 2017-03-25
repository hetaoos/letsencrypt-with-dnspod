using ACMESharp;
using ACMESharp.ACME;
using ACMESharp.HTTP;
using ACMESharp.JOSE;
using ACMESharp.PKI;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XWare.ACME.Data;
using XWare.ACME.Dnspod;

namespace XWare.ACME
{
    public static class ACMEHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static async Task<bool> Authorize(AcmeClient client, Account account, List<Domain> ls)
        {
            if (!(account.dnspod_tokens?.Count > 0))
            {
                log.Error("not dnspod token.");
                return false;
            }
            List<DnspodApiItem> ds = new List<DnspodApiItem>();
            foreach (var token in account.dnspod_tokens)
            {
                var p = await DnspodApiItem.Create(token);
                if (p.domains?.Length > 0)
                    ds.Add(p);
            }
            if (ds.Count == 0)
            {
                log.Error("dnspod token error");
                return false;
            }

            List<DnspodApiPart> parts = new List<DnspodApiPart>();
            var records = new Dictionary<int, Dnspod.Record.DnspodRecordListResultRecordItem[]>();
            foreach (var d in ls)
            {
                foreach (var a in ds)
                {
                    var found = a.domains.FirstOrDefault(o => o.name.Equals(d.domain, StringComparison.CurrentCultureIgnoreCase)
                      || d.domain.EndsWith($".{o.name}", StringComparison.CurrentCultureIgnoreCase));
                    if (found != null)
                    {
                        var part = new DnspodApiPart() { api = a.api, dns_domain = found, domain = d };
                        if (records.ContainsKey(d.id) == false)
                        {
                            var rr = (await a.api.Record.List(found.id))?.records;
                            records[found.id] = rr;
                            part.records = rr;
                        }
                        else
                            part.records = records[found.id];
                        parts.Add(part);
                        break;
                    }
                }
            }

            foreach (var part in parts)
            {

                log.Info(
                    $"\nAuthorizing Identifier {part.domain.domain} Using Challenge Type {AcmeProtocol.CHALLENGE_TYPE_DNS}");

                part.authzState = client.AuthorizeIdentifier(part.domain.domain);
                part.challenge = client.DecodeChallenge(part.authzState, AcmeProtocol.CHALLENGE_TYPE_DNS);
                part.dnsChallenge = part.challenge.Challenge as DnsChallenge;

                // We need to strip off any leading '/' in the path
                var name = part.dnsChallenge.RecordName.Substring(0, part.dnsChallenge.RecordName.Length - 1 - part.dns_domain.name.Length);

                var record = part.records?.FirstOrDefault(o => o.name.ToLower() == name.ToLower());
                if (record == null)
                {
                    var r = await part.api.Record.Create(part.dns_domain.id, name, part.dnsChallenge.RecordValue);
                    part.record_id = r.record.id;
                }
                else
                {
                    var r = await part.api.Record.Modify(part.dns_domain.id, record.id, name, part.dnsChallenge.RecordValue);
                    part.record_id = r.record.id;
                }
            }
            foreach (var part in parts)
            {
                //while (DnspodApi.DnsGetTxtRecord(part.dnsChallenge.RecordName) != part.dnsChallenge.RecordValue)
                //{
                //    Thread.Sleep(10000);
                //}

                log.Info($" Answer should now be browsable at {part.dnsChallenge.RecordName}");

                try
                {
                    log.Info(" Submitting answer");
                    part.authzState.Challenges = new AuthorizeChallenge[] { part.challenge };
                    client.SubmitChallengeAnswer(part.authzState, AcmeProtocol.CHALLENGE_TYPE_DNS, true);

                    // have to loop to wait for server to stop being pending.
                    // TODO: put timeout/retry limit in this loop
                    while (part.authzState.Status == "pending")
                    {
                        log.Info(" Refreshing authorization");
                        Thread.Sleep(4000); // this has to be here to give ACME server a chance to think
                        var newAuthzState = client.RefreshIdentifierAuthorization(part.authzState);
                        if (newAuthzState.Status != "pending")
                            part.authzState = newAuthzState;
                    }
                    log.Info($" Authorization Result: {part.authzState.Status}");

                    part.domain.AuthorizationState = part.authzState;

                    if (part.authzState.Status == "invalid")
                    {
                        log.Warn($"Authorization Failed {part.authzState.Status}");
                    }


                }
                finally
                {
                }
            }

            return true;
        }

        public static void GetCertificateAutoGen(CertificateGeneratorParam param)
        {
            if (param == null)
            {
                log.Info($" {nameof(param)} is null");
                return;
            }
            param.domains = param.domains.Where(o => o.valid).ToList();
            if (param.domains.Count == 0)
            {
                log.Info($" can't find a valid domain name.");
                return;
            }
            var dnsIdentifier = string.IsNullOrWhiteSpace(param.common_name) ? param.domains.FirstOrDefault().domain : param.common_name.Trim();
            var cp = CertificateProvider.GetProvider();
            var rsaPkp = new RsaPrivateKeyParams();

            var rsaKeys = cp.GeneratePrivateKey(rsaPkp);
            var csrDetails = new CsrDetails
            {
                CommonName = dnsIdentifier,
                AlternativeNames = param.domains.Select(o => o.domain).ToList(),
            };
            var csrParams = new CsrParams
            {
                Details = csrDetails,
            };
            var csr = cp.GenerateCsr(csrParams, rsaKeys, Crt.MessageDigest.SHA256);

            byte[] derRaw;
            using (var bs = new MemoryStream())
            {
                cp.ExportCsr(csr, EncodingFormat.DER, bs);
                derRaw = bs.ToArray();
            }
            var derB64u = JwsHelper.Base64UrlEncode(derRaw);

            log.Info($"\nRequesting Certificate");
            var certRequ = param.client.RequestCertificate(derB64u);

            log.Info($" Request Status: {certRequ.StatusCode}");

            //log.Info($"Refreshing Cert Request");
            //client.RefreshCertificateRequest(certRequ);

            if (certRequ.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var keyGenFile = Path.Combine(param.path, $"{dnsIdentifier}-gen-key.json");
                var keyPemFile = Path.Combine(param.path, $"{dnsIdentifier}-key.pem");
                var csrGenFile = Path.Combine(param.path, $"{dnsIdentifier}-gen-csr.json");
                var csrPemFile = Path.Combine(param.path, $"{dnsIdentifier}-csr.pem");
                var crtDerFile = Path.Combine(param.path, $"{dnsIdentifier}-crt.der");
                var crtPemFile = Path.Combine(param.path, $"{dnsIdentifier}-crt.pem");
                string crtPfxFile = null;

                crtPfxFile = Path.Combine(param.path, $"{dnsIdentifier}-all.pfx");


                using (var fs = new FileStream(keyGenFile, FileMode.Create))
                    cp.SavePrivateKey(rsaKeys, fs);
                using (var fs = new FileStream(keyPemFile, FileMode.Create))
                    cp.ExportPrivateKey(rsaKeys, EncodingFormat.PEM, fs);
                using (var fs = new FileStream(csrGenFile, FileMode.Create))
                    cp.SaveCsr(csr, fs);
                using (var fs = new FileStream(csrPemFile, FileMode.Create))
                    cp.ExportCsr(csr, EncodingFormat.PEM, fs);

                log.Info($" Saving Certificate to {crtDerFile}");
                using (var file = File.Create(crtDerFile))
                    certRequ.SaveCertificate(file);

                Crt crt;
                using (FileStream source = new FileStream(crtDerFile, FileMode.Open),
                        target = new FileStream(crtPemFile, FileMode.Create))
                {
                    crt = cp.ImportCertificate(EncodingFormat.DER, source);
                    cp.ExportCertificate(crt, EncodingFormat.PEM, target);
                }

                // To generate a PKCS#12 (.PFX) file, we need the issuer's public certificate
                var isuPemFile = GetIssuerCertificate(certRequ, cp, param.account);

                log.Info($" Saving Certificate to {crtPfxFile} (with no password set)");
                using (FileStream source = new FileStream(isuPemFile, FileMode.Open),
                        target = new FileStream(crtPfxFile, FileMode.Create))
                {
                    var isuCrt = cp.ImportCertificate(EncodingFormat.PEM, source);
                    cp.ExportArchive(rsaKeys, new[] { crt, isuCrt }, ArchiveFormat.PKCS12, target);
                }

                cp.Dispose();

                return;
            }

            throw new Exception($"Request status = {certRequ.StatusCode}");
        }

        public static void GetCertificateUseCSR(CertificateGeneratorParam param)
        {
            if (param == null)
            {
                log.Info($" {nameof(param)} is null");
                return;
            }
            var dnsIdentifier = DateTime.Now.ToString("yyyyMMddHHmmss");
            var cp = CertificateProvider.GetProvider();

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(param.csr));
            var csr = cp.ImportCsr(EncodingFormat.PEM, ms);
            ms.Dispose();
            byte[] derRaw;
            using (var bs = new MemoryStream())
            {
                cp.ExportCsr(csr, EncodingFormat.DER, bs);
                derRaw = bs.ToArray();
            }

            var derB64u = JwsHelper.Base64UrlEncode(derRaw);

            log.Info($"\nRequesting Certificate");
            var certRequ = param.client.RequestCertificate(derB64u);

            log.Info($" Request Status: {certRequ.StatusCode}");

            if (certRequ.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var csrPemFile = Path.Combine(param.path, $"{dnsIdentifier}-csr.pem");
                var crtDerFile = Path.Combine(param.path, $"{dnsIdentifier}-crt.der");
                var crtPemFile = Path.Combine(param.path, $"{dnsIdentifier}-crt.pem");
                using (var fs = new FileStream(csrPemFile, FileMode.Create))
                    cp.ExportCsr(csr, EncodingFormat.PEM, fs);

                log.Info($" Saving Certificate to {crtDerFile}");
                using (var file = File.Create(crtDerFile))
                    certRequ.SaveCertificate(file);

                Crt crt;
                using (FileStream source = new FileStream(crtDerFile, FileMode.Open),
                        target = new FileStream(crtPemFile, FileMode.Create))
                {
                    crt = cp.ImportCertificate(EncodingFormat.DER, source);
                    cp.ExportCertificate(crt, EncodingFormat.PEM, target);
                }
                cp.Dispose();

                return;
            }

            throw new Exception($"Request status = {certRequ.StatusCode}");
        }

        public static string GetIssuerCertificate(CertificateRequest certificate, CertificateProvider cp, Account account)
        {
            var configPath = @"Z:\";
            var linksEnum = certificate.Links;
            if (linksEnum != null)
            {
                var links = new LinkCollection(linksEnum);
                var upLink = links.GetFirstOrDefault("up");
                if (upLink != null)
                {
                    var tmp = Path.GetTempFileName();
                    try
                    {
                        using (var web = new WebClient())
                        {
                            //if (v.Proxy != null)
                            //    web.Proxy = v.Proxy.GetWebProxy();

                            var uri = new Uri(new Uri(account.uri), upLink.Uri);
                            web.DownloadFile(uri, tmp);
                        }

                        var cacert = new X509Certificate2(tmp);
                        var sernum = cacert.GetSerialNumberString();
                        var tprint = cacert.Thumbprint;
                        var sigalg = cacert.SignatureAlgorithm?.FriendlyName;
                        var sigval = cacert.GetCertHashString();

                        var cacertDerFile = Path.Combine(configPath, $"ca-{sernum}-crt.der");
                        var cacertPemFile = Path.Combine(configPath, $"ca-{sernum}-crt.pem");

                        if (!File.Exists(cacertDerFile))
                            File.Copy(tmp, cacertDerFile, true);

                        log.Info($" Saving Issuer Certificate to {cacertPemFile}");
                        if (!File.Exists(cacertPemFile))
                            using (FileStream source = new FileStream(cacertDerFile, FileMode.Open),
                                    target = new FileStream(cacertPemFile, FileMode.Create))
                            {
                                var caCrt = cp.ImportCertificate(EncodingFormat.DER, source);
                                cp.ExportCertificate(caCrt, EncodingFormat.PEM, target);
                            }

                        return cacertPemFile;
                    }
                    finally
                    {
                        if (File.Exists(tmp))
                            File.Delete(tmp);
                    }
                }
            }

            return null;
        }
    }

    public class CertificateGeneratorParam
    {
        public AcmeClient client { get; set; }
        public Account account { get; set; }
        public List<Domain> domains { get; set; }
        public string path { get; set; }

        public string common_name { get; set; }

        public string csr { get; set; }
    }

    public class DnspodApiItem
    {


        public static async Task<DnspodApiItem> Create(string token)
        {
            var api = new DnspodApi(token);
            var domains = (await api.Domain.List()).domains;
            return new DnspodApiItem()
            {
                api = api,
                domains = domains,
            };
        }

        public DnspodApi api { get; private set; }

        public Dnspod.Domain.Domain[] domains { get; private set; }
    }

    public class DnspodApiPart
    {
        internal AuthorizationState authzState;
        internal AuthorizeChallenge challenge;
        internal DnsChallenge dnsChallenge;

        public DnspodApi api { get; set; }

        public Dnspod.Domain.Domain dns_domain { get; set; }

        public Domain domain { get; set; }

        public Dnspod.Record.DnspodRecordListResultRecordItem[] records { get; set; }

        public int record_id { get; set; }
    }
}
