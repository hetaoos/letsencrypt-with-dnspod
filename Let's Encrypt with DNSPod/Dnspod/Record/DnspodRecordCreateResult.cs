﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xware.ACME.Dnspod.Record
{
    public class DnspodRecordCreateResult : DnspodResult
    {
        public DnspodRecordCreateResultRecordItem record { get; set; }
    }


    public class DnspodRecordCreateResultRecordItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
    }

}
