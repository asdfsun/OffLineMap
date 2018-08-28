using log4net;
using NUCTECH.SUBWAY.Common;
using NUCTECH.SUBWAY.Model.ViewModel;
using System;
using System.Linq;
using System.Web;

namespace NUCTECH.SUBWAY.Business
{
    public class BaseBiz
    {
        public static ILog Logger
        {
            get
            {
                return LogManager.GetLogger("LSS_WebIscService");
            }
        }
        /// <summary>
        /// 设置当前登录用户id
        /// </summary>
        /// <param name="value"></param>
        public void SetUserID(string value)
        {
            HttpContext.Current.Response.Cookies["NUCTECHGLZ_USER"].Value = value;
        }
        /// <summary>
        /// 获取当前登录用户id
        /// </summary>
        /// <returns></returns>
        public static string GetUserID()
        {
            if (HttpContext.Current.Request.Cookies["NUCTECHGLZ_USER"] != null && HttpContext.Current.Request.Cookies["NUCTECHGLZ_USER"].Value != null)
            {
                return HttpContext.Current.Request.Cookies["NUCTECHGLZ_USER"].Value;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 缓存当前登录用户信息列表
        /// </summary>
        public static void SetRoleMenuInfo(LoginUserViewModel currentUserInfo)
        {
            var oldTree = CacheHelper.Read("loginUserRole_" + currentUserInfo.UserId) as LoginUserViewModel;
            if (oldTree != null)
            {
                CacheHelper.Remove("loginUserRole_" + currentUserInfo.UserId);
            }

            CacheHelper.Add("loginUserRole_" + currentUserInfo.UserId, currentUserInfo, DateTimeOffset.MaxValue);
        }
        
        /// <summary>
        /// 缓存当前登录用户信息列表
        /// </summary>
        /// <param name="userId"></param>
        public static LoginUserViewModel GetRoleMenuInfo(string userId)
        {
           return  CacheHelper.Read("loginUserRole_" + userId) as LoginUserViewModel;
        }


    }
}
