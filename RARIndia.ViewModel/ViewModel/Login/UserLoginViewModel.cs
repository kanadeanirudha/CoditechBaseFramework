
using System.ComponentModel.DataAnnotations;

namespace Coditech.ViewModel
{
    public class UserLoginViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [MaxLength(100, ErrorMessage = "User name can not exceed 100 characters.")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [MaxLength(100, ErrorMessage = "Email Address can not exceed 100 characters.")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Please enter a valid password.")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}