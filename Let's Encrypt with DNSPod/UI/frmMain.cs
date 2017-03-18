﻿using ACMESharp;
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
using XWare.ACME.Data;
using XWare.ACME.Dnspod;

namespace XWare.ACME.UI
{
    public partial class frmMain : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = ApplicationDbContext.Default;
        public RS256Signer signer;
        public AcmeClient client;
        public Account account;
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
                account = t.account;
                refreshToolStripMenuItem.PerformClick();
                labInfo.Text = account.ToString();
            }
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            Program.ShowConsole();
            refreshToolStripMenuItem.PerformClick();
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
                this.signer = signer;
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

        private void registerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            registerToolStripMenuItem.PerformClick();
        }

        private void btnRefreshDomains_Click(object sender, EventArgs e)
        {
            if (account == null)
                return;
            dgvDomains.DataSource = db.Domains.Find(o => o.rid == account.id).ToList();

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
            var ls = dgvDomains.DataSource as List<Domain>;
            var dt = DateTime.Now.AddDays(-2);
            ls = ls.Where(o => o.status != "valid" || o.expires < dt).ToList();
            var r = await ACMEHelper.Authorize(client, account, ls);
            db.Domains.Update(ls);
            btnRefreshDomains.PerformClick();
        }

        private void btnCertificate_Click(object sender, EventArgs e)
        {
            var ls = dgvDomains.DataSource as List<Domain>;
            var r = ACMEHelper.GetCertificate(client, account, ls, @"Z:\");
        }
    }
}