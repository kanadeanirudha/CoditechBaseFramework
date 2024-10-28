using Coditech.BusinessLogicLayer;
using Coditech.Filters;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;
using System.Web.Mvc;
using System.Web.Security;
namespace Coditech.Controllers
{
    [SessionTimeoutAttribute]
    public class UserController : BaseController
    {
        UserMasterBA _userMasterBA = null;
        AdminRoleMasterBA _adminRoleMasterBA = null;
        public UserController()
        {
            _userMasterBA = new UserMasterBA();
            _adminRoleMasterBA = new AdminRoleMasterBA();
        }

        public ActionResult List()
        {
            UserMasterListViewModel list = _userMasterBA.GetUserList();
            return View($"~/Views/UserMaster/List.cshtml", list);
        }

        [HttpGet]
        public virtual ActionResult EditUserMaster(int userMasterId)
        {
            UserMasterViewModel userMasterViewModel = _userMasterBA.GetUserMaster(userMasterId);
            foreach (var item in _adminRoleMasterBA.GetAdminRoleList()?.AdminRoleMasterList)
            {
                userMasterViewModel.AdminRoleMasterList.Add(new SelectListItem
                {
                    Text = item.RoleName,
                    Value = item.AdminRoleMasterId.ToString(),
                    Selected = item.AdminRoleMasterId == userMasterViewModel.AdminRoleMasterId
                });
            }

            return ActionView($"~/Views/UserMaster/Edit.cshtml", userMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult EditUserMaster(UserMasterViewModel userMasterViewModel)
        {
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