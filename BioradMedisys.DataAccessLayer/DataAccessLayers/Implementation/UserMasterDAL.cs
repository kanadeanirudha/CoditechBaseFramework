using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;
using System.Linq;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.DataAccessLayer
{
    public class UserMasterDAL : BaseDataAccessLogic
    {
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<AdminRoleMaster> _roleMasterRepository;
        public UserMasterDAL()
        {
            _userMasterRepository = new CoditechRepository<UserMaster>();
            _roleMasterRepository = new CoditechRepository<AdminRoleMaster>();
        }

        #region Public Method
        public UserModel Login(UserModel userModel)
        {
            if (IsNull(userModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x => x.UserName == userModel.UserName && x.Password == userModel.Password);

            if (IsNull(userMasterData))
                throw new CoditechException(ErrorCodes.NotFound, null);
            else if (!userMasterData.IsActive)
                throw new CoditechException(ErrorCodes.ContactAdministrator, null);

            userModel = userMasterData?.FromEntityToModel<UserModel>();
            return userModel;
        }

        public UserMasterListModel GetUserList()
        {
            UserMasterListModel listModel = new UserMasterListModel();
            listModel.UserMasterList = (from user in _userMasterRepository.Table
                                        join y in _roleMasterRepository.Table
                                        on user.AdminRoleMasterId equals y.AdminRoleMasterId
                                        select new UserModel
                                        {
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                            IsActive = user.IsActive,
                                            AdminRoleMasterId = (short)user.AdminRoleMasterId,
                                            IsDocumentApprovalAuthority = (bool)user.IsDocumentApprovalAuthority,
                                            RoleName = y.RoleName,
                                            UserMasterId = user.UserMasterId,
                                        }).ToList();
            return listModel;
        }


        //Get UserMaster by UserMaster id.
        public UserModel GetUserMaster(int userMasterId)
        {
            if (userMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "userMasterId"));

            //Get the UserMaster Details based on id.
            UserMaster userMasterData = _userMasterRepository.Table.FirstOrDefault(x => x.UserMasterId == userMasterId);
            UserModel userMasterModel = userMasterData.FromEntityToModel<UserModel>();
            return userMasterModel;
        }

        //Update UserMaster.
        public UserModel UpdateUserMaster(UserModel userMasterModel)
        {
            if (IsNull(userMasterModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (userMasterModel.UserMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ProductMasterID"));

          UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.UserMasterId == userMasterModel.UserMasterId)?.FirstOrDefault();
            
            //Update UserMaster
            bool isUserMasterUpdated = _userMasterRepository.Update(userMasterData);
            if (!isUserMasterUpdated)
            {
                userMasterModel.HasError = true;
                userMasterModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return userMasterModel;
        }

        #endregion
    }
}
