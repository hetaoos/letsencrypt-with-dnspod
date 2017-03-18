using ACMESharp;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XWare.ACME.Data
{
    /// <summary>
    /// 域名
    /// </summary>
    public class Domain
    {
        [BsonId]
        [Browsable(false)]
        public int id { get; set; }

        /// <summary>
        /// 账户id
        /// </summary>
        [BsonIndex]
        [Browsable(false)]
        public int rid { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        [BsonIgnore]
        [DisplayName("选择")]
        public bool @checked { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        [BsonIndex]
        [DisplayName("域名")]
        [ReadOnly(true)]
        public string domain { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [DisplayName("过期时间")]
        [ReadOnly(true)]
        public DateTime? expires { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("状态")]
        [ReadOnly(true)]
        public string status { get; set; }

        [DisplayName("更新时间")]
        [ReadOnly(true)]
        public DateTime? updated { get; set; }

        [Browsable(false)]
        [DisplayName("创建时间")]
        [ReadOnly(true)]
        public DateTime? created { get; set; }

        [Browsable(false)] public byte[] authorization_state { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [BsonIgnore]
        [Browsable(false)]
        public AuthorizationState AuthorizationState
        {
            get
            {
                using (MemoryStream ms = new MemoryStream(this.authorization_state))
                {
                    ms.Position = 0;
                    return AuthorizationState.Load(ms);
                }
            }
            set
            {
                updated = DateTime.Now;
                if (value == null)
                {
                    authorization_state = null;
                    return;
                }
                expires = value.Expires;
                status = value.Status;
                using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms);
                    authorization_state = ms.ToArray();
                }
                
            }
        }

    }
}
