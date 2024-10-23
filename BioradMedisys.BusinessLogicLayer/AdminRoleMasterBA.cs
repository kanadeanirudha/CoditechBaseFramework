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
    }
}
