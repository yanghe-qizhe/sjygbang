using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UpLoadServer;

namespace UtilPlugin.Http
{
    public class HttpUtil
    {
        public HttpUtil(string url)
        {
            this.Url = url;
        }
        /// <summary>
        /// 提交地址
        /// </summary>
        public string Url { set; get; }

        
        public RetT PostJson<RetT>(object obj, string methodUrl)
        {
            #region MyRegion
            HttpHelper helper = new HttpHelper();
            HttpItem item = new HttpItem();
            item.URL = $"{Url}/{methodUrl}";
            item.Timeout = 1000 * 0x3e8;
            item.Method = "POST";
            item.ContentType = "application/json";
            item.Postdata = Newtonsoft.Json.JsonConvert.SerializeObject( obj);
            


           // item.CerPath = "1";//有证书
            if (obj is string)
            {
                item.Postdata = obj.ToString();
            }
            item.PostEncoding = Encoding.UTF8;
            //item.Allowautoredirect = true;
            item.ProtocolVersion = new Version(1,0);
            HttpResult html = helper.GetHtml(item);
            if (html.StatusCode.ToString() != "OK")
            {
                throw new Exception($"PostJson提交：{item.URL}错误：" + html.StatusDescription);
            }
            //string str = html.Html;
            #endregion
            //return html.Html.ToJson<RetT>();
            return Newtonsoft.Json.JsonConvert.DeserializeObject< RetT >(html.Html);
        }
        //public RetT GetJson<RetT>(object objQuery, WebHeaderCollection header, string methodUrl)
        //{
        //    #region MyRegion
        //    HttpHelper helper = new HttpHelper();
        //    HttpItem item = new HttpItem();
        //    item.URL = $"{Url}/{methodUrl}";
        //    item.Timeout = 1000 * 0x3e8;
        //    item.Method = "Get";
        //    item.Header = header;
        //    // item.ContentType = "application/json";
        //    //item.Postdata = obj.ToJson();
        //    var pros= objQuery.GetType().GetProperties();
        //    string query = "?";
        //    if (objQuery!=null)
        //    {
        //        foreach (var pro in pros)
        //        {
        //            query += $"{pro.Name}={System.Web.HttpUtility.UrlEncode(pro.GetValue(objQuery).ToString())}&";
        //        }
        //    }
        //    else
        //    {
        //        query = "";
        //    }
        //    item.URL += query.TrimEnd('&');
        //    item.Allowautoredirect = true;
        //    item.ProtocolVersion = new Version(1, 0);
        //    HttpResult html = helper.GetHtml(item);
        //    if (html.StatusCode.ToString() != "OK")
        //    {
        //        throw new Exception($"GetJson：{item.URL}错误：" + html.StatusDescription+html.Html);
        //    }
        //    //string str = html.Html;
        //    #endregion
        //    //return html.Html.ToJson<RetT>();
        //    return html.Html.ToJson<RetT>();
        //}
    }
}
