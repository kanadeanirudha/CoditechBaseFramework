using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Coditech.Utilities.Helper
{
    public static class CoditechSetting
    {
        private static NameValueCollection settings = ConfigurationManager.AppSettings;
        public static bool EnableScriptOptimizations
        {
            get
            {
                return Convert.ToBoolean(settings["EnableScriptOptimizations"]);
            }
        }

        public static string ProjectTitle
        {
            get
            {
                return Convert.ToString(settings["ProjectTitle"]);
            }
        }

        public static bool IsCookieHttpOnly
        {
            // In case of IsCookieHttpOnly value explicitly set as false in that case IsCookieHttpOnly will return false value 
            // otherwise for all other condition it will return true
            get
            {
                if (!string.IsNullOrEmpty(settings["IsCookieHttpOnly"]))
                    return Convert.ToBoolean(settings["IsCookieHttpOnly"]);
                else
                    return true;
            }
        }
        public static bool IsCookieSecure
        {
            get
            {
                return Convert.ToBoolean(settings["IsCookieSecure"]);
            }
        }

        public static bool IsUserWantToDebugSql
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableLinqSQLDebugging"]))
                    return false;
                else
                    return Convert.ToBoolean(settings["EnableLinqSQLDebugging"]);
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CoditechEntities"].ConnectionString;
            }
        }


        public static string NotificationMessagesIsFadeOut
        {
            get
            {
                return Convert.ToString(settings["NotificationMessagesIsFadeOut"]);
            }
        }
        public static string ApplicationUrl
        {
            get
            {
                return Convert.ToString(settings["ApplicationUrl"]);
            }
        }
    }
}