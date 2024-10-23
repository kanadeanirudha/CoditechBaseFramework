using System.ComponentModel.DataAnnotations;

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
    }
}
