using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Helper;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.Expando;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.DataAccessLayer
{
    public class ProductMasterDAL
    {
        private readonly ICoditechRepository<ProductMaster> _productMasterRepository;
        public ProductMasterDAL()
        {
            _productMasterRepository = new CoditechRepository<ProductMaster>();
        }

        public ProductMasterListModel GetProductList(FilterCollection filters, NameValueCollection sorts, int pagingStart, int pagingLength)
        {

            //Bind the Filter, sorts & Paging details.
            PageListModel pageListModel = new PageListModel(filters, sorts, pagingStart, pagingLength);
            CoditechViewRepository<ProductMasterModel> objStoredProc = new CoditechViewRepository<ProductMasterModel>();
            //SP parameters
            objStoredProc.SetParameter("@WhereClause", pageListModel.SPWhereClause?.Replace("true", "1")?.Replace("false", "0"), ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@Rows", pageListModel.PagingLength, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@PageNo", pageListModel.PagingStart, ParameterDirection.Input, DbType.Int32);
            objStoredProc.SetParameter("@Order_By", pageListModel.OrderBy, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("@RowCount", pageListModel.TotalRowCount, ParameterDirection.Output, DbType.Int32);

            IList<ProductMasterModel> productEntityList = objStoredProc.ExecuteStoredProcedureList("Coditech_GetProductList @WhereClause,@Rows,@PageNo,@Order_By,@RowCount OUT", 4, out pageListModel.TotalRowCount);
            ProductMasterListModel listModel = new ProductMasterListModel();
            listModel.ProductMasterList = productEntityList?.Count > 0 ? productEntityList?.ToList() : new List<ProductMasterModel>();
            listModel.BindPageListModel(pageListModel);

            return listModel;
        }

        //Create ProductMaster.
        public ProductMasterModel CreateProductMaster(ProductMasterModel productMasterModel)
        {
            if (IsNull(productMasterModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsProductNameAlreadyExist(productMasterModel.ProductName))
            {
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "ProductMaster name"));
            }
            productMasterModel.Date = DateTime.Now;
            //Create new ProductMaster and return it.
            ProductMaster productMaster = _productMasterRepository.Insert(productMasterModel.FromModelToEntity<ProductMaster>());
            if (productMaster?.ProductMasterId > 0)
            {
                productMasterModel.ProductMasterId = productMaster.ProductMasterId;
            }
            else
            {
                productMasterModel.HasError = true;
                productMasterModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return productMasterModel;
        }

        //Get ProductMaster by ProductMaster id.
        public ProductMasterModel GetProductMaster(int productMasterId)
        {
            if (productMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ProductMasterID"));

            //Get the ProductMaster Details based on id.
            ProductMaster productMasterData = _productMasterRepository.Table.FirstOrDefault(x => x.ProductMasterId == productMasterId && !x.IsDeleted);
            ProductMasterModel productMasterModel = productMasterData.FromEntityToModel<ProductMasterModel>();
            return productMasterModel;
        }

        //Update ProductMaster.
        public ProductMasterModel UpdateProductMaster(ProductMasterModel productMasterModel)
        {
            if (IsNull(productMasterModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (productMasterModel.ProductMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ProductMasterID"));

            if (IsProductNameAlreadyExist(productMasterModel.ProductName, productMasterModel.ProductMasterId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Product Name"));


            ProductMaster productMasterData = _productMasterRepository.Table.Where(x => x.ProductMasterId == productMasterModel.ProductMasterId)?.FirstOrDefault();
            productMasterData.IsDeleted = true;
            productMasterData.DeletedDate = DateTime.Now;
            productMasterData.CreatedBy = productMasterData.ModifiedBy = productMasterModel.ModifiedBy;

            //Update ProductMaster
            bool isProductMasterUpdated = _productMasterRepository.Update(productMasterData);
            if (!isProductMasterUpdated)
            {
                productMasterModel.HasError = true;
                productMasterModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            productMasterData.DeletedDate = null;
            productMasterModel.Date = DateTime.Now;
            productMasterData.IsDeleted = false;
            productMasterData.ProductName = productMasterModel.ProductName;
            productMasterData.Version = productMasterModel.Version;
            productMasterData.Date = productMasterModel.Date ?? productMasterModel.Date;
            productMasterData.IsActive = productMasterModel.IsActive;
            productMasterData.FileName = string.IsNullOrEmpty(productMasterModel.FileName) ? productMasterData.FileName : productMasterModel.FileName;
            ProductMaster productMasterInsertData = _productMasterRepository.Insert(productMasterData);
            if(productMasterInsertData.ProductMasterId > 0)
            {
                productMasterModel.ProductMasterId = productMasterInsertData.ProductMasterId;
            }
            return productMasterModel;
        }

        //Delete ProductMaster.
        public bool DeleteProductMaster(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ProductMasterID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>();
            objStoredProc.SetParameter("ProductMasterId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteProductMaster @ProductMasterId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        //Get Product Details ByProductUniqueCode
        public ProductMasterModel GetProductDetailsByProductUniqueCode(string productUniqueCode)
        {
            if (string.IsNullOrEmpty(productUniqueCode))
                throw new CoditechException(ErrorCodes.NotFound, string.Format(GeneralResources.ErrorIdLessThanOne, "productUniqueCode"));

            //Get the ProductMaster Details based on id.
            ProductMaster productMaster = _productMasterRepository.Table.Where(x => x.ProductUniqueCode == productUniqueCode && x.IsActive && !x.IsDeleted)?.FirstOrDefault();
            ProductMasterModel productMasterModel = productMaster?.FromEntityToModel<ProductMasterModel>();
            if (productMaster != null)
            {
                productMaster.DownloadCount = productMaster.DownloadCount + 1;
                _productMasterRepository.Update(productMaster);
            }
            return productMasterModel;
        }

        //Get FileName By ProductUniqueCode
        public string GetFileNameByProductUniqueCode(string productUniqueCode)
        {
            if (string.IsNullOrEmpty(productUniqueCode))
                throw new CoditechException(ErrorCodes.NotFound, string.Format(GeneralResources.ErrorIdLessThanOne, "productUniqueCode"));

            //Get the ProductMaster Details based on id.
            ProductMaster productMaster = _productMasterRepository.Table.Where(x => x.ProductUniqueCode == productUniqueCode && x.IsActive && !x.IsDeleted)?.FirstOrDefault();
            return productMaster?.FileName;
        }

        #region Private Method

        //Check if Product Master code is already present or not.
        private bool IsProductNameAlreadyExist(string productName, int productMasterId = 0)
             => _productMasterRepository.Table.Any(x => !x.IsDeleted && x.ProductName == productName && (x.ProductMasterId != productMasterId || productMasterId == 0));

        #endregion
    }
}
