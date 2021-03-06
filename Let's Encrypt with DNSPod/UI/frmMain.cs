﻿using ACMESharp;
using ACMESharp.ACME;
using ACMESharp.HTTP;
using ACMESharp.JOSE;
using ACMESharp.PKI;
using log4net;
using OpenSSL.X509;
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
using XWare.ACME.Data;
using XWare.ACME.Dnspod;

namespace XWare.ACME.UI
{
    public partial class frmMain : frmBaseForm
    {
        public AcmeClient client;
        public Account account;
        public frmMain()
        {
            InitializeComponent();
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new frmRegister();
            if (t.ShowDialog() == DialogResult.OK)
            {
                client = t.client;
                account = t.account;
                btnRefresh.PerformClick();
                labInfo.Text = account.ToString();
            }
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            Program.ShowConsole();
            btnRefresh.PerformClick();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private async void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var account = cmbAccounts.SelectedItem as Account;
            if (account == null)
            {
                log.Error("not account.");
                await Task.FromResult(0);
                return;
            }

            RS256Signer signer = null;
            AcmeClient client = null;
            try
            {
                signer = account.Signer;

                client = new AcmeClient(new Uri(account.uri), new AcmeServerDirectory(), signer);
                client.Init();
                log.Info("\nGetting AcmeServerDirectory");
                client.GetDirectory(true);

                client.Registration = account.Registration;

                this.client = client;
                this.account = account;
                log.Info("load done.");
                labInfo.Text = account.ToString();
                btnRefreshDomains.PerformClick();
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
                    log.Error(acmeWebException.Message);
                    log.Error("ACME Server Returned:");
                    log.Error(acmeWebException.Response.ContentAsString);
                }
                else
                {
                    log.Error(ex);
                }

            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ls = db.Accounts.FindAll().OrderBy(o => o.email).ThenBy(o => o.uri).ToList();
            cmbAccounts.Items.Clear();
            cmbAccounts.Items.AddRange(ls.ToArray());
            if (ls.Count > 0)
            {
                if (account != null)
                {
                    var r = ls.Where(o => o.id == account.id).FirstOrDefault();
                    if (r != null)
                        cmbAccounts.SelectedItem = r;
                    account = r;
                }

                if (cmbAccounts.SelectedItem == null)
                    cmbAccounts.SelectedIndex = 0;

            }
        }

        private void btnRefreshDomains_Click(object sender, EventArgs e)
        {
            if (account == null)
                return;

            var ls = dgvDomains.DataSource as List<Domain>;
            List<int> selectids = null;
            if (ls?.Count > 0)
            {
                selectids = ls.Where(o => o.@checked).Select(o => o.id).ToList();
            }
            ls = db.Domains.Find(o => o.rid == account.id).ToList();
            if (selectids?.Count > 0)
                foreach (var it in ls.Where(o => selectids.Contains(o.id)))
                    it.@checked = true;

            dgvDomains.DataSource = ls;

            foreach (DataGridViewColumn col in dgvDomains.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnAddDomains_Click(object sender, EventArgs e)
        {
            var f = new frmAddDomains(account.id);
            var dl = f.ShowDialog();
            if (dl == DialogResult.OK)
                btnRefreshDomains.PerformClick();
        }

        private async void btnAuthorize_Click(object sender, EventArgs e)
        {
            TryLoad();
            var ls = dgvDomains.DataSource as List<Domain>;
            var dt = DateTime.Now.AddDays(-2);
            ls = ls.Where(o => o.valid == false).ToList();
            var r = await ACMEHelper.Authorize(client, account, ls);
            db.Domains.Update(ls);
            btnRefreshDomains.PerformClick();
        }

        private void btnCertificate_Click(object sender, EventArgs e)
        {
            TryLoad();
            if (string.IsNullOrWhiteSpace(txtSavePath.Text))
            {
                btnAddDomains.PerformClick();
                if (string.IsNullOrWhiteSpace(txtSavePath.Text))
                    return;
            }
            var path = txtSavePath.Text;
            if (Directory.Exists(path) == false)
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

            if (radioCSRAutoGen.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtCSRCommonName.Text) == false)
                {
                    var ls2 = dgvDomains.DataSource as List<Domain>;
                    var name = txtCSRCommonName.Text;
                    var r = ls2.Any(o => o.valid && o.domain == name);
                    if (r == false)
                    {
                        log.Error("CommonName invalid.");
                        return;
                    }
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtCSR.Text))
                {
                    log.Error("CSR's empty.");
                    return;
                }
            }
            var ls = dgvDomains.DataSource as List<Domain>;
            var param = new CertificateGeneratorParam()
            {
                account = account,
                client = client,
                common_name = txtCSRCommonName.Text,
                path = txtSavePath.Text,
                csr = txtCSR.Text,
                domains = ls,
            };
            try
            {
                if (radioCSRAutoGen.Checked)
                    ACMEHelper.GetCertificateAutoGen(param);
                else
                {
                    ACMEHelper.GetCertificateUseCSR(param);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnSelectAllDomains_Click(object sender, EventArgs e)
        {
            var ls = dgvDomains.DataSource as List<Domain>;
            if (ls?.Count > 0)
            {
                foreach (var it in ls)
                    it.@checked = true;
                dgvDomains.Refresh();
            }
        }

        private void btnReverseSelectDomain_Click(object sender, EventArgs e)
        {
            var ls = dgvDomains.DataSource as List<Domain>;
            if (ls?.Count > 0)
            {
                foreach (var it in ls)
                    it.@checked = !it.@checked;
                dgvDomains.Refresh();
            }
        }

        private void cmbAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var account = cmbAccounts.SelectedItem as Account;
            if (account == null)
            {
                log.Error("not account.");
                return;
            }
            if (this.account?.id != account.id)
            {
                this.account = account;
                if (this.client != null)
                {
                    this.client.Dispose();
                    this.client = null;
                }
                labInfo.Text = "Ready";
                btnRefreshDomains.PerformClick();
            }

        }

        private bool TryLoad(bool force = false)
        {
            if (force == false && this.client != null)
                return true;

            RS256Signer signer = null;
            AcmeClient client = null;
            try
            {
                signer = account.Signer;

                client = new AcmeClient(new Uri(account.uri), new AcmeServerDirectory(), signer);
                client.Init();
                log.Info("\nGetting AcmeServerDirectory");
                client.GetDirectory(true);

                client.Registration = account.Registration;

                this.client = client;
                log.Info("load done.");
                labInfo.Text = account.ToString();
                //
                return true;
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
                    log.Error(acmeWebException.Message);
                    log.Error("ACME Server Returned:");
                    log.Error(acmeWebException.Response.ContentAsString);
                }
                else
                {
                    log.Error(ex);
                }

            }
            return false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (account == null)
                return;
            var frm = new frmEditAccount(account);
            frm.ShowDialog();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dlg = folderBrowserDialog1.ShowDialog();
            if (dlg == DialogResult.OK)
                txtSavePath.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
