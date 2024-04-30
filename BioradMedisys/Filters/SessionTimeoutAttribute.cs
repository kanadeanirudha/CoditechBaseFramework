using Coditech.Model;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coditech.Filters
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string[] excludeFromName = new string[] { "generalcommandata", "dashboard" };

            //HttpContext ctx = HttpContext.Current;
            //UserModel userModel = CoditechSessionHelper.GetDataFromSession<UserModel>(CoditechConstant.UserDataSession);
            //if (userModel == null)
            //{
            //    filterContext.Result = new RedirectResult("~/User/Login");
            //    return;
            //}
            //string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName?.ToLower();
            ////if (!excludeFromName.Any(x => x == controllerName) && !userModel.MenuList.Any(x => x.ControllerName == controllerName))
            ////{
            ////    filterContext.Result = new RedirectResult("~/User/Unauthorized");
            ////    return;
            ////}
            base.OnActionExecuting(filterContext);
        }
    }
}