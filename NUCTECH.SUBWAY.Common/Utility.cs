using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Common
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    public static class Utility
    {
        /// <summary>
        /// 加密向量
        /// </summary>
        private static string IV = "thtfiviv";

        private static string KEY = "DSLCTKEY";

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt)
        {
            if (string.IsNullOrEmpty(pToDecrypt))
            {
                return "";
            }
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(KEY);
                des.IV = Encoding.ASCII.GetBytes(IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }
                    string str = Encoding.UTF8.GetString(ms.ToArray());
                    return str;
                }
            }
        }

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string pToEncrypt)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = Encoding.ASCII.GetBytes(KEY);
                des.IV = Encoding.ASCII.GetBytes(IV);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }
                    string str = Convert.ToBase64String(ms.ToArray());
                    return str;
                }
            }
        }

        public static string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 获取时间差（返回秒）
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public static double GetDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts2 - ts1;

            return ts3.TotalSeconds;
        }

        /// <summary>
        /// 获取图片名称
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
            }
            int startPos = path.LastIndexOf("/", StringComparison.Ordinal) + 1;
            int length = path.Length - startPos - 4;
            return path.Substring(startPos, length);
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFileText(string path)
        {
            return File.ReadAllText(path, Encoding.Default);
        }

        /// <summary>
        /// SQL通配符转义
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSqlFilterString(this string str)
        {
            var linkchar = new[] { "%", "_", "[]", "-" };
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    if (linkchar.Any(p => Regex.IsMatch(str, GetString(p))))
                    {
                        foreach (string item in linkchar)
                        {
                            str = str.Replace(item, "[" + item + "]");
                        }

                        foreach (var item in linkchar)
                        {
                            str = str.Replace("[]" + item, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return str;
        }

        /// <summary>
        /// 将&#2688; 类型的数字转成汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Uncode(string str)
        {
            var reg = new Regex(@"&#(\d+);");
            string result = reg.Replace(
                str,
                m =>
                {
                    string temp = m.Groups[1].ToString();
                    int iResult;
                    if (Int32.TryParse(temp, out iResult))
                    {
                        return Convert.ToChar(iResult).ToString(CultureInfo.InvariantCulture);
                    }
                    return "";
                });
            return result;
        }

        private static string GetString(string str)
        {
            if (str == "[]")
            {
                str = @"\[]";
            }

            return str;
        }

        /// <summary>
        /// 获取一个混淆码 ,共10位,utc时间的后6位和一个4位的随机数
        /// </summary>
        /// <returns></returns>
        public static string GetSalt()
        {
            TimeSpan ts = DateTime.Now.Subtract(new DateTime(1970, 1, 1));
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            int num = 0;
            string result = string.Empty;
            while (num < 4)
            {
                int n = random.Next(0, 9);
                result += n;
                num++;
            }
            //return result;

            string utcTime = ((long)ts.TotalMilliseconds).ToString(CultureInfo.InvariantCulture);
            string tempStr = utcTime.Substring(utcTime.Length - 6);
            return tempStr + result;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">接收邮件的邮箱</param>
        /// <param name="mailSubject">邮件主题</param>
        /// <param name="message">发送的邮件内容</param>
        public static bool SendMail(string mailTo, ref string msg, string mailSubject = "", string message = "")
        {
            //string msg = "";
          
            //SMTP服务器
            var mailHost = ConfigurationManager.AppSettings["MailHost"];
            //邮件发送人
            var mailFrom = ConfigurationManager.AppSettings["MailFrom"];
            //邮箱登录名
            var mailUserName = ConfigurationManager.AppSettings["MailUserName"];
            //邮箱密码
            var mailPassword = ConfigurationManager.AppSettings["MailPassword"];

            // 邮件服务设置
            var smtpClient = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = mailHost,
                Port = 25,
                EnableSsl = true,
                Credentials = new System.Net.NetworkCredential(mailUserName, mailPassword)
            };
            try
            {
                // 发送邮件设置        
                var mailMessage = new MailMessage(mailFrom, mailTo)
                {
                    Subject = mailSubject,
                    Body = message,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                }; // 发送人和收件人
                smtpClient.Send(mailMessage); // 发送邮件
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return false;
            }
        }
    }
}
