using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;
using System.Collections.Generic;
using System.Linq;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.DataAccessLayer
{
    public class UserMasterDAL : BaseDataAccessLogic
    {
        private readonly ICoditechRepository<UserMaster> _userMasterRepository;
        private readonly ICoditechRepository<AdminRoleMaster> _roleMasterRepository;
        private readonly ICoditechRepository<AdminAssociateFormsToRole> _adminAssociateFormsToRoleRepository;
        public UserMasterDAL()
        {
            _userMasterRepository = new CoditechRepository<UserMaster>();
            _roleMasterRepository = new CoditechRepository<AdminRoleMaster>();
            _adminAssociateFormsToRoleRepository = new CoditechRepository<AdminAssociateFormsToRole>();
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
            if (IsNotNull(userModel))
            {
                userModel.FormAccessList = new List<string>();
                if (userModel.UserType == "SuperAdmin")
                {
                    userModel.FormAccessList.Add("User");
                    userModel.FormAccessList.Add("AdminRoleMaster");
                    userModel.FormAccessList.Add("ProductMaster");
                }
                else
                {
                    List<AdminAssociateFormsToRole> list = _adminAssociateFormsToRoleRepository.Table.Where(x => x.AdminRoleMasterId == userModel.AdminRoleMasterId)?.ToList();
                    foreach (AdminAssociateFormsToRole role in list)
                    {
                        userModel.FormAccessList.Add(role.AdminFormCode);
                    }
                }
            }
            return userModel;
        }

        public UserMasterListModel GetUserList()
        {
            UserMasterListModel listModel = new UserMasterListModel();
            listModel.UserMasterList = (from user in _userMasterRepository.Table
                                        join role in _roleMasterRepository.Table
                                        on user.AdminRoleMasterId equals role.AdminRoleMasterId
                                        into UserRoleGroup //Performing LINQ Group Join
                                        from userrole in UserRoleGroup.DefaultIfEmpty()
                                        where user.UserType != "SuperAdmin"
                                        select new UserModel
                                        {
                                            FirstName = user.FirstName,
                                            LastName = user.LastName,
                                            IsActive = user.IsActive,
                                            AdminRoleMasterId = user.AdminRoleMasterId,
                                            IsDocumentApprovalAuthority = (bool)user.IsDocumentApprovalAuthority,
                                            RoleName = userrole.RoleName,
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
        public UserModel UpdateUserMaster(UserModel userModel)
        {
            if (IsNull(userModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (userModel.UserMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "ProductMasterID"));

            UserMaster userMasterData = _userMasterRepository.Table.Where(x => x.UserMasterId == userModel.UserMasterId)?.FirstOrDefault();
            userMasterData.IsActive = userModel.IsActive;
            userMasterData.IsDocumentApprovalAuthority = userModel.IsDocumentApprovalAuthority;
            userMasterData.AdminRoleMasterId = userModel.AdminRoleMasterId;
            userMasterData.ModifiedBy = userModel.ModifiedBy;
            //Update UserMaster
            bool isUserMasterUpdated = _userMasterRepository.Update(userMasterData);
            if (!isUserMasterUpdated)
            {
                userModel.HasError = true;
                userModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            return userModel;
        }

        #endregion
    }
}
