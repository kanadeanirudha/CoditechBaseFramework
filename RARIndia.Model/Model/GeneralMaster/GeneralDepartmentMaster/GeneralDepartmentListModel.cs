using System.Collections.Generic;

namespace Coditech.Model
{
    public class GeneralDepartmentListModel : BaseListModel
    {
        public List<GeneralDepartmentModel> GeneralDepartmentList { get; set; }
        public GeneralDepartmentListModel()
        {
            GeneralDepartmentList = new List<GeneralDepartmentModel>();
        }

        public string SelectedDepartmentID { get; set; }
    }
}
