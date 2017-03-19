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
    public partial class frmEditAccount : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = ApplicationDbContext.Default;
        public Account account;

        public frmEditAccount(Account account)
        {
            InitializeComponent();
            this.account = account;
            txtEmail.Text = account.email;
            if (account.dnspod_tokens?.Count > 0)
                txtTokens.Text = string.Join(Environment.NewLine, account.dnspod_tokens);
            cmbUrl.Text = account.uri;
            chkAcceptTOS.Checked = true;
            txtEmail.Select(0, 0);
            txtTokens.Focus();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            Working(true);
            if (string.IsNullOrWhiteSpace(txtTokens.Text) == false)
            {
                account.dnspod_tokens = txtTokens.Text.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(o => o.Trim())
                        .Distinct().ToList();
            }
            account.dnspod_tokens = null;
            db.Accounts.Update(account);
            Working(false);
            log.Info("save done.");
            this.DialogResult = DialogResult.OK;


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
