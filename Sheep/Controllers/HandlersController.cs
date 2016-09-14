using MvcApplication1.Utility;
using Sheep.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class HandlersController : Controller
    {
        
         public string creatQRCode(string v1,string id)
        {
            string url = "http://www.2d-code.cn/2dcode/api.php?key=c_4eaaXfMq1nsHRfxQXZ3/5/j2ZbgsdT7J05haWRfQwHc&url=neday.cn&text=" + id;
            string sss = JsonHelper.Serialize(url);
            //return url;
            return JsonHelper.Serialize(url);
            //string url="http://www.2d-code.cn/2dcode/api.php?key=c_4eaaXfMq1nsHRfxQXZ3/5/j2ZbgsdT7J05haWRfQwHc&url=neday.cn&text="+id;
            //HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(url, System.Text.Encoding.GetEncoding("UTF-8"), "application/json") };
            //return result;
        }
        public string noLoginCredits(string v1,string uid, int credits)
        {
            string url = "http://www.duiba.com.cn/autoLogin/autologin";
            Hashtable hshTable = new Hashtable();
            hshTable.Add("uid", uid);
            hshTable.Add("credits", credits);
            
            return JsonHelper.Serialize(duiba.BuildUrlWithSign(url, hshTable, "4NKx99JCWcbJUpayXFwoz9WzDFTG", "2JWLbAs2aR8r4LifEMCR1eEkQJCa" ));
            //HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(url, System.Text.Encoding.GetEncoding("UTF-8"), "application/json") };
            //return result;
        }
        public string parseCreditConsume(string v1,string appKey,string appSecret,HttpRequest request)
        {
            if(!request.Params["appKey"].Equals(appKey)){
                throw new Exception("appKey not match");
            }
            if(request.Params["timestamp"] == null ){
                throw new Exception("timestamp can't be null");
            }
            Hashtable hshTable = duiba.GetUrlParams(request);

            bool verify=duiba.SignVerify(appSecret,hshTable);
            if(!verify){
                throw new Exception("sign verify fail");
            }
            return JsonHelper.Serialize(hshTable);
        }
        public string parseCreditNotify(string v1, string appKey, string appSecret, HttpRequest request)
        {
            if(!request.Params["appKey"].Equals(appKey)){
                throw new Exception("appKey not match");
            }
            if(request.Params["timestamp"] == null ){
                throw new Exception("timestamp can't be null");
            }
            Hashtable hshTable = duiba.GetUrlParams(request);

            bool verify=duiba.SignVerify(appSecret,hshTable);
            if(!verify){
                throw new Exception("sign verify fail");
            }
            return JsonHelper.Serialize(hshTable);
        }
        public string Timestamp(string v1)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            string url = "{\"timestamp\":\"" + timeStamp + ",\"datetime\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
            //HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(url, System.Text.Encoding.GetEncoding("UTF-8"), "application/json") };
            return JsonHelper.Serialize(url);

        }
    }
}
