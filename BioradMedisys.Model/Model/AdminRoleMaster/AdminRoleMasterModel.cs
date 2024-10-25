using System.Collections.Generic;

namespace Coditech.Model
{
    public class AdminRoleMasterModel : BaseModel
    {
        public byte AdminRoleMasterId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public List<string> SelectedFormAccess { get; set; }
    }
}
