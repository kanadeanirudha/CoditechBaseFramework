using Coditech.DataAccessLayer;
using Coditech.Model;
using Coditech.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Coditech.DropdownHelper
{
    public static class DropdownHelper
    {
        private static readonly AdminRoleMasterDAL _adminRoleMasterDAL = new AdminRoleMasterDAL();

        public static DropdownViewModel GeneralDropdownList(DropdownViewModel dropdownViewModel)
        {
            List<SelectListItem> dropdownList = new List<SelectListItem>();
            if (Equals(dropdownViewModel.DropdownType, "User"))
            {
                GetUserRoleDropDown(dropdownViewModel, dropdownList);
            }
            dropdownViewModel.DropdownList = dropdownList;
            return dropdownViewModel;
        }

        private static void GetUserRoleDropDown(DropdownViewModel dropdownViewModel, List<SelectListItem> dropdownList)
        {
            AdminRoleMasterListModel AdminRoleMasterList = _adminRoleMasterDAL.GetAdminRoleList();
            dropdownList.Add(new SelectListItem() { Text = "-------Select Role Name-------" });
            foreach (var item in AdminRoleMasterList.AdminRoleMasterList)
            {
                dropdownList.Add(new SelectListItem()
                {
                    Text = item.RoleName,
                    Value = item.AdminRoleMasterId.ToString()
                    //Selected = dropdownViewModel.DropdownSelectedValue == Convert.ToString(item.IsActive)
                });
            }

        }
    }
}