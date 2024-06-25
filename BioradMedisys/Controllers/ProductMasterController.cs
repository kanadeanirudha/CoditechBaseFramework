using ClosedXML.Excel;

using Coditech.BusinessLogicLayer;
using Coditech.Model;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

using QRCoder;

using System;
using System.Data;
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

        public ActionResult List(string filterBy)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            ProductMasterListViewModel list = _productMasterBA.GetProductList(filterBy, new DataTableModel());
            list.FilterBy = filterBy;
            return View($"~/Views/ProductMaster/List.cshtml", list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            return View(createEdit, new ProductMasterViewModel() { IsDisabled = false });
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
                        return RedirectToAction<ProductMasterController>(x => x.List(productMasterViewModel.IsActive.ToString().ToLower()));
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
        public virtual ActionResult Edit(int productMasterId, bool isDisabled = true)
        {
            if (IsLoginSessionExpired())
                return RedirectToAction<UserController>(x => x.Login());

            ProductMasterViewModel productMasterViewModel = _productMasterBA.GetProductMaster(productMasterId);
            productMasterViewModel.IsDisabled = isDisabled;
            return ActionView(createEdit, productMasterViewModel);
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

                productMasterViewModel = _productMasterBA.UpdateProductMaster(productMasterViewModel);
                bool status = productMasterViewModel.HasError;
                if (postedFile?.ContentLength > 0 && !status)
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
                    return RedirectToAction<ProductMasterController>(x => x.Edit(productMasterViewModel.ProductMasterId, true));
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
                return RedirectToAction<ProductMasterController>(x => x.List("true"));
            }

            SetNotificationMessage(GetErrorNotificationMessage(GeneralResources.DeleteErrorMessage));
            return RedirectToAction<ProductMasterController>(x => x.List("true"));
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
            ProductMasterModel productMasterModel = _productMasterBA.GetProductDetailsByProductUniqueCode(productuniquecode);

            string fileVirtualPath = $"{Request.Url.Scheme}://{Request.Url.Authority}/{uploadFolderName}/QRImages/{productuniquecode}.png";
            string html = $"<div><img src='{fileVirtualPath}'/></div>" +
                          $"<div style='padding-left:50px;' >" +
                          $"<div>Product Name: {productMasterModel.ProductName}</div>" +
                          $"<div >Version: {productMasterModel.Version}</div>" +
                          $"<div>Updated Date: {productMasterModel.ModifiedDate}</div>" +
                          $"</div>";
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(html);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", $"{productMasterModel.ProductName}.pdf");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }

        //[HttpPost]
        public FileResult ExportToExcel()
        {
            ProductMasterListViewModel list = _productMasterBA.GetProductList("", new DataTableModel());

            DataTable dataTable = new DataTable("ProductList");
            dataTable.Clear();
            dataTable.Columns.Add("ProductName", typeof(string));
            dataTable.Columns.Add("FileName", typeof(string));
            dataTable.Columns.Add("Version", typeof(string));
            dataTable.Columns.Add("DownloadCount", typeof(string));
            dataTable.Columns.Add("UploadedBy", typeof(string));
            dataTable.Columns.Add("UpdatedDate", typeof(DateTime));
            dataTable.Columns.Add("Status", typeof(bool));
            foreach (ProductMasterViewModel item in list?.ProductMasterList)
            {
                dataTable.Rows.Add(item.ProductName, item.FileName, item.Version, item.DownloadCount, item.UploadedBy, item.ModifiedDate, item.IsActive);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductList.xlsx");
                }
            }
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