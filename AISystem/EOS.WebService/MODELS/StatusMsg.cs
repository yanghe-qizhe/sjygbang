using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class StatusMsg
    {
        //状态
        public bool Status
        {
            set;
            get;
        }
        //消息
        public string Msg { set; get; }
        //内容
        public object Data { set; get; }

        public List<string> REDATA { get; set; }
        //内容
        public object Tag { set; get; }

        public StatusMsg Error(string error)
        {
            Msg += error;
            Status = false;
            return this;
        }
        public StatusMsg IFWhere(bool IsTrue, string error)
        {
            if (!IsTrue)
            {
                Msg += error;
                Status = false;
            }
            return this;
        }
        public void Create()
        {
            this.Status = true;
        }
    }
}