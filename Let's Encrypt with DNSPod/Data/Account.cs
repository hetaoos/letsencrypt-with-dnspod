using ACMESharp;
using ACMESharp.JOSE;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xware.ACME.Data
{
    /// <summary>
    /// 账户
    /// </summary>
    public class Account
    {
        [BsonId]
        public int id { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [BsonIndex]
        public string email { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// dnspod app key
        /// </summary>
        public List<string> dnspod_tokens { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public byte[] signer { get; set; }
        /// <summary>
        /// 注册信息
        /// </summary>
        public byte[] registration { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [BsonIgnore]
        public RS256Signer Signer
        {
            get
            {
                var signer = new RS256Signer();
                signer.Init();
                using (MemoryStream ms = new MemoryStream(this.signer))
                {
                    ms.Position = 0;
                    signer.Load(ms);
                }
                return signer;
            }
            set
            {
                if (value == null)
                {
                    signer = null;
                    return;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms);
                    signer = ms.ToArray();
                }
            }
        }
        /// <summary>
        /// 注册信息
        /// </summary>
        [BsonIgnore]
        public AcmeRegistration Registration
        {
            get
            {
                using (MemoryStream ms = new MemoryStream(this.registration))
                {
                    ms.Position = 0;
                    return AcmeRegistration.Load(ms);
                }
            }
            set
            {
                if (value == null)
                {
                    registration = null;
                    return;
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    value.Save(ms);
                    registration = ms.ToArray();
                }
            }
        }

    }
}
