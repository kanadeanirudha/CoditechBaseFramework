using Coditech.Model;
using System.Web;
using System.Web.Mvc;
using Coditech.Utilities.Helper;
using Coditech.Utilities.Constant;
using System.Linq;
namespace Coditech.Filters
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string[] excludeFromName = new string[] { "Account" };

            HttpContext ctx = HttpContext.Current;
            UserModel userModel = CoditechSessionHelper.GetDataFromSession<UserModel>(CoditechConstant.UserDataSession);
            if (userModel == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName?.ToLower();
            string action = filterContext.ActionDescriptor.ActionName?.ToLower();
            if (!excludeFromName.Any(x => x == $"{controllerName}") && !userModel.FormAccessList.Any(x => x.ToLower() == controllerName))
            {
                filterContext.Result = new RedirectResult("~/Account/UnauthorizedAccess");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}