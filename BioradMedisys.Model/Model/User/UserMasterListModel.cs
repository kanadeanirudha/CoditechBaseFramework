using System.Collections.Generic;

namespace Coditech.Model
{
    public class UserMasterListModel : BaseListModel
    {
        public List<UserMasterModel> UserMasterList { get; set; }
        public UserMasterListModel()
        {
            UserMasterList = new List<UserMasterModel>();
        }
    }
}
