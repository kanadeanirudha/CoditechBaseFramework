using System.Collections.Generic;

namespace Coditech.ViewModel
{
    public class AdminRoleMasterListViewModel : BaseViewModel
    {
        public List<AdminRoleMasterViewModel> AdminRoleMasterList { get; set; }

        public AdminRoleMasterListViewModel()
        {
            AdminRoleMasterList = new List<AdminRoleMasterViewModel>();
        }
    }
}
