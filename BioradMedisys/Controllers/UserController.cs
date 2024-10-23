using Coditech.BusinessLogicLayer;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;
using System;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using System.Web.Security;
namespace Coditech.Controllers
{

    public class UserController : BaseController
    {
        UserMasterBA _userMasterBA = null;
        public UserController()
        {
            _userMasterBA = new UserMasterBA();
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
                        return RedirectToAction<ProductMasterController>(x => x.List("true"));
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

        public ActionResult List()
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            UserMasterListViewModel list = _userMasterBA.GetUserList();
            return View($"~/Views/UserMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult EditUserMaster(int userMasterId)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            UserMasterViewModel userMasterViewModel = _userMasterBA.GetUserMaster(userMasterId);
            return ActionView($"~/Views/UserMaster/Edit.cshtml", userMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult EditUserMaster(UserMasterViewModel userMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            if (ModelState.IsValid)
            {
                bool status = _userMasterBA.UpdateUserMaster(userMasterViewModel).HasError;
                SetNotificationMessage(status
                    ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                    : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));

                if (!status)
                {
                    return RedirectToAction<UserController>(x => x.List());
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(userMasterViewModel.ErrorMessage));
            return RedirectToAction<UserController>(x => x.EditUserMaster(userMasterViewModel.UserMasterId));
        }
    }
}