using System.Collections.Generic;

namespace Coditech.ViewModel
{
    public class GeneralDepartmentListViewModel : BaseViewModel
    {

        public List<GeneralDepartmentViewModel> GeneralDepartmentList { get; set; }

        public GeneralDepartmentListViewModel()
        {
            GeneralDepartmentList = new List<GeneralDepartmentViewModel>();
        }
    }
}
