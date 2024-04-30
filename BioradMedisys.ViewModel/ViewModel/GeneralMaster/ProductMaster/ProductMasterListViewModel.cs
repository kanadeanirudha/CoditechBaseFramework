using System.Collections.Generic;

namespace Coditech.ViewModel
{
    public class ProductMasterListViewModel : BaseViewModel
    {

        public List<ProductMasterViewModel> ProductMasterList { get; set; }

        public ProductMasterListViewModel()
        {
            ProductMasterList = new List<ProductMasterViewModel>();
        }
    }
}
