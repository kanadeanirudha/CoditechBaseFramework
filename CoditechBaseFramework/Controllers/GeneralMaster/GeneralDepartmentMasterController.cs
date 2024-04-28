using Coditech.BusinessLogicLayer;
using Coditech.Filters;
using Coditech.Model;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.ViewModel;

using System.Web.Mvc;

namespace Coditech.Controllers
{
    [SessionTimeoutAttribute]
    public class GeneralDepartmentMasterController : BaseController
    {
        readonly GeneralDepartmentMasterBA _generalDepartmentMasterBA = null;
        private const string createEdit = "~/Views/GeneralMaster/GeneralDepartmentMaster/CreateEdit.cshtml";
        public GeneralDepartmentMasterController()
        {
            _generalDepartmentMasterBA = new GeneralDepartmentMasterBA();
        }

        public ActionResult List(DataTableModel dataTableModel)
        {
            dataTableModel = dataTableModel ?? new DataTableModel();
            GeneralDepartmentListViewModel list = _generalDepartmentMasterBA.GetDepartmentList(dataTableModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/GeneralMaster/GeneralDepartmentMaster/_List.cshtml", list);
            }
            return View($"~/Views/GeneralMaster/GeneralDepartmentMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(createEdit, new GeneralDepartmentViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(GeneralDepartmentViewModel generalDepartmentViewModel)
        {
            if (ModelState.IsValid)
            {
                generalDepartmentViewModel = _generalDepartmentMasterBA.CreateDepartment(generalDepartmentViewModel);
                if (!generalDepartmentViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordCreationSuccessMessage));
                    return RedirectToAction<GeneralDepartmentMasterController>(x => x.List(null));
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(generalDepartmentViewModel.ErrorMessage));
            return View(createEdit, generalDepartmentViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int DepartmentId)
        {
            GeneralDepartmentViewModel generalDepartmentViewModel = _generalDepartmentMasterBA.GetDepartment(DepartmentId);
            return ActionView(createEdit, generalDepartmentViewModel);
        }

        //Post:Edit Department.
        [HttpPost]
        public virtual ActionResult Edit(GeneralDepartmentViewModel generalDepartmentViewModel)
        {
            if (ModelState.IsValid)
            {
                bool status = _generalDepartmentMasterBA.UpdateDepartment(generalDepartmentViewModel).HasError;
                SetNotificationMessage(status
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));

                if (!status)
                    return RedirectToAction<GeneralDepartmentMasterController>(x => x.List(new DataTableModel() { SortByColumn = SortKeys.ModifiedDate, SortBy = CoditechConstant.DESCKey }));
            }
            return View(createEdit, generalDepartmentViewModel);
        }

        //Delete Department.
        public virtual ActionResult Delete(string DepartmentIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(DepartmentIds))
            {
                status = _generalDepartmentMasterBA.DeleteDepartment(DepartmentIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<GeneralDepartmentMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<GeneralDepartmentMasterController>(x => x.List(null));
        }
    }
}