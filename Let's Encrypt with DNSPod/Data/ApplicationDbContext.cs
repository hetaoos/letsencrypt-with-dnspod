using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XWare.ACME.Data;

namespace XWare.ACME
{
    public class ApplicationDbContext : LiteDatabase
    {
        private static ApplicationDbContext instance;
        public ApplicationDbContext() :
            base("Filename=Data.db;Password=qJ6FxxNGRqx8qq")
        { }
        /// <summary>
        /// 返回默认的
        /// </summary>
        /// <returns></returns>

        public static ApplicationDbContext Default
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (instance == null)
                    instance = new ApplicationDbContext();
                return instance;
            }
        }
        /// <summary>
        /// 账户
        /// </summary>
        public LiteCollection<Account> Accounts => GetCollection<Account>();
        /// <summary>
        /// 域名
        /// </summary>
        public LiteCollection<Domain> Domains => GetCollection<Domain>();
    }
}
