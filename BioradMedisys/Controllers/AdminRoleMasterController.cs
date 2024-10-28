using Coditech.BusinessLogicLayer;
using Coditech.Filters;
using Coditech.Resources;
using Coditech.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Coditech.Controllers
{
    [SessionTimeoutAttribute]
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
            AdminRoleMasterListViewModel list = _adminRoleMasterBA.GetAdminRoleList();
            return View($"~/Views/AdminRoleMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            AdminRoleMasterViewModel adminRoleMasterViewModel = new AdminRoleMasterViewModel();
            BindFormList(adminRoleMasterViewModel);
            return View(createEdit, adminRoleMasterViewModel);
        }

        [HttpPost]
        public virtual ActionResult Create(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
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
            AdminRoleMasterViewModel adminRoleMasterViewModel = _adminRoleMasterBA.GetAdminRoleMaster(adminRoleMasterId);
            BindFormList(adminRoleMasterViewModel);
            return ActionView(createEdit, adminRoleMasterViewModel);
        }

        //Post:Edit AdminRole Master.
        [HttpPost]
        public virtual ActionResult Edit(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
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
            adminRoleMasterViewModel.FormList.Add(new SelectListItem() { Text = "ProductMaster", Value = "Product List" });
            adminRoleMasterViewModel.FormList.Add(new SelectListItem() { Text = "User", Value = "User List" });
            adminRoleMasterViewModel.FormList.Add(new SelectListItem() { Text = "AdminRoleMaster", Value = "Admin Role List" });
        }
    }
}