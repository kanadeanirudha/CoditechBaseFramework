using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Coditech.ViewModel
{
    public class AdminRoleMasterViewModel : BaseViewModel
    {
        public byte AdminRoleMasterId { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        [MaxLength(50)]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Form Access")]
        public List<string> SelectedFormAccess { get; set; }
        public List<SelectListItem> FormList { get; set; }
    }
}
