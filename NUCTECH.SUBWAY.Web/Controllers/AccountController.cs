using NUCTECH.SUBWAY.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NUCTECH.SUBWAY.Web.Controllers
{
    public class AccountController : BaseController
    {
        private S_UserInfoBiz userInfoBiz = new S_UserInfoBiz();
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returenUrl = returnUrl;
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult Login(FormCollection form)
        {
            var userName = form.Get("UserName");
            var password = form.Get("Password");
            var isRememberLogin = form.Get("isRemember") == "on";
            var returnUrl = form.Get("returnUrl");
            var result = userInfoBiz.Login(userName, password, isRememberLogin);
            if (result.Status)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    result.ReturnUrl = returnUrl;
                }
                Logger.InfoFormat("[Account]:[Login]:[" + userName + "]:User Login Success,userName:{0},Msg:Success", userName);
            }
            else
            {
                Logger.WarnFormat("[Account]:[Login]:[" + userName + "]:User Login Failed,userName:{0},Msg:{1}", userName, result.ReMsg);
            }
            return Json(result);
        }

    }
}