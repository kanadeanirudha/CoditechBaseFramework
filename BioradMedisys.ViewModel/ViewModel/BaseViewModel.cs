using System;

namespace Coditech.ViewModel
{
    public abstract class BaseViewModel
    {
        public BaseViewModel()
        {
            PageListViewModel = new PageListViewModel();
        }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public PageListViewModel PageListViewModel { get; set; }
    }
}