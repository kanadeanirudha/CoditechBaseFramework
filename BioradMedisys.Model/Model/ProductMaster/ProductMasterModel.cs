using System;

namespace Coditech.Model
{
    public class ProductMasterModel : BaseModel
    {
        public int ProductMasterId { get; set; }
        public string ProductName { get; set; }
        public string ProductUniqueCode { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
        public string Version { get; set; }
        public DateTime? Date { get; set; }
        public int DownloadCount { get; set; }
    }
}
