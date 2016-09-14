using MvcApplication1.BLL;
using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sheep.Controllers
{
    public class signInController : baseApiController
    {
       
        _UserBLL bll = new _UserBLL();
        CreditsHistoryBLL historyBLL = new CreditsHistoryBLL();
         
        public IHttpActionResult Get(string v1, string objectId,int type)
        {
            try
            {

                if (string.IsNullOrEmpty(objectId))
                {
                    return invildRequest("用户ID不能为空");
                }

                _User user = bll.QuerySingleById(objectId);
                if (user == null) {
                    return notFound("用户不纯在");               
                }
                if (!user.sign_in) {
                    return notFound("已签到");      
                }



                CreditsHistory history = new CreditsHistory();
                if (type == 0)
                {
                    history.type = 0;
                    history.change = 2;
                }
                else if (type == 1)
                {
                    Random ran = new Random();
                    history.change = ran.Next(0, 6);
                    history.type = 1;
                }
                else {
                    Random ran = new Random();
                    history.change = ran.Next(-2, 9);
                    history.type = 2;
                }
                Guid guid = Guid.NewGuid();
                history.objectId = guid.ToString();

                history.updatedAt = DateTime.Now;
                history.createdAt = DateTime.Now;
                history.userId = objectId;
                history.credit = user.credit + history.change; 

                if (historyBLL.SignIn(history, objectId))
                {
                    return ok(history);
                }
                else {
                    return notFound("发生错误");
                }
            }
            catch (Exception e)
            {
                return execept(e.Message);
            }

        }
        
    }
}
