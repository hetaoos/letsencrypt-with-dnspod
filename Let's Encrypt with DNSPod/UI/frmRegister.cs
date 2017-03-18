using ACMESharp;
using ACMESharp.JOSE;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XWare.ACME.Data;

namespace XWare.ACME.UI
{
    public partial class frmRegister : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = ApplicationDbContext.Default;
        public RS256Signer signer;
        public AcmeClient client;
        public Account account;

        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            Working(true);
            var ok = CheckInput();
            if (ok == false)
            {
                Working(false);
                await Task.FromResult(0);
                return;
            }
            
            var c = db.Accounts.Find(o => o.email == txtEmail.Text && o.uri == cmbUrl.Text)
                 .Count();
            if (c > 0)
            {
                log.Warn($"该账号已存在。");
                return;
            }

            try
            {
                signer = new RS256Signer();

                signer.Init();

                client = new AcmeClient(new Uri(cmbUrl.Text), new AcmeServerDirectory(), signer);

                client.Init();
                log.Info("Getting AcmeServerDirectory");
                client.GetDirectory(true);

                var email = txtEmail.Text;

                var contacts = new string[] { };
                if (!String.IsNullOrEmpty(email))
                {
                    email = "mailto:" + email;
                    contacts = new string[] { email };
                }
                log.Info("Calling Register");
                var registration = client.Register(contacts);

                log.Info("Updating Registration");
                client.UpdateRegistration(true, true);

                log.Info("Saving Registration");

                account = new Account()
                {
                    email = txtEmail.Text,
                    Registration = client.Registration,
                    Signer = signer,
                    uri = cmbUrl.Text,
                };
                if (string.IsNullOrWhiteSpace(txtTokens.Text) == false)
                {
                    account.dnspod_tokens = txtTokens.Text.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.Trim())
                            .Distinct().ToList();
                }
                db.Accounts.Insert(account);
                Working(false);
                log.Info("register done.");
                this.DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {
                var acmeWebException = ex as AcmeClient.AcmeWebException;
                if (acmeWebException != null)
                {
                    log.Error(acmeWebException.Message);
                    log.Error("ACME Server Returned:");
                    log.Error(acmeWebException.Response.ContentAsString);
                }
                else
                {
                    log.Error(e);
                }
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
                account = null;
                Working(false);
                return;
            }

        }

        private bool CheckInput()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.SelectAll();
                txtEmail.Focus();
                log.Error("请输入一个有效的邮箱。");
                return false;
            }
            try
            {
                MailAddress m = new MailAddress(txtEmail.Text);
            }
            catch (FormatException ex)
            {
                txtEmail.SelectAll();
                txtEmail.Focus();
                log.Error(ex.Message);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbUrl.Text))
            {
                txtEmail.SelectAll();
                txtEmail.Focus();
                log.Error("请输入一个地址。");
                return false;
            }


            if (chkAcceptTOS.Checked == false)
            {
                chkAcceptTOS.Focus();
                log.Error("必须接收许可协议。");
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void Working(bool working = true)
        {
            this.UseWaitCursor = working;
            foreach (Control c in this.Controls)
            {
                if (c is Button || c is TextBox || c is CheckBox || c is ComboBox)
                    c.Enabled = !working;
            }
        }
    }
}
