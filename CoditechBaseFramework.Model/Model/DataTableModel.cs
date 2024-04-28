namespace Coditech.Model.Model
{
    public class DataTableModel
    {
        public string SearchBy { get; set; }
        public string SortByColumn { get; set; }
        public string SortBy { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SelectedCentreCode { get; set; } = string.Empty;
        public int SelectedDepartmentID { get; set; }
    }
}
