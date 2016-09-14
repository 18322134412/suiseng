using MvcApplication1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.BLL
{
    public partial class CommentBLL
    {
        #region 向数据库中添加一条记录 +bool reply(Comment model,bool isReply)
        /// <summary>
        /// 向数据库中添加一条记录
        /// </summary>
        /// <param name="model">要添加的实体</param>
        /// <returns>是否成功</returns>
        public bool reply(Comment model,bool isReply)
        {
            return _dao.reply(model,isReply);
        }
        #endregion
    }
}
