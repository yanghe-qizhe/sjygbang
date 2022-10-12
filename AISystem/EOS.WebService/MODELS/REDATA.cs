using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EOS.WebService
{
    [Serializable]
    [XmlInclude(typeof(REDATA))]
    public class REDATA
    {
        
        public string MATDOCUMENTYEAR { get; set; } = "";
        public string MATERIALDOCUMENT { get; set; } = "";
        public string PRUEFLOS { get; set; } = "";
        public string E_MBLNR { get; set; } = "";

        public string E_MJAHR { get; set; } = "";

        public string E_POSNR { get; set; } = "";
    }
}