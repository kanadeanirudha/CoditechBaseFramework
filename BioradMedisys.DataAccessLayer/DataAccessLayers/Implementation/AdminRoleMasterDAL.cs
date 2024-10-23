using Coditech.DataAccessLayer.DataEntity;
using Coditech.DataAccessLayer.Repository;
using Coditech.Model;
using Coditech.Utilities.Helper;
using System.Collections.Specialized;
using System.Linq;
namespace Coditech.DataAccessLayer
{
    public class AdminRoleMasterDAL
    {
        private readonly ICoditechRepository<AdminRoleMaster> _adminRoleMasterRepository;
        public AdminRoleMasterDAL()
        {
            _adminRoleMasterRepository = new CoditechRepository<AdminRoleMaster>();
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
        #region Private Method

        #endregion
    }
}
