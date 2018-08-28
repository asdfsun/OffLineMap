using NUCTECH.SUBWAY.Business;
using NUCTECH.SUBWAY.Web.App_Start.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NUCTECH.SUBWAY.Web.Controllers
{
    [Login]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index(string securityName)
        {
            ViewBag.sName = securityName;
            ViewBag.IsOnline = System.Configuration.ConfigurationManager.AppSettings["IsOnline"];
            ViewBag.MapTilesUrl = System.Configuration.ConfigurationManager.AppSettings["MapTilesUrl"];
            return View();
            
        }
      
        public JsonResult getMenuInfo()
        {
            return Json(BaseBiz.GetRoleMenuInfo(BaseBiz.GetUserID()),JsonRequestBehavior.AllowGet);
        }
    }
}