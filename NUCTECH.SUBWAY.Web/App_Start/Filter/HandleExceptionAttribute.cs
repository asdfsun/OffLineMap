using NUCTECH.SUBWAY.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace NUCTECH.SUBWAY.Web.App_Start.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class HandleExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var baseController = filterContext.Controller as BaseController;
            var logger = baseController.Logger;
            //记录log4net
            if (baseController != null)
            {
                string msg;
                string lgoMsg = filterContext.Exception.ToString();
               
                //响应页面
                if (filterContext.Exception.GetType().Equals(typeof(HttpAntiForgeryException)))
                {
                    msg = "跨站攻击";
                }
                else if (filterContext.Exception.InnerException == null)
                {
                    msg = filterContext.Exception.Message;
                }
                else
                {
                    msg = filterContext.Exception.ToString();
                }

                logger.WarnFormat("[Exceute Method :{0}]:[Exception:{1}]", MethodBase.GetCurrentMethod().Name, lgoMsg);

                baseController.ReturnLogin(filterContext.HttpContext.Request.Url.ToString());

                filterContext.ExceptionHandled = true;
            }
        }
    }
}