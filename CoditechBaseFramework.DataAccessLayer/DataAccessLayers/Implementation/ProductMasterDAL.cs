using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Helper;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

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
            ProductMasterListModel listModel = new ProductMasterListModel();
            List<ProductMaster> productList =   _productMasterRepository.GetEntityList(pageListModel.SPWhereClause)?.ToList();
            listModel.ProductMasterList = new List<ProductMasterModel>();
            foreach(ProductMaster productMaster in productList)
            {
                listModel.ProductMasterList.Add(new ProductMasterModel()
                {
                    ProductMasterId = productMaster.ProductMasterId,
                    ProductName = productMaster.ProductName,
                    ProductUniqueCode= productMaster.ProductUniqueCode,
                    IsActive= productMaster.IsActive
                });
            }
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
            ProductMaster productMasterData = _productMasterRepository.Table.FirstOrDefault(x => x.ProductMasterId == productMasterId);
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

            //Update ProductMaster
            bool isProductMasterUpdated = _productMasterRepository.Update(productMasterModel.FromModelToEntity<ProductMaster>());
            if (!isProductMasterUpdated)
            {
                productMasterModel.HasError = true;
                productMasterModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
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
            objStoredProc.ExecuteStoredProcedureList("RARIndia_DeleteProductMaster @ProductMasterId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }

        #region Private Method

        //Check if ProductMaster code is already present or not.
        private bool IsProductNameAlreadyExist(string productName)
         => _productMasterRepository.Table.Any(x => x.ProductName == productName);
        #endregion
    }
}
