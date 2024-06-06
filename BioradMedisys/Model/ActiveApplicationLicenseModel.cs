using System;

namespace Coditech.Model
{
    public class ActiveApplicationLicenseModel 
    {
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool HasError { get; set; } = false;
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }
}
