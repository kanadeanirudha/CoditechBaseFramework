namespace Coditech.Model
{
    public class ProductMasterModel : BaseModel
    {
        public int ProductMasterId { get; set; }
        public string ProductName { get; set; }
        public string ProductUniqueCode { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
    }
}
