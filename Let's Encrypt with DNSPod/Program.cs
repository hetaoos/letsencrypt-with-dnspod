using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xware.ACME
{
    static class Program
    {
        public static string BaseURI = "https://acme-v01.api.letsencrypt.org/";
        //public static string BaseURI = "https://acme-staging.api.letsencrypt.org/";

        public static LiteDatabase db;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            db = new LiteDatabase("Filename=Data.db;Password=qJ6FxxNGRqx8qq");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
