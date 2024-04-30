using System.Collections.Generic;

namespace Coditech.Model
{
    public class ProductMasterListModel : BaseListModel
    {
        public List<ProductMasterModel> ProductMasterList { get; set; }
        public ProductMasterListModel()
        {
            ProductMasterList = new List<ProductMasterModel>();
        }
    }
}
