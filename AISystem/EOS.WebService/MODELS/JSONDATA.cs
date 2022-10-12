using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class JSONDATA
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public string costtime { get; set; }

        public object rows { get; set; }

    }
}