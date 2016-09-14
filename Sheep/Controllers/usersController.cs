using MvcApplication1.BLL;
using MvcApplication1.Model;
using MvcApplication1.Models;
using MvcApplication1.Utility;
using Newtonsoft.Json.Linq;
using Sheep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Sheep.Controllers
{
    public class usersController : baseApiController
    {

        _UserBLL bll = new _UserBLL();
        public IHttpActionResult GetIsExit(string v1, int limit = 0, int page = 0, string where = null, string include = null, string order = null)
        {
            try
            {
                List<Wheres> list = new List<Wheres>();
                //条件
                if (!string.IsNullOrEmpty(where))
                {
                    list = JsonHelper.Deserialize<List<Wheres>>(where);
                }
                int count = bll.QueryCount(list);
                return ok(new { count=count});
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }   
           

        }
        //public async Task<HttpResponseMessage> Get()
        //{
        //    return await Task<HttpResponseMessage>.Factory.StartNew(() =>
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, "aa");
        //    });
        ////}
        //public IHttpActionResult GetUsers(string v1, string include = "")
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(include))
        //        {
        //            IEnumerable<_User> list = bll.QueryList(0);
        //            //return result<IEnumerable<_User>>(list);
        //            return ok(list);
        //        }
        //        else
        //        {
        //            //非空时，解析所有列
        //            Dictionary<string, string[]> columns = new Dictionary<string, string[]>();
        //            string includeInit = include.Substring(0, include.Count() - 1);
        //            string[] cols = includeInit.Split(new string[] { "]," }, StringSplitOptions.None);
        //            foreach (var col in cols)
        //            {
        //                string[] cols1 = col.Split('[');
        //                columns.Add(cols1[0], cols1[1].Split('|'));
        //            }

        //            IEnumerable<Dictionary<string, object>> list = bll.QueryListX(0, 10, columns);
        //            //return result<IEnumerable<Dictionary<string, object>>>(list);
        //            return ok(list);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return execept(e.Message);
        //    }
        //}
        // GET api/values/5  获取指定id信息
        public IHttpActionResult GetUser(string v1, string objectId, string include = "")
        {
            try
            {

            
                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("用户ID不能为空");
                }
               
               
                if (string.IsNullOrEmpty(include))
                {
                    _User model = bll.QuerySingleById(objectId);
                    return ok(model);
                }
                else
                {
                    Dictionary<string, string[]> columns = new Dictionary<string, string[]>();
                    string includeInit = include.Substring(0, include.Count() - 1);

                    string[] cols = includeInit.Split(new string[] { "]," }, StringSplitOptions.None);
                    foreach (var col in cols)
                    {
                        string[] cols1 = col.Split('[');
                        columns.Add(cols1[0], cols1[1].Split(','));
                    }
                    Dictionary<string, object> model = bll.QuerySingleByIdX(objectId, columns);
                    //IEnumerable<Dictionary<string, object>> model = bll.QueryListX(0, 1, columns, new List<Wheres> { new Wheres("objectId", "=", objectId) });
                    
                    //if (model == null||model.Count()<1) {
                    //    return notFound("查询失败");
                    //}
                    //return ok(model.First());
                    if (model == null)
                    {
                        return notFound("查询失败");
                    }
                    return ok(model);
                }
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }       
            
        }
        public IHttpActionResult GetAuthorization(string v1,string username, string password)
        {
            try
            {
                //表单验证
                if (isNUll(username, password))
                {
                    return invildRequest("参数不能为空");
                }

                List<Wheres> whs = new List<Wheres>() { new Wheres("username", "=", username) };
                var dir = bll.QuerySingleByWheres(whs);
                if (dir!=null)
                {
                    string obj = (string)(dir.objectId);
                    string pas = (string)(dir.password);
                    string li = "raw:" + password + "  sql:" + pas + "  jiami:" + (password + obj).Md5();
                    if ((password + obj).Md5().Equals(pas))
                    {
                        string sessionToken = Guid.NewGuid().ToString();
                        bll.UpdateById(obj, new Dictionary<string, object> { { "sessionToken", sessionToken } });

                        _User model = bll.QuerySingleById(obj);
                        return ok(model); 
                    }
                    else
                    {
                        return notFound("密码错误" + li);
                    }
                }
                else
                {
                    return notFound("用户不存在");
                }

            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        // PUT api/values/5  修改指定列
        [ApiAuthorizationFilter]
        public IHttpActionResult PutUser(string v1,[FromBody] Dictionary<string, object> column)
        {
            try
            {
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                //更新时间
                DateTime dt = DateTime.Now;
                if (column!=null)
                {
                    column.Add("updatedAt", dt);
                }
                else {
                    return invildRequest("修改参数不能为空");
                }

               
                bool result = bll.UpdateById(objectId, column);              
                if (result)
                {
                    return create(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                return notFound("修改失败");
               
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
        public IHttpActionResult PutPassword(string v1,string username, string password, string code)
        {
            try
            {
                if (isNUll(username, password, code))
                {
                    return invildRequest("参数不能为空");
                }
                //objectId
                var dir = bll.QuerySingleByWheres(new List<Wheres> { new Wheres("username", "=", username) });
                if (dir == null)
                {
                    return notFound("用户不存在");
                }
                string objectId =dir.objectId;
                //验证手机验证码
                MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
                string postUri = "sms/verify?appkey=ed3fea7a8a28&phone=" + username + "&zone=86&code=" + code;

                //string userJson = @"{""appkey"":""ed3fea7a8a28"",""phone"":" + username + @",""zone"":""86"",""code"":" + code + "}";
                //请求验证
                string postResponse = client.Get(postUri);
                if (!string.IsNullOrEmpty(postResponse))
                {
                    JObject jo = JsonHelper.DeserializeObject(postResponse);
                    string status = jo["status"].ToString();
                    if (!status.Equals("200"))
                    {
                        return notFound("验证码错误" + postResponse);
                    }
                }
                else {
                    return notFound("验证码请求验证失败");
                }
                //更新时间
                DateTime dt = DateTime.Now;
               
                
                //更新
                bool result = bll.UpdateByWheres(new Wheres("username","=",username), new Dictionary<string, object> { { "password", (password + objectId).Md5() }, { "updatedAt", dt } });

                if (result)
                {
                    return ok(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                return notFound("修改失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
        }
        [ApiAuthorizationFilter]
        public IHttpActionResult PutPassword(string v1,String oldPassword, String newPassword)
        {
            try
            {
                string objectId = HttpContext.Current.Request.Headers["objectId"];
                //查询密码
                List<Wheres> whs = new List<Wheres>() { new Wheres("objectId", "=", objectId) };
                var dir = bll.QuerySingleByWheres(whs);
                string password = dir.password.ToString();
                if (!password.Equals((oldPassword + objectId).Md5()))
                {
                    return notFound("旧密码错误");
                }
                DateTime dt = DateTime.Now;
                bool result = bll.UpdateById(objectId, new Dictionary<string, object> { { "password", (newPassword + objectId).Md5() }, { "updatedAt", dt } });
                if (result)
                {
                    return create(dt.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    return notFound("失败");
                }
            }
            catch (Exception e)
            {
                return execept(e.Message); 
            }

        }
        // POST api/values  添加用户
        public IHttpActionResult Post(string v1,[FromBody]_User model, string code)
        {
            try
            {
                //表单验证
                if (isNUll(model.username, model.password, code))
                {
                    return invildRequest("参数不能为空");
                }
                //条件
                List<Wheres> list = new List<Wheres>();
                Wheres wh = new Wheres();
                wh.setField("username", "=", model.username, "");
                list.Add(wh);
                //查询用户是否已经存在
                int num = bll.QueryCount(list);
                if (num > 0)
                {
                    return notFound("用户名已存在");
                }
                //验证手机验证码
                MvcApplication1.Utility.HttpClient client = new MvcApplication1.Utility.HttpClient("https://webapi.sms.mob.com");
                string postUri = "sms/verify?appkey=ed3fea7a8a28&phone="+model.username + "&zone=86&code=" + code;

                //string userJson = @"{""appkey"":""ed3fea7a8a28"",""phone"":" + model.username + @",""zone"":""86"",""code"":" + code + "}";
                //请求验证
                string postResponse = client.Get(postUri);
                if (!string.IsNullOrEmpty(postResponse))
                {
                    JObject jo = JsonHelper.DeserializeObject(postResponse);
                    string status = jo["status"].ToString();
                    if (!status.Equals("200"))
                    {
                        return notFound("验证码错误" + postResponse);
                    }
                }
                else {
                    return notFound("验证码请求验证失败");
                }
               
                //主键
                Guid guid = Guid.NewGuid();
                model.objectId = guid.ToString();
                //密码加盐保存
                model.password = (model.password + model.objectId).Md5();
                //初始化数据
                model.nickname = "口袋爆料人";
                model.credit = 0;
                model.overage = 0;
                model.sign_in = true;
                model.shake_times = 3;
                model.createdAt = DateTime.Now;
                model.updatedAt = DateTime.Now;
                string initPassword = "123456";
                model.transaction_password = (initPassword.Md5() + model.objectId).Md5();
                //初始化第三方登录
                authData da = new authData();
                model.authData = da;
                //注册用户
                bool result = bll.Insert(model);
                if (result)
                {
                    return create(model);
                   
                }
                return notFound("注册失败");
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }
           
        }
    }
}
