using System.Collections.Generic;

namespace Coditech.Model
{
    public class AdminRoleMasterListModel : BaseListModel
    {
        public List<AdminRoleMasterModel> AdminRoleMasterList { get; set; }
        public AdminRoleMasterListModel()
        {
            AdminRoleMasterList = new List<AdminRoleMasterModel>();
        }
    }
}
