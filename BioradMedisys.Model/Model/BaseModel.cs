using System;

namespace Coditech.Model
{
    public class BaseModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool HasError { get; set; } = false;
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }
}
