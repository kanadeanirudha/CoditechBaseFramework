//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coditech.DataAccessLayer.DataEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class GymPlanDurationMaster : CoditechEntityBaseModel
    {
        public short GymPlanDurationMasterId { get; set; }
        public string PlanDuration { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}
