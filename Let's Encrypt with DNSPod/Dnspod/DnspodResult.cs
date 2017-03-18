﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xware.ACME.Dnspod
{
    public class DnspodResult
    {
        public Status status { get; set; }

    }

    public class Status
    {
        public int code { get; set; }
        public string message { get; set; }
        public DateTime created_at { get; set; }
    }
}
