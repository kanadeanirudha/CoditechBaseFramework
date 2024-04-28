using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.Utilities.Helper
{
    public static class CoditechSessionHelper
    {
        public static void SaveDataInSession<T>(string key, T value)
        {
            //only InProc session mode will support CLR objects, hence no need of object serialization
            //but other modes (SQL or State server or any custom) may not support storing CLR objects, so in those cases serialization would be required

            switch (GetSessionStateMode())
            {
                case SessionStateMode.InProc:
                    if (IsNotNull(HttpContext.Current.Session))
                        HttpContext.Current.Session[key] = value;
                    break;

                default:
                    SaveInSessionByDataType(key, value);
                    break;

            }
        }

        public static T GetDataFromSession<T>(string key)
        {
            //only InProc session mode will support CLR objects, hence no need of object deserialization
            //but other modes (SQL or State server or any custom) may not support storing CLR objects, so in those cases deserialization would be required

            switch (GetSessionStateMode())
            {
                case SessionStateMode.InProc:
                    var o = HttpContext.Current?.Session?[key];// ConvertValueToNullableType<T>(HttpContext.Current?.Session?[key]);
                    if (o is T)
                    {
                        return (T)o;
                    }
                    break;

                default:
                    // for other modes(SQL or State server or any custom) and generic data type is list of dynamic ,conditional deserialization would be required. 
                    if (typeof(T) == typeof(List<dynamic>))
                    {
                        return (T)(object)GetDeSerializeExpandoData(Convert.ToString(HttpContext.Current.Session[key]));
                    }
                    else
                    {
                        return GetDeSerializeData<T>(Convert.ToString(HttpContext.Current.Session[key]));
                    }
            }

            //NOTE: We can't return default, it may have negative effect on if "T" is bool which will always be false, also int may always return 0 as default
            //but this method have been called on many places to check existence rather then just "get", but then how we can return type T?

            return default(T);
        }

        public static void RemoveDataFromSession(string key)
        {
            var obj = GetDataFromSession<object>(key);
            if (IsNull(obj)) return;

            HttpContext.Current.Session.Remove(key);
        }

        //This method handle grid filter related issue, due to list of dynamic type unable to deserialize from saved data in session, when session state is sql server.
        private static void SaveInSessionByDataType<T>(string key, T value)
        {
            string sessionValue = string.Empty;
            //Only in other mode(SQL or State server or any custom) and generic type is list of dynamic.
            if (typeof(T) == typeof(List<dynamic>))
            {
                List<ExpandoObject> listExpObj = ((List<dynamic>)(object)value).Select(d => d as ExpandoObject).ToList();
                sessionValue = GetSerializedData<List<ExpandoObject>>(listExpObj);
            }
            else
            {
                sessionValue = GetSerializedData<T>(value);
            }
            HttpContext.Current.Session[key] = sessionValue;
        }

        public static SessionStateMode GetSessionStateMode() => IsNull(HttpContext.Current?.Session) ? SessionStateMode.InProc : HttpContext.Current.Session.Mode;

        public static bool IsSessionObjectPresent()
        {
            return !Equals(HttpContext.Current?.Session, null);
        }

        public static void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

        public static string GetSessionId()
        {
            return HttpContext.Current.Session.SessionID;
        }

        #region Private
        private static string GetSerializedData<T>(T tClass) => JsonConvert.SerializeObject(tClass, Formatting.None);

        private static T GetDeSerializeData<T>(string sessionString)
        {
            if (!string.IsNullOrEmpty(sessionString))
                return JsonConvert.DeserializeObject<T>(sessionString);

            return default(T);
        }

        private static List<dynamic> GetDeSerializeExpandoData(string sessionString)
        {
            if (!string.IsNullOrEmpty(sessionString))
            {
                List<ExpandoObject> list = JsonConvert.DeserializeObject<List<ExpandoObject>>(sessionString);
                return list.Select(d => d as dynamic).ToList();
            }
            return default(List<dynamic>);
        }
        #endregion
    }
}
