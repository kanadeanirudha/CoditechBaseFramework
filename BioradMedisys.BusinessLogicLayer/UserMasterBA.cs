using Coditech.DataAccessLayer;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;
using System;
using System.Linq;
using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.BusinessLogicLayer
{
    public class UserMasterBA : BaseBusinessLogic
    {
        UserMasterDAL _userMasterDAL = null;
        public UserMasterBA()
        {
            _userMasterDAL = new UserMasterDAL();
        }

        public UserLoginViewModel Login(UserLoginViewModel userLoginViewModel)
        {
            try
            {
                userLoginViewModel.Password = MD5Hash(userLoginViewModel.Password);
                UserModel userModel = _userMasterDAL.Login(userLoginViewModel.ToModel<UserModel>());
                if (IsNotNull(userModel))
                {
                    SaveInSession<UserModel>(CoditechConstant.UserDataSession, userModel);
                }
                return userLoginViewModel;
            }
            catch (CoditechException ex)
            {
                switch (ex.ErrorCode)
                {
                    case ErrorCodes.NotFound:
                        return (UserLoginViewModel)GetViewModelWithErrorMessage(userLoginViewModel, GeneralResources.ErrorMessage_ThisaccountdoesnotexistEnteravalidemailaddressorpassword);
                    default:
                        return (UserLoginViewModel)GetViewModelWithErrorMessage(userLoginViewModel, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);
                }
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.User.ToString());
                return (UserLoginViewModel)GetViewModelWithErrorMessage(userLoginViewModel, GeneralResources.ErrorMessage_PleaseContactYourAdministrator);
            }
        }

        public UserMasterListViewModel GetUserList()
        {
            UserMasterListModel userMasterList = _userMasterDAL.GetUserList();
            UserMasterListViewModel listViewModel = new UserMasterListViewModel { UserMasterList = userMasterList?.UserMasterList?.ToViewModel<UserMasterViewModel>().ToList() };
            return listViewModel;
        }

        //Get ProductMaster by ProductMaster id.
        public UserMasterViewModel GetUserMaster(int userMasterId)
            => _userMasterDAL.GetUserMaster(userMasterId).ToViewModel<UserMasterViewModel>();

        //Update ProductMaster.
        public UserMasterViewModel UpdateUserMaster(UserMasterViewModel userMasterViewModel)
        {
            try
            {
                userMasterViewModel.ModifiedBy = LoginUserId();
                UserModel userMasterModel = _userMasterDAL.UpdateUserMaster(userMasterViewModel.ToModel<UserModel>());
                return IsNotNull(userMasterModel) ? userMasterModel.ToViewModel<UserMasterViewModel>() : (UserMasterViewModel)GetViewModelWithErrorMessage(new UserMasterListViewModel(), GeneralResources.UpdateErrorMessage);
            }
            catch (Exception ex)
            {
                CoditechFileLogging.LogMessage(ex.Message, CoditechComponents.Components.ProductMaster.ToString());
                return (UserMasterViewModel)GetViewModelWithErrorMessage(userMasterViewModel, GeneralResources.UpdateErrorMessage);
            }
        }
    }
}
