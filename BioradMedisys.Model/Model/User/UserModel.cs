﻿namespace Coditech.Model
{
    public class UserModel : BaseModel
    {
        public UserModel()
        {
           
        }
        public int UserMasterId { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public byte? AdminRoleMasterId { get; set; }
        public bool IsDocumentApprovalAuthority { get; set; }
        public string RoleName { get; set; }

    }
}
