using log4net;
using NUCTECH.SUBWAY.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NUCTECH.SUBWAY.Web.Controllers
{ 
    public class BaseController : Controller
    {
        public ILog Logger
        {
            get
            {
                return LogManager.GetLogger("LSS_WebIscService");
            }
        }
        public LoginUserViewModel CurLoginUser
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 返回登录页面
        /// </summary>
        /// <returns></returns>
        public void ReturnLogin(string returnUrl)
        {
            Response.Redirect("/Account/Login?returnUrl=" + HttpUtility.UrlEncode(returnUrl) + "");
        }
    }
}