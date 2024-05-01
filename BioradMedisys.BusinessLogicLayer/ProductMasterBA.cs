using Coditech.DataAccessLayer;
using Coditech.DataAccessLayer.DataEntity;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Model.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using System;
using System.Collections.Specialized;
using System.Linq;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.BusinessLogicLayer
{
    public class ProductMasterBA : BaseBusinessLogic
    {
        ProductMasterDAL _productMasterDAL = null;
        public ProductMasterBA()
        {
            _productMasterDAL = new ProductMasterDAL();
        }

        public ProductMasterListViewModel GetProductList(DataTableModel dataTableModel)
        {
            FilterCollection filters = null;
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("ProductName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("IsActive", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            NameValueCollection sortlist = SortingData(dataTableModel.SortByColumn, dataTableModel.SortBy);
            ProductMasterListModel ProductMasterList = _productMasterDAL.GetProductList(filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            ProductMasterListViewModel listViewModel = new ProductMasterListViewModel { ProductMasterList = ProductMasterList?.ProductMasterList?.ToViewModel<ProductMasterViewModel>().ToList() };

            SetListPagingData(listViewModel.PageListViewModel, ProductMasterList, dataTableModel, listViewModel.ProductMasterList.Count);

            return listViewModel;
        }

        //Create ProductMaster.
        public ProductMasterViewModel CreateProductMaster(ProductMasterViewModel productMasterViewModel)
        {
            try
            {
                productMasterViewModel.CreatedBy = LoginUserId();
                ProductMasterModel productMasterModel = _productMasterDAL.CreateProductMaster(productMasterViewModel.ToModel<ProductMasterModel>());
                return IsNotNull(productMasterModel) ? productMasterModel.ToViewModel<ProductMasterViewModel>() : new ProductMasterViewModel();
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (ProductMasterViewModel)GetViewModelWithErrorMessage(productMasterViewModel, ex.ErrorMessage);
                    default:
                        return (ProductMasterViewModel)GetViewModelWithErrorMessage(productMasterViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ProductMaster.ToString());
                return (ProductMasterViewModel)GetViewModelWithErrorMessage(productMasterViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get ProductMaster by ProductMaster id.
        public ProductMasterViewModel GetProductMaster(int ProductMasterId)
            => _productMasterDAL.GetProductMaster(ProductMasterId).ToViewModel<ProductMasterViewModel>();

        public string GetFileNameByProductUniqueCode(string productUniqueCode)
             => _productMasterDAL.GetFileNameByProductUniqueCode(productUniqueCode);
        //Update ProductMaster.
        public ProductMasterViewModel UpdateProductMaster(ProductMasterViewModel productMasterViewModel)
        {
            try
            {
                productMasterViewModel.ModifiedBy = LoginUserId();
                ProductMasterModel productMasterModel = _productMasterDAL.UpdateProductMaster(productMasterViewModel.ToModel<ProductMasterModel>());
                return IsNotNull(productMasterModel) ? productMasterModel.ToViewModel<ProductMasterViewModel>() : (ProductMasterViewModel)GetViewModelWithErrorMessage(new ProductMasterListViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ProductMaster.ToString());
                return (ProductMasterViewModel)GetViewModelWithErrorMessage(productMasterViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete ProductMaster.
        public bool DeleteProductMaster(string ProductMasterIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                return _productMasterDAL.DeleteProductMaster(new ParameterModel() { Ids = ProductMasterIds });
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    default:
                        errorMessage = GeneralResources.ErrorFailedToDelete;
                        return false;
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ProductMaster.ToString());
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
    }
}
