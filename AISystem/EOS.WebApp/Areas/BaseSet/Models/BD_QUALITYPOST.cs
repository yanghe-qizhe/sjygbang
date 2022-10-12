using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.DataAccess.Attributes;
using System.ComponentModel;

namespace EOS.WebApp.Areas.BaseSet.Models
{
    public class BD_QUALITYPOST
    {
        public int ID { get; set; }
        public string WORK_ID { get; set; }
        public string WORK_NAME { get; set; }
        public string WORK_AMOUNT { get; set; }
    }
}