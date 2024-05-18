using Coditech.BusinessLogicLayer;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using QRCoder;

using System;
using System.Drawing;
using System.Drawing.Imaging;
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
        private static string uploadFolderName = "Uploads";
        public ProductMasterController()
        {
            _productMasterBA = new ProductMasterBA();
        }

        public ActionResult List(DataTableModel dataTableModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            dataTableModel = dataTableModel ?? new DataTableModel();
            ProductMasterListViewModel list = _productMasterBA.GetProductList(dataTableModel);
            return View($"~/Views/ProductMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            return View(createEdit, new ProductMasterViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(ProductMasterViewModel productMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

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
                        string userManualFolderPath = Server.MapPath($"~/{uploadFolderName}/UserManual/");
                        if (!Directory.Exists(userManualFolderPath))
                        {
                            Directory.CreateDirectory(userManualFolderPath);
                        }
                        postedFile.SaveAs(userManualFolderPath + Path.GetFileName(postedFile.FileName));
                        //-------------Save QR-----------
                        string url = $"{CoditechSetting.ApplicationUrl}/ProductMaster/DownloadUserManual?productuniquecode={productMasterViewModel.ProductUniqueCode}";
                        CreateQRCode(url, productMasterViewModel.ProductUniqueCode);
                        //-------------Save QR-----------
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
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            ProductMasterViewModel productMasterViewModel = _productMasterBA.GetProductMaster(productMasterId);
            return ActionView(createEdit, productMasterViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult DownloadUserManual(string productuniquecode)
        {
            string fileName = _productMasterBA.GetFileNameByProductUniqueCode(productuniquecode);
            if (!string.IsNullOrEmpty(fileName))
            {
                string fileVirtualPath = $"~/{uploadFolderName}/UserManual/{fileName}";
                return File(fileVirtualPath, "application/force-download", Path.GetFileName(fileVirtualPath));
            }
            return Content("Invalid QR code.");
        }

        [HttpGet]
        public FileResult DownloadQRImage(string productuniquecode)
        {
            string fileVirtualPath = $"~/{uploadFolderName}/QRImages/{productuniquecode}.png";
            return File(fileVirtualPath, "application/force-download", Path.GetFileName(fileVirtualPath));
        }

        //Post:Edit Product Master.
        [HttpPost]
        public virtual ActionResult Edit(ProductMasterViewModel productMasterViewModel)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

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
                    string path = Server.MapPath($"~/{uploadFolderName}/UserManual/");
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
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

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

        private string CreateQRCode(string uniqueCode, string qrFileName)
        {
            //Generate the QR code
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(uniqueCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            string userQRFolderPath = Server.MapPath($"~/{uploadFolderName}/QRImages/");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(userQRFolderPath))
            {
                Directory.CreateDirectory(userQRFolderPath);
            }
            // Specify the folder path to save the QR code image
            string folderPath = @"~/" + uploadFolderName + "/QRImages";

            // Save the QR code as a PNG image file inside the specified folder
            string fileName = Server.MapPath(Path.Combine(folderPath, qrFileName + ".png"));
            qrCodeImage.Save(fileName, ImageFormat.Png);
            return fileName;
        }

    }
}