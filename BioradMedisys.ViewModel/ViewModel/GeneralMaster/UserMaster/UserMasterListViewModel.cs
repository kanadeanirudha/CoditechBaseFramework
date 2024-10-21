using System.Collections.Generic;

namespace Coditech.ViewModel
{
public  class UserMasterListViewModel : BaseViewModel
    {
        public List<UserMasterViewModel> UserMasterList { get; set; }

        public UserMasterListViewModel()
        {
            UserMasterList = new List<UserMasterViewModel>();
        }
        public string FilterBy { get; set; }
    }
}
