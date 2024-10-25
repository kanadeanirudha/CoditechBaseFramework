using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Repository;
using Coditech.ExceptionManager;
using Coditech.Model;
using Coditech.Resources;
using Coditech.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.DataAccessLayer
{
    public class AdminRoleMasterDAL
    {
        private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;
        private readonly ICoditechRepository<AdminAssociateFormsToRole> _adminAssociateFormsToRoleRepository;
        public AdminRoleMasterDAL()
        {
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>();
            _adminAssociateFormsToRoleRepository = new CoditechRepository<AdminAssociateFormsToRole>();
        }

        public AdminRoleMasterListModel GetAdminRoleList()
        {
            AdminRoleMasterListModel listModel = new AdminRoleMasterListModel();
            listModel.AdminRoleMasterList = (from a in _adminRoleMasterRepository.Table.ToList()
                                             select new AdminRoleMasterModel
                                             {
                                                 AdminRoleMasterId = a.AdminRoleMasterId,
                                                 RoleName = a.RoleName,
                                                 IsActive = a.IsActive,
                                             })?.ToList();
            return listModel;
        }

        public AdminRoleMasterModel CreateAdminRoleMaster(AdminRoleMasterModel adminRoleMasterModel)
        {
            if (IsNull(adminRoleMasterModel))
                throw new CoditechException(ErrorCodes.NullModel, GeneralResources.ModelNotNull);

            if (IsAdminRoleNameAlreadyExist(adminRoleMasterModel.RoleName))
            {
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Admin Role name"));
            }
            //Create new AdminRoleMaster and return it.
            AdminRoleMaster adminRoleMaster = _adminRoleMasterRepository.Insert(adminRoleMasterModel.FromModelToEntity<AdminRoleMaster>());
            if (adminRoleMaster?.AdminRoleMasterId > 0)
            {
                adminRoleMasterModel.AdminRoleMasterId = adminRoleMaster.AdminRoleMasterId;
                List<AdminAssociateFormsToRole> adminAssociateFormsToRoles = new List<AdminAssociateFormsToRole>();
                foreach (string formCode in adminRoleMasterModel.SelectedFormAccess)
                {
                    adminAssociateFormsToRoles.Add(new AdminAssociateFormsToRole()
                    {
                        AdminFormCode = formCode,
                        AdminRoleMasterId=adminRoleMasterModel.AdminRoleMasterId,
                        CreatedBy= adminRoleMasterModel.CreatedBy,
                        CreatedDate= adminRoleMasterModel.CreatedDate
                    });
                }
                _adminAssociateFormsToRoleRepository.Insert(adminAssociateFormsToRoles);
            }
            else
            {
                adminRoleMasterModel.HasError = true;
                adminRoleMasterModel.ErrorMessage = GeneralResources.ErrorFailedToCreate;
            }
            return adminRoleMasterModel;
        }

        //Get AdminRoleMaster by AdminRoleMaster id.
        public AdminRoleMasterModel GetAdminRoleMaster(int adminRoleMasterId)
        {
            if (adminRoleMasterId <= 0)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "AdminRoleMasterID"));

            //Get the AdminRoleMaster Details based on id.
            AdminRoleMaster adminRoleMasterData = _adminRoleMasterRepository.Table.FirstOrDefault(x => x.AdminRoleMasterId == adminRoleMasterId);
            AdminRoleMasterModel adminRoleMasterModel = adminRoleMasterData.FromEntityToModel<AdminRoleMasterModel>();
            if (IsNotNull(adminRoleMasterModel))
            {
                adminRoleMasterModel.SelectedFormAccess = _adminAssociateFormsToRoleRepository.Table.Where(x => x.AdminRoleMasterId == adminRoleMasterId)?.Select(y => y.AdminFormCode)?.ToList();
            }
            return adminRoleMasterModel;
        }

        //Update AdminRoleMaster.
        public AdminRoleMasterModel UpdateAdminRoleMaster(AdminRoleMasterModel adminRoleMasterModel)
        {
            if (IsNull(adminRoleMasterModel))
                throw new CoditechException(ErrorCodes.InvalidData, GeneralResources.ModelNotNull);

            if (adminRoleMasterModel.AdminRoleMasterId < 1)
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "AdminRoleMasterID"));

            if (IsAdminRoleNameAlreadyExist(adminRoleMasterModel.RoleName, adminRoleMasterModel.AdminRoleMasterId))
                throw new CoditechException(ErrorCodes.AlreadyExist, string.Format(GeneralResources.ErrorCodeExists, "Admn Role Name"));

            //Update AdminRoleMaster
            bool isAdminRoleMasterUpdated = _adminRoleMasterRepository.Update(adminRoleMasterModel.FromModelToEntity<AdminRoleMaster>());
            if (!isAdminRoleMasterUpdated)
            {
                adminRoleMasterModel.HasError = true;
                adminRoleMasterModel.ErrorMessage = GeneralResources.UpdateErrorMessage;
            }
            else
            {
                List<AdminAssociateFormsToRole> deleteAdminAssociateFormsToRole = null;
                List<AdminAssociateFormsToRole> insertAdminAssociateFormsToRole = null;
                List<AdminAssociateFormsToRole> dminAssociateFormsToRoleList = _adminAssociateFormsToRoleRepository.Table.Where(x => x.AdminRoleMasterId == adminRoleMasterModel.AdminRoleMasterId)?.ToList();

                foreach (string item in adminRoleMasterModel.SelectedFormAccess)
                {
                    if (!dminAssociateFormsToRoleList.Any(x => x.AdminFormCode.ToString() == item))
                    {
                        if (IsNull(insertAdminAssociateFormsToRole))
                        {
                            insertAdminAssociateFormsToRole = new List<AdminAssociateFormsToRole>();
                        }
                        insertAdminAssociateFormsToRole.Add(new AdminAssociateFormsToRole()
                        {
                            AdminRoleMasterId = adminRoleMasterModel.AdminRoleMasterId,
                            AdminFormCode = item,
                            CreatedBy = adminRoleMasterModel.CreatedBy,
                            CreatedDate = DateTime.Now,
                        });
                    }
                }

                foreach (AdminAssociateFormsToRole item in dminAssociateFormsToRoleList)
                {
                    if (!adminRoleMasterModel.SelectedFormAccess.Any(x => x == item.AdminFormCode.ToString()))
                    {
                        if (IsNull(deleteAdminAssociateFormsToRole))
                        {
                            deleteAdminAssociateFormsToRole = new List<AdminAssociateFormsToRole>();
                        }
                        deleteAdminAssociateFormsToRole.Add(item);
                    }
                }

                if (insertAdminAssociateFormsToRole?.Count > 0)
                {
                    _adminAssociateFormsToRoleRepository.Insert(insertAdminAssociateFormsToRole);
                }

                if (deleteAdminAssociateFormsToRole?.Count > 0)
                {
                    _adminAssociateFormsToRoleRepository.Delete(deleteAdminAssociateFormsToRole);
                }
            }
            return adminRoleMasterModel;
        }

        //Delete AdminRoleMaster.
        public bool DeleteAdminRoleMaster(ParameterModel parameterModel)
        {
            if (IsNull(parameterModel) || string.IsNullOrEmpty(parameterModel.Ids))
                throw new CoditechException(ErrorCodes.IdLessThanOne, string.Format(GeneralResources.ErrorIdLessThanOne, "AdminRoleMasterID"));

            CoditechViewRepository<View_ReturnBoolean> objStoredProc = new CoditechViewRepository<View_ReturnBoolean>();
            objStoredProc.SetParameter("AdminRoleMasterId", parameterModel.Ids, ParameterDirection.Input, DbType.String);
            objStoredProc.SetParameter("Status", null, ParameterDirection.Output, DbType.Int32);
            int status = 0;
            objStoredProc.ExecuteStoredProcedureList("Coditech_DeleteAdminRoleMaster @AdminRoleMasterId,  @Status OUT", 1, out status);

            return status == 1 ? true : false;
        }
        #region Private Method

        //Check if Product Master code is already present or not.
        private bool IsAdminRoleNameAlreadyExist(string adminRoleName, int adminRoleMasterId = 0)
             => _adminRoleMasterRepository.Table.Any(x => x.RoleName == adminRoleName && (x.AdminRoleMasterId != adminRoleMasterId || adminRoleMasterId == 0));

        #endregion
    }
}
