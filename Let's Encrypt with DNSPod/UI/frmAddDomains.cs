using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using XWare.ACME.Data;

namespace XWare.ACME.UI
{
    public partial class frmAddDomains : Form
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ApplicationDbContext db = ApplicationDbContext.Default;

        private int rid;
        public frmAddDomains(int rid)
        {
            InitializeComponent();
            this.rid = rid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        static Regex regex = new Regex(@"^([a-z0-9][a-z0-9-]*\.)+[a-z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDomains.Text))
            {
                return;
            }

            var arr = txtDomains.Text.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)
                  .Select(o => o.Trim().ToLower())
                  .Distinct().ToList();
            if (arr.Count == 0)
                return;

            var err = arr.Where(o => regex.IsMatch(o) == false)
                   .ToList();
            if (err.Count > 0)
            {
                return;
            }

            arr = arr.Except(db.Domains.Find(o => o.rid == rid)
                    .Select(o => o.domain)
                    .ToList()).ToList();
            if (arr.Count == 0)
            {
                return;
            }

            var domains = arr.Select(o => new Domain()
            {
                domain = o,
                created = DateTime.Now,
                rid = rid,
            }).ToList();
            domains.ForEach(o => db.Domains.Insert(o));

            DialogResult = DialogResult.OK;
        }
    }
}
