using Coditech.BusinessLogicLayer;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.ViewModel;

using System;
using System.IO;
using System.Web;
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
            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = Request.Files["UploadManual"];
                if (postedFile.ContentLength > 0 && postedFile.ContentType == "application/pdf")
                {
                    productMasterViewModel.FileName = postedFile.FileName;
                    productMasterViewModel.ProductUniqueCode = Guid.NewGuid().ToString();
                    productMasterViewModel = _productMasterBA.CreateProductMaster(productMasterViewModel);
                    if (!productMasterViewModel.HasError)
                    {
                        string userManualFolderPath = Server.MapPath("~/Uploads/UserManual/");
                        if (!Directory.Exists(userManualFolderPath))
                        {
                            Directory.CreateDirectory(userManualFolderPath);
                        }
                        postedFile.SaveAs(userManualFolderPath + Path.GetFileName(postedFile.FileName));
                        //-------------QR-----------
                        string qrFolderPath = Server.MapPath("~/Uploads/QRImages/");
                        string qrUrl = $"http://coditechsoftware.com/productmaster/downloadusermanual?productuniquecode={productMasterViewModel.ProductUniqueCode}";
                        //-------------QR-----------
                        SetNotificationMessage(GetSuccessNotificationMessage(GeneralResources.RecordCreationSuccessMessage));
                        return RedirectToAction<ProductMasterController>(x => x.List(null));
                    }
                    errorMessage = productMasterViewModel.ErrorMessage;
                }
                else
                {
                    errorMessage = "Please upload prouct manual in PDF Format.";
                }
            }
            SetNotificationMessage(GetErrorNotificationMessage(errorMessage));
            return View(createEdit, productMasterViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int productMasterId)
        {
            ProductMasterViewModel productMasterViewModel = _productMasterBA.GetProductMaster(productMasterId);
            return ActionView(createEdit, productMasterViewModel);
        }


        [HttpGet]
        public virtual ActionResult DownloadUserManual(string productuniquecode)
        {
            string fileName = _productMasterBA.GetFileNameByProductUniqueCode(productuniquecode);
            return null;
        }

        //Post:Edit Product Master.
        [HttpPost]
        public virtual ActionResult Edit(ProductMasterViewModel productMasterViewModel)
        {
            string errorMessage = string.Empty;
            if (ModelState.IsValid)
            {
                HttpPostedFileBase postedFile = Request.Files["UploadManual"];
                if (postedFile?.ContentLength > 0)
                {
                    if (postedFile.ContentType != "application/pdf")
                    {
                        errorMessage = "Please upload prouct manual in PDF Format.";
                        SetNotificationMessage(GetErrorNotificationMessage(errorMessage));
                        return View(createEdit, productMasterViewModel);
                    }
                    productMasterViewModel.FileName = postedFile.FileName;
                }

                bool status = _productMasterBA.UpdateProductMaster(productMasterViewModel).HasError;
                if (postedFile?.ContentLength > 0 && status)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                }
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