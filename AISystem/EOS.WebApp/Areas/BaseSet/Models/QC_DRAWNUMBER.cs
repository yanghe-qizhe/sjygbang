using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.DataAccess.Attributes;
using System.ComponentModel;

namespace EOS.WebApp.Areas.BaseSet.Models
{
    public class QC_DRAWNUMBER
    {
        public int ID { get; set; }
        public string END_DATE { get; set; }
        public string WORKER_ID { get; set; }
        public string WORKER_NAME { get; set; }
        public string WORKER_RESULT { get; set; }
        public string WORK_NAME { get; set; }
    }
}