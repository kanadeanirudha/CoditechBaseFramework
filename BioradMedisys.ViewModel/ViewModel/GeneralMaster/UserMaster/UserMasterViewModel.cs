using System.Collections.Generic;
using System.Web.Mvc;

namespace Coditech.ViewModel
{
    public class UserMasterViewModel : BaseViewModel
    {
        public UserMasterViewModel()
        {
            AdminRoleMasterList = new List<SelectListItem>();
        }
        public List<SelectListItem> AdminRoleMasterList { get; set; }
        public int UserMasterId { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public short AdminRoleMasterId { get; set; }
        public bool IsDocumentApprovalAuthority { get; set; }
        public string RoleName { get; set; }
    }
}
