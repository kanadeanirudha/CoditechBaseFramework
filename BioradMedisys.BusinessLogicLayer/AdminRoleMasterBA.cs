using Coditech.DataAccessLayer;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using System;
using System.Linq;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.BusinessLogicLayer
{
    public class AdminRoleMasterBA : BaseBusinessLogic
    {
        AdminRoleMasterDAL _adminRoleMasterDAL = null;
        public AdminRoleMasterBA()
        {
            _adminRoleMasterDAL = new AdminRoleMasterDAL();
        }

        public AdminRoleMasterListViewModel GetAdminRoleList()
        {
            AdminRoleMasterListModel AdminRoleMasterList = _adminRoleMasterDAL.GetAdminRoleList();
            AdminRoleMasterListViewModel listViewModel = new AdminRoleMasterListViewModel { AdminRoleMasterList = AdminRoleMasterList?.AdminRoleMasterList?.ToViewModel<AdminRoleMasterViewModel>().ToList() };
            return listViewModel;
        }

        //Create AdminRoleMaster.
        public AdminRoleMasterViewModel CreateAdminRoleMaster(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
            try
            {
                adminRoleMasterViewModel.CreatedBy = adminRoleMasterViewModel.ModifiedBy = LoginUserId();
                adminRoleMasterViewModel.CreatedDate = adminRoleMasterViewModel.ModifiedDate = DateTime.Now;
                AdminRoleMasterModel adminRoleMasterModel = _adminRoleMasterDAL.CreateAdminRoleMaster(adminRoleMasterViewModel.ToModel<AdminRoleMasterModel>());
                return IsNotNull(adminRoleMasterModel) ? adminRoleMasterModel.ToViewModel<AdminRoleMasterViewModel>() : new AdminRoleMasterViewModel();
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.AlreadyExist:
                        return (AdminRoleMasterViewModel)GetViewModelWithErrorMessage(adminRoleMasterViewModel, ex.ErrorMessage);
                    default:
                        return (AdminRoleMasterViewModel)GetViewModelWithErrorMessage(adminRoleMasterViewModel, GeneralResources.ErrorFailedToCreate);
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.AdminRoleMaster.ToString());
                return (AdminRoleMasterViewModel)GetViewModelWithErrorMessage(adminRoleMasterViewModel, GeneralResources.ErrorFailedToCreate);
            }
        }

        //Get Admin Role Master by AdminRoleMaster id.
        public AdminRoleMasterViewModel GetAdminRoleMaster(int AdminRoleMasterId)
            => _adminRoleMasterDAL.GetAdminRoleMaster(AdminRoleMasterId).ToViewModel<AdminRoleMasterViewModel>();

        //Update AdminRoleMaster.
        public AdminRoleMasterViewModel UpdateAdminRoleMaster(AdminRoleMasterViewModel adminRoleMasterViewModel)
        {
            try
            {
                adminRoleMasterViewModel.ModifiedBy = LoginUserId();
                adminRoleMasterViewModel.CreatedDate = adminRoleMasterViewModel.ModifiedDate = DateTime.Now;
                AdminRoleMasterModel adminRoleMasterModel = _adminRoleMasterDAL.UpdateAdminRoleMaster(adminRoleMasterViewModel.ToModel<AdminRoleMasterModel>());
                return IsNotNull(adminRoleMasterModel) ? adminRoleMasterModel.ToViewModel<AdminRoleMasterViewModel>() : (AdminRoleMasterViewModel)GetViewModelWithErrorMessage(new AdminRoleMasterListViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.AdminRoleMaster.ToString());
                return (AdminRoleMasterViewModel)GetViewModelWithErrorMessage(adminRoleMasterViewModel, GeneralResources.UpdateErrorMessage);
            }
        }

        //Delete AdminRoleMaster.
        public bool DeleteAdminRoleMaster(string AdminRoleMasterIds, out string errorMessage)
        {
            errorMessage = GeneralResources.ErrorFailedToDelete;
            try
            {
                return _adminRoleMasterDAL.DeleteAdminRoleMaster(new ParameterModel() { Ids = AdminRoleMasterIds });
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
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.AdminRoleMaster.ToString());
                errorMessage = GeneralResources.ErrorFailedToDelete;
                return false;
            }
        }

    }
}
