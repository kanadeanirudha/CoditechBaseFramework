using Coditech.BusinessLogicLayer;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using System.Web.Mvc;
using System.Web.Security;

namespace Coditech.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController
    {
        UserMasterBA _userMasterBA = null;

        public UserController()
        {
            _userMasterBA = new UserMasterBA();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("~/Views/Login/Login.cshtml", new UserLoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(userLoginViewModel.UserName) && !string.IsNullOrEmpty(userLoginViewModel.Password))
                {
                    userLoginViewModel = _userMasterBA.Login(userLoginViewModel);
                    if (!userLoginViewModel.HasError)
                    {
                        FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                        return RedirectToAction<ProductMasterController>(x => x.List(null));
                    }
                    ModelState.AddModelError("ErrorMessage", userLoginViewModel.ErrorMessage);
                }
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Invalid Email Address or Password");
            }
            return View("~/Views/Login/Login.cshtml", userLoginViewModel);
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            CoditechSessionHelper.RemoveDataFromSession(CoditechConstant.UserDataSession);
            return RedirectToAction<UserController>(x => x.Login());
        }

        public ActionResult Unauthorized()
        {
            return View("~/Views/Shared/Unauthorized.cshtml");
        }
    }
}