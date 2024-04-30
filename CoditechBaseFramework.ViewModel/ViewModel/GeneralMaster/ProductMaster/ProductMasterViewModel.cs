
using System.ComponentModel.DataAnnotations;

namespace Coditech.ViewModel
{
    public class ProductMasterViewModel : BaseViewModel
    {
        public int ProductMasterId { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public string ProductName { get; set; }
        public string ProductUniqueCode { get; set; }
        public string FileName { get; set; }
        public bool IsActive { get; set; }
    }
}
