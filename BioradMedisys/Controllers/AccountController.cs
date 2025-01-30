using Coditech.BusinessLogicLayer;
using Coditech.Model;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
namespace Coditech.Controllers
{
    public class AccountController : BaseController
    {
        UserMasterBA _userMasterBA = null;
        AdminRoleMasterBA _adminRoleMasterBA = null;
        public AccountController()
        {
            _userMasterBA = new UserMasterBA();
            _adminRoleMasterBA = new AdminRoleMasterBA();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            ActiveApplicationLicenseModel activeApplicationLicenseModel = IsApplicationLicenseActive();
            UserLoginViewModel userLoginViewModel = new UserLoginViewModel();
            if (activeApplicationLicenseModel == null)
            {
                ModelState.AddModelError("ErrorMessage", "Server error. Please contact administrator.");
                userLoginViewModel.ErrorMessage = "Server error. Please contact administrator.";
            }
            else if (!activeApplicationLicenseModel.IsActive)
            {
                ModelState.AddModelError("ErrorMessage", activeApplicationLicenseModel.ErrorMessage);
                userLoginViewModel.ErrorMessage = activeApplicationLicenseModel.ErrorMessage;
            }
            return View("~/Views/Login/Login.cshtml", userLoginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            ActiveApplicationLicenseModel activeApplicationLicenseModel = IsApplicationLicenseActive();
            if (activeApplicationLicenseModel == null)
            {
                ModelState.AddModelError("ErrorMessage", "Server error. Please contact administrator.");
                userLoginViewModel.ErrorMessage = "Server error. Please contact administrator.";
                return View("~/Views/Login/Login.cshtml", userLoginViewModel);
            }
            else if (!activeApplicationLicenseModel.IsActive)
            {
                ModelState.AddModelError("ErrorMessage", activeApplicationLicenseModel.ErrorMessage);
                userLoginViewModel.ErrorMessage = activeApplicationLicenseModel.ErrorMessage;
                return View("~/Views/Login/Login.cshtml", userLoginViewModel);
            }
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(userLoginViewModel.UserName) && !string.IsNullOrEmpty(userLoginViewModel.Password))
                {
                    if (Convert.ToBoolean(CoditechSetting.IsLoginWithAD))
                    {
                        var domainContext = new PrincipalContext(ContextType.Domain);
                        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, ""))
                        {
                            userLoginViewModel.HasError = !pc.ValidateCredentials(userLoginViewModel.UserName, userLoginViewModel.Password);
                        }
                        userLoginViewModel.Password = "user@123";
                    }
                    userLoginViewModel = _userMasterBA.Login(userLoginViewModel);
                    if (!userLoginViewModel.HasError)
                    {
                        FormsAuthentication.SetAuthCookie(userLoginViewModel.UserName, false);
                        List<string> list = CoditechSessionHelper.GetDataFromSession<UserModel>(CoditechConstant.UserDataSession)?.FormAccessList;
                        if (list?.Count > 0)
                        {
                            if (list.Any(x => x == "ProductMaster"))
                            {
                                return RedirectToAction<ProductMasterController>(x => x.List("true"));
                            }
                            else if (list.Any(x => x == "User"))
                            {
                                return RedirectToAction<UserController>(x => x.List());
                            }
                            else if (list.Any(x => x == "AdminRoleMaster"))
                            {
                                return RedirectToAction<AdminRoleMasterController>(x => x.List());
                            }
                        }
                        else
                        {
                            return RedirectToAction<AccountController>(x => x.UnauthorizedAccess());
                        }
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
            return RedirectToAction<AccountController>(x => x.Login());
        }

        [HttpGet]
        public virtual ActionResult UnauthorizedAccess()
        {
            return ActionView($"~/Views/UserMaster/UnauthorizedAccess.cshtml");
        }
    }
}