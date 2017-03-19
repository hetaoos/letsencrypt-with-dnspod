using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XWare.ACME.UI
{
    public class frmBaseForm : Form
    {
        protected static ILog log;
        protected static readonly ApplicationDbContext db = ApplicationDbContext.Default;

        public frmBaseForm()
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            Icon = Properties.Resources.Icon;
        }
    }
}
