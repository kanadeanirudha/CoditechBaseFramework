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
    
    public partial class UserModuleMaster : CoditechEntityBaseModel
    {
        public byte UserModuleMasterId { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public Nullable<bool> ModuleInstalledFlag { get; set; }
        public Nullable<bool> ModuleActiveFlag { get; set; }
        public Nullable<int> ModuleSeqNumber { get; set; }
        public string ModuleRelatedWith { get; set; }
        public string ModuleTooltip { get; set; }
        public string ModuleIconName { get; set; }
        public string ModuleIconPath { get; set; }
        public string ModuleFormName { get; set; }
        public string ModuleColorClass { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
