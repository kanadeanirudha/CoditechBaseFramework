using Coditech.ExceptionManager;

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.Utilities.Helper
{
    public static class CoditechCookieHelper
    {

        /// <summary>
        /// To create HtppCookies default global setting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static HttpCookie CreateHttpCookies(string name, string value = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            HttpCookie httpCookie = new HttpCookie(name, value);

            return httpCookie;
        }

        /// <summary>
        /// Method to get the http cookies
        /// </summary>
        /// <param name="name"></param>
        /// <returns>HttpCookie</returns>
        public static HttpCookie GetCookie(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            //Read cookie from Response. Cookie will be available in Response if server round trip has not happend after adding cookie            
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(name))
            {
                return HttpContext.Current.Response.Cookies[name];
            }

            // Read cookie from Request. Cookie will be available in Request after server round trip.
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(name))
            {
                return HttpContext.Current.Request.Cookies[name];
            }
            return null;
        }

        /// <summary>
        /// To set or update HttpCookies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="cookieExpireInMinutes">Cookie expiration time. If time is not provide then cookie will set as default time. You may use RARIndiaConstants for parameter</param>
        /// <param name="isCookieHttpOnly">Set HttpOnly property of cookie. If parameter is null then it will set default setting</param>
        /// <param name="isCookieSecure">Set Secure property of cookie. If parameter is null then it will set default setting</param>
        /// <returns></returns>
        public static void SetCookie(string name, string value, double cookieExpireInMinutes = 0, bool? isCookieHttpOnly = null, bool? isCookieSecure = null)
        {
            HttpCookie cookie = GetCookie(name) ?? CreateHttpCookies(name);
            cookie.Value = value;
            SetCookie(cookie, cookieExpireInMinutes, isCookieHttpOnly, isCookieSecure);
        }

        /// <summary>
        /// To set or update HttpCookies
        /// </summary>
        /// <param name="cookie">Object of cookie</param>
        /// <param name="cookieExpireInMinutes">Cookie expiration time. If time is not provide then cookie will set as default time</param>
        /// <param name="IsCookieHttpOnly">Set HttpOnly property of cookie. If parameter is null then it will set default setting</param>
        /// <param name="IsCookieSecure">Set Secure property of cookie. If parameter is null then it will set default setting</param>
        /// <returns></returns>
        private static void SetCookie(HttpCookie cookie, double cookieExpireInMinutes = 0, bool? isCookieHttpOnly = null, bool? isCookieSecure = null)
        {
            try
            {
                if (cookieExpireInMinutes != 0)
                    cookie.Expires = DateTime.Now.AddMinutes(cookieExpireInMinutes);

                if (IsNull(isCookieHttpOnly))
                    cookie.HttpOnly = CoditechSetting.IsCookieHttpOnly;

                if (IsNull(isCookieSecure))
                    cookie.Secure = CoditechSetting.IsCookieSecure;


                if (!HttpContext.Current.Response.Cookies.AllKeys.Contains(cookie.Name))
                {
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message.ToString(), "CookieHelper");
            }
        }

        /// <summary>
        /// To Get value of cookie
        /// </summary>
        /// <param name="name">Name whoose value to get</param>
        /// <returns>T</returns>
        public static T GetCookieValue<T>(string name)
                => (T)Convert.ChangeType(GetCookie(name)?.Value, typeof(T));

        /// <summary>
        /// To Get values of cookie
        /// </summary>
        /// <param name="name">Name whoose value to get</param>
        /// <returns>NameValueCollection</returns>
        public static NameValueCollection GetCookieValues(string name)
                => GetCookie(name)?.Values;

        /// <summary>
        /// To Check if cookie exists 
        /// </summary>
        /// <param name="name">Name whoose value to get</param>
        /// <returns>bool</returns>
        public static bool IsCookieExists(string name)
                => IsNull(GetCookie(name)) ? false : true;



        /// <summary>
        /// To remove http cookie
        /// </summary>
        /// <param name="name">Name of cookie which need to remove</param>
        public static void RemoveCookie(string name)
        {
            if (IsNotNull(GetCookie(name)))
            {
                HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
            }
        }

        /// <summary>
        /// To remove http cookie
        /// </summary>
        /// <param name="cookie">Cookie object which need to remove</param>
        public static void RemoveCookie(HttpCookie cookie)
        {
            if (GetCookie(cookie.Name) == null) return;

            RemoveCookie(cookie);
        }
    }
}
