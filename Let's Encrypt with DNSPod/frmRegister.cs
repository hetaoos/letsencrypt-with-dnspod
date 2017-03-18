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

namespace xware.ACME
{
    public partial class frmRegister : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RS256Signer signer;
        public AcmeClient client;

        public frmRegister()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dl = folderBrowserDialog1.ShowDialog();
            if (dl == DialogResult.OK)
                txtPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private async void btnCreate_Click(object sender, EventArgs e)
        {
            Working(true);
            var msg = CheckInput();
            if (string.IsNullOrWhiteSpace(msg) == false)
            {
                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Working(false);
                await Task.FromResult(0);
                return;
            }

            var configPath = txtPath.Text;

            try
            {
                signer = new RS256Signer();

                signer.Init();

                var signerPath = Path.Combine(configPath, "Signer.key");
                if (File.Exists(signerPath))
                {
                    log.Info($"Loading Signer from {signerPath}");
                    using (var signerStream = File.OpenRead(signerPath))
                        signer.Load(signerStream);
                }

                client = new AcmeClient(new Uri(Program.BaseURI), new AcmeServerDirectory(), signer);

                client.Init();
                log.Info("Getting AcmeServerDirectory");
                client.GetDirectory(true);
                var registrationPath = Path.Combine(configPath, "Registration.json");
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
                using (var registrationStream = File.OpenWrite(registrationPath))
                    client.Registration.Save(registrationStream);

                log.Info("Saving Signer");
                using (var signerStream = File.OpenWrite(signerPath))
                    signer.Save(signerStream);
                Working(false);
                this.DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {
                var acmeWebException = ex as AcmeClient.AcmeWebException;
                if (acmeWebException != null)
                {
                    log.Info(acmeWebException.Message);
                    log.Info("ACME Server Returned:");
                    log.Info(acmeWebException.Response.ContentAsString);
                }
                else
                {
                    log.Info(e);
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

                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Working(false);
                return;
            }

        }

        private string CheckInput()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.SelectAll();
                txtEmail.Focus();
                return "请输入一个有效的邮箱。";
            }
            try
            {
                MailAddress m = new MailAddress(txtEmail.Text);
            }
            catch (FormatException ex)
            {
                txtEmail.SelectAll();
                txtEmail.Focus();
                return ex.Message;
            }

            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                txtPath.SelectAll();
                txtPath.Focus();
                return "请选择一个有效的存储路径。";
            }

            try
            {
                if (Directory.Exists(txtPath.Text) == false)
                    Directory.CreateDirectory(txtPath.Text);
            }
            catch (Exception ex)
            {
                txtEmail.SelectAll();
                txtEmail.Focus();
                return ex.Message;
            }

            if (chkAcceptTOS.Checked == false)
            {
                chkAcceptTOS.Focus();
                return "必须接收许可协议。";
            }

            return null;
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
                if (c is Button || c is TextBox || c is CheckBox)
                    c.Enabled = !working;
            }
        }
    }
}
