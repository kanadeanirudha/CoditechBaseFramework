
using System.ComponentModel.DataAnnotations;

namespace Coditech.ViewModel
{
    public class GeneralDepartmentViewModel : BaseViewModel
    {
        public short GeneralDepartmentMasterId { get; set; }
        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        [Required]
        [Display(Name = "Short Code")]
        public string DepartmentShortCode { get; set; }
        [Required]
        [Display(Name = "Print Short Desc")]
        public string PrintShortDesc { get; set; }

        [Display(Name = "Work Activity")]
        public bool WorkActivity { get; set; }
    }
}
