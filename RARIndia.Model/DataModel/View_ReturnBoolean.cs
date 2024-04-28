using Coditech.DataAccessLayer.DataEntity;

using System;

namespace Coditech.Model
{
    public partial class View_ReturnBoolean : CoditechEntityBaseModel
    {
        public int Id { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
