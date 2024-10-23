using Coditech.BusinessLogicLayer;
using Coditech.Resources;
using Coditech.ViewModel;
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

            return View(createEdit, new AdminRoleMasterViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(AdminRoleMasterViewModel productMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
            }
            SetNotificationMessage(GetErrorNotificationMessage(errorMessage));
            return View(createEdit, productMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int productMasterId, bool isDisabled = true)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            AdminRoleMasterViewModel productMasterViewModel = null;// _adminRoleMasterBA.GetAdminRoleMaster(productMasterId);
            return ActionView(createEdit, productMasterViewModel);
        }

        //Post:Edit Product Master.
        [HttpPost]
        public virtual ActionResult Edit(AdminRoleMasterViewModel productMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                productMasterViewModel = null;// _adminRoleMasterBA.UpdateAdminRoleMaster(productMasterViewModel);
                bool status = productMasterViewModel.HasError;
                SetNotificationMessage(status
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));

                if (!status)
                    return RedirectToAction<AdminRoleMasterController>(x => x.Edit(productMasterViewModel.AdminRoleMasterId, true));
            }
            return View(createEdit, productMasterViewModel);
        }

        //Delete Product Master.
        public virtual ActionResult Delete(string productMasterIds)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(productMasterIds))
            {
                //status = _adminRoleMasterBA.DeleteAdminRoleMaster(productMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<AdminRoleMasterController>(x => x.List());
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<AdminRoleMasterController>(x => x.List());
        }
    }
}