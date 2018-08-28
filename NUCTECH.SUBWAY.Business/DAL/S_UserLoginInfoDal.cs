using System;
using System.Data;

namespace NUCTECH.SUBWAY.Business.DAL
{
    public class S_UserLoginInfoDal : BaseDal
    {
        public double GetCurDayLoginSecond(string userid,string startTime,string endTime)
        {
            double result = 0;
            //string startTime = DateTime.Now.ToString("yyyy-MM-dd")+" 00:00:00";
            //string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            #region  计算时间内部
            string sql = " select LoginTime from S_UserLoginInfo where UserID='" + userid+ "' and UserLoginStatus=1 and LoginTime>='"+startTime+ "' and LoginTime<='"+ endTime + "' order by LoginTime  asc";
            DataSet tempopen = db.Query(sql);
            string nextTime = "";
            string tempsql = "";
            for (int i = 0; i < tempopen.Tables[0].Rows.Count; i++)
            {
                nextTime = i < tempopen.Tables[0].Rows.Count - 1 ? tempopen.Tables[0].Rows[i + 1]["LoginTime"].ToString() : endTime;
                tempsql = " select top 1 LoginTime from  S_UserLoginInfo where UserID='" + userid + "' and UserLoginStatus=0 and LoginTime>'" + tempopen.Tables[0].Rows[i]["LoginTime"].ToString() + "' and LoginTime<'"+ nextTime + "' order by LoginTime asc";
                DataSet dstemp = db.Query(tempsql);
                if (dstemp.Tables[0].Rows.Count > 0)
                {
                    result += (Convert.ToDateTime(dstemp.Tables[0].Rows[0][0]) - Convert.ToDateTime(tempopen.Tables[0].Rows[i]["LoginTime"])).TotalSeconds;
                }
                else
                {
                    result += (Convert.ToDateTime(nextTime) - Convert.ToDateTime(tempopen.Tables[0].Rows[i]["LoginTime"])).TotalSeconds;
                }
            }
            #endregion

            #region   计算时间外部
            string sqlwai = " select top 1 UserLoginStatus,LoginTime from S_UserLoginInfo where UserID='" + userid + "'  and LoginTime<'" + startTime + "' order by LoginTime desc";
            DataSet dswai = db.Query(sqlwai);
            if(dswai!=null&&dswai.Tables[0].Rows.Count>0)
            {
                if (dswai.Tables[0].Rows[0]["UserLoginStatus"] != null && dswai.Tables[0].Rows[0]["UserLoginStatus"].ToString() == "True")
                {
                    string sqlwaitemp = " select top 1 LoginTime from S_UserLoginInfo where UserID='" + userid + "'  and LoginTime>='" + startTime + "' and LoginTime<'"+endTime+"' order by LoginTime asc";
                    DataSet dstempwai = db.Query(sqlwaitemp);
                    if (dstempwai.Tables[0].Rows.Count > 0)
                    {
                        result += (Convert.ToDateTime(dstempwai.Tables[0].Rows[0]["LoginTime"].ToString()) - Convert.ToDateTime(startTime)).TotalSeconds;
                    }
                    else
                    {
                        result += (Convert.ToDateTime(endTime) - Convert.ToDateTime(startTime)).TotalSeconds;
                    }
                }
            }
            
            #endregion

            return result;
        }

    }
}
