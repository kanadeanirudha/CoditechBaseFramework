using Coditech.BusinessLogicLayer;
using Coditech.Resources;
using Coditech.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Coditech.Controllers
{
    [Authorize]
    public class AdminRoleMasterController : BaseController
    {
        readonly AdminRoleMasterBA _adminRoleMasterBA = null;
        private const string createEdit = "~/Views/AdminRoleMaster/CreateEdit.cshtml";
        public AdminRoleMasterController()
        {
            _adminRoleMasterBA = new AdminRoleMasterBA();
        }

        public ActionResult List()
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            AdminRoleMasterListViewModel list = _adminRoleMasterBA.GetAdminRoleList();
            return View($"~/Views/AdminRoleMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());
            AdminRoleMasterViewModel adminRoleMasterViewModel = new AdminRoleMasterViewModel();
            BindFormList(adminRoleMasterViewModel);
            return View(createEdit, adminRoleMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                adminRoleMasterViewModel = _adminRoleMasterBA.CreateAdminRoleMaster(adminRoleMasterViewModel);
                if (!adminRoleMasterViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordCreationSuccessMessage));
                    return RedirectToAction<AdminRoleMasterController>(x => x.List());
                }
                errorMessage = adminRoleMasterViewModel.ErrorMessage;
            }
            SetNotificationMessage(GetErrorNotificationMessage(errorMessage));
            BindFormList(adminRoleMasterViewModel);
            return View(createEdit, adminRoleMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int adminRoleMasterId)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            AdminRoleMasterViewModel adminRoleMasterViewModel = _adminRoleMasterBA.GetAdminRoleMaster(adminRoleMasterId);
            BindFormList(adminRoleMasterViewModel);
            return ActionView(createEdit, adminRoleMasterViewModel);
        }

        //Post:Edit AdminRole Master.
        [HttpPost]
        public virtual ActionResult Edit(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                adminRoleMasterViewModel = _adminRoleMasterBA.UpdateAdminRoleMaster(adminRoleMasterViewModel);
                bool status = adminRoleMasterViewModel.HasError;
                SetNotificationMessage(status
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));

                if (!status)
                    return RedirectToAction<AdminRoleMasterController>(x => x.Edit(adminRoleMasterViewModel.AdminRoleMasterId));
            }
            BindFormList(adminRoleMasterViewModel);
            return View(createEdit, adminRoleMasterViewModel);
        }

        //Delete AdminRole Master.
        public virtual ActionResult Delete(string adminRoleMasterIds)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(adminRoleMasterIds))
            {
                status = _adminRoleMasterBA.DeleteAdminRoleMaster(adminRoleMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<AdminRoleMasterController>(x => x.List());
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<AdminRoleMasterController>(x => x.List());
        }

        private void BindFormList(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
            adminRoleMasterViewModel.FormList = new List<SelectListItem>();
            adminRoleMasterViewModel.FormList.Add(new SelectListItem() { Text = "AdminRole List", Value = "AdminRoleList" });
            adminRoleMasterViewModel.FormList.Add(new SelectListItem() { Text = "User List", Value = "UserList" });
            adminRoleMasterViewModel.FormList.Add(new SelectListItem() { Text = "Admin Role List", Value = "AdminRoleList" });
        }
    }
}