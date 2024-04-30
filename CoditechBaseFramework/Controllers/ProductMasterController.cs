using Coditech.BusinessLogicLayer;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.ViewModel;

using System.Web.Mvc;

namespace Coditech.Controllers
{
    [Authorize]
    public class ProductMasterController : BaseController
    {
        readonly ProductMasterBA _productMasterBA = null;
        private const string createEdit = "~/Views/ProductMaster/CreateEdit.cshtml";
        public ProductMasterController()
        {
            _productMasterBA = new ProductMasterBA();
        }

        public ActionResult List(DataTableModel dataTableModel)
        {
            dataTableModel = dataTableModel ?? new DataTableModel();
            ProductMasterListViewModel list = _productMasterBA.GetProductList(dataTableModel);
            return View($"~/Views/ProductMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(createEdit, new ProductMasterViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(ProductMasterViewModel productMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                productMasterViewModel = _productMasterBA.CreateProductMaster(productMasterViewModel);
                if (!productMasterViewModel.HasError)
                {
                    SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordCreationSuccessMessage));
                    return RedirectToAction<ProductMasterController>(x => x.List(null));
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(productMasterViewModel.ErrorMessage));
            return View(createEdit, productMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int productMasterId)
        {
            ProductMasterViewModel productMasterViewModel = _productMasterBA.GetProductMaster(productMasterId);
            return ActionView(createEdit, productMasterViewModel);
        }

        //Post:Edit Product Master.
        [HttpPost]
        public virtual ActionResult Edit(ProductMasterViewModel productMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                bool status = _productMasterBA.UpdateProductMaster(productMasterViewModel).HasError;
                SetNotificationMessage(status
                ? GetErrorNotificationMessage(GeneralResources.UpdateErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.UpdateMessage));

                if (!status)
                    return RedirectToAction<ProductMasterController>(x => x.List(new DataTableModel() { SortByColumn = SortKeys.ModifiedDate, SortBy = CoditechConstant.DESCKey }));
            }
            return View(createEdit, productMasterViewModel);
        }

        //Delete Product Master.
        public virtual ActionResult Delete(string productMasterIds)
        {
            string message = string.Empty;
            bool status = false;
            if (!string.IsNullOrEmpty(productMasterIds))
            {
                status = _productMasterBA.DeleteProductMaster(productMasterIds, out message);
                SetNotificationMessage(!status
                ? GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage)
                : GetSuccessNotificationMessage(GeneralResources.DeleteMessage));
                return RedirectToAction<ProductMasterController>(x => x.List(null));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<ProductMasterController>(x => x.List(null));
        }
    }
}