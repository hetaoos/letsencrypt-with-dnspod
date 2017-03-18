using ACMESharp;
using ACMESharp.ACME;
using ACMESharp.HTTP;
using ACMESharp.JOSE;
using ACMESharp.PKI;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xware.ACME.Dnspod;

namespace xware.ACME
{
    public partial class frmMain : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RS256Signer signer;
        public AcmeClient client;
        public DnspodApi api = new DnspodApi();
        public frmMain()
        {
            InitializeComponent();


        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new frmRegister();
            if (t.ShowDialog() == DialogResult.OK)
            {
                signer = t.signer;
                client = t.client;
            }
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dl = openFileDialog1.ShowDialog();
            if (dl != DialogResult.OK)
                return;
            var configPath = Path.GetDirectoryName(openFileDialog1.FileName);
            var msg = LoadFile(configPath);
            if (string.IsNullOrWhiteSpace(msg) == false)
            {
                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                await Task.FromResult(0);
                return;
            }
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="configPath"></param>
        /// <returns></returns>
        private string LoadFile(string configPath)
        {
            RS256Signer signer = null;
            AcmeClient client = null;
            try
            {
                signer = new RS256Signer();
                signer.Init();
                var signerPath = Path.Combine(configPath, "Signer.key");

                log.Info($"Loading Signer from {signerPath}");
                using (var signerStream = File.OpenRead(signerPath))
                    signer.Load(signerStream);


                client = new AcmeClient(new Uri(Program.BaseURI), new AcmeServerDirectory(), signer);
                client.Init();
                log.Info("\nGetting AcmeServerDirectory");
                client.GetDirectory(true);

                var registrationPath = Path.Combine(configPath, "Registration.json");

                log.Info($"Loading Registration from {registrationPath}");
                using (var registrationStream = File.OpenRead(registrationPath))
                    client.Registration = AcmeRegistration.Load(registrationStream);

                this.client = client;
                this.signer = signer;
                return null;

            }
            catch (Exception ex)
            {

                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }

                if (signer != null)
                {
                    signer.Dispose();
                    signer = null;
                }

                var acmeWebException = ex as AcmeClient.AcmeWebException;
                if (acmeWebException != null)
                {
                    log.Info(acmeWebException.Message);
                    log.Info("ACME Server Returned:");
                    log.Info(acmeWebException.Response.ContentAsString);
                    return acmeWebException.Message;
                }
                else
                {
                    log.Info(ex);
                    return ex.Message;
                }

            }
        }


        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {

        }

    }
}
