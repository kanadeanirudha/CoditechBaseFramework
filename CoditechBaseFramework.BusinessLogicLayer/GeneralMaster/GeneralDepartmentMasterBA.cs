using Coditech.DataAccessLayer;
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
    public class GeneralDepartmentMasterBA : BaseBusinessLogic
    {
        GeneralDepartmentMasterDAL _generalDepartmentMasterDAL = null;
        public GeneralDepartmentMasterBA()
        {
            _generalDepartmentMasterDAL = new GeneralDepartmentMasterDAL();
        }

        public GeneralDepartmentListViewModel GetDepartmentList(DataTableModel dataTableModel)
        {
            FilterCollection filters = null;
            if (!string.IsNullOrEmpty(dataTableModel.SearchBy))
            {
                filters = new FilterCollection();
                filters.Add("DepartmentName", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
                filters.Add("ContryCode", ProcedureFilterOperators.Like, dataTableModel.SearchBy);
            }

            NameValueCollection sortlist = SortingData(dataTableModel.SortByColumn, dataTableModel.SortBy);
            GeneralDepartmentListModel departmentList = _generalDepartmentMasterDAL.GetDepartmentList(filters, sortlist, dataTableModel.PageIndex, dataTableModel.PageSize);
            GeneralDepartmentListViewModel listViewModel = new GeneralDepartmentListViewModel { GeneralDepartmentList = departmentList?.GeneralDepartmentList?.ToViewModel<GeneralDepartmentViewModel>().ToList() };

            SetListPagingData(listViewModel.PageListViewModel, departmentList, dataTableModel, listViewModel.GeneralDepartmentList.Count);

            return listViewModel;
        }

        //Create Department.
        public GeneralDepartmentViewModel CreateDepartment(GeneralDepartmentViewModel generalDepartmentViewModel)
        {
            try
            {
                generalDepartmentViewModel.CreatedBy = LoginUserId();
                GeneralDepartmentModel generalDepartmentModel = _generalDepartmentMasterDAL.CreateDepartment(generalDepartmentViewModel.ToModel<GeneralDepartmentModel>());
                return IsNotNull(generalDepartmentModel) ? generalDepartmentModel.ToViewModel<GeneralDepartmentViewModel>() : new GeneralDepartmentViewModel();
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (GeneralDepartmentViewModel)GetViewModelWithErrorMessage(generalDepartmentViewModel, ex.ErrorMessage);
                    default:
                        return (GeneralDepartmentViewModel)GetViewModelWithErrorMessage(generalDepartmentViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.Department.ToString());
                return (GeneralDepartmentViewModel)GetViewModelWithErrorMessage(generalDepartmentViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get Department by Department id.
        public GeneralDepartmentViewModel GetDepartment(int DepartmentId)
            => _generalDepartmentMasterDAL.GetDepartment(DepartmentId).ToViewModel<GeneralDepartmentViewModel>();

        //Update Department.
        public GeneralDepartmentViewModel UpdateDepartment(GeneralDepartmentViewModel generalDepartmentViewModel)
        {
            try
            {
                generalDepartmentViewModel.ModifiedBy = LoginUserId();
                GeneralDepartmentModel generalDepartmentModel = _generalDepartmentMasterDAL.UpdateDepartment(generalDepartmentViewModel.ToModel<GeneralDepartmentModel>());
                return IsNotNull(generalDepartmentModel) ? generalDepartmentModel.ToViewModel<GeneralDepartmentViewModel>() : (GeneralDepartmentViewModel)GetViewModelWithErrorMessage(new GeneralDepartmentListViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.Department.ToString());
                return (GeneralDepartmentViewModel)GetViewModelWithErrorMessage(generalDepartmentViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete Department.
        public bool DeleteDepartment(string DepartmentIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                return _generalDepartmentMasterDAL.DeleteDepartment(new ParameterModel() { Ids = DepartmentIds });
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
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.Department.ToString());
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }
    }
}
