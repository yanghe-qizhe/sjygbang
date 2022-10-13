using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.DataAccess.Attributes;
using System.ComponentModel;

namespace EOS.WebApp.Areas.BaseSet.Models
{
    public class BD_QUALITYPERSON
    {
        public int ID { get; set; }
        public string WORKER_ID { get; set; }
        public string WORKER_NAME { get; set; }
        public string VALID { get; set; }
        public string SAMPLING { get; set; }
        public string CHECK_GRADE { get; set; }
        public string SAMPLING_CHECK_GRADE { get; set; }
        public string ABILITY { get; set; }
    }
}