using Coditech.Model;
using Coditech.Model.Model;
using Coditech.Utilities.Constant;
using Coditech.Utilities.Helper;
using Coditech.ViewModel;

using System;
using System.Collections.Specialized;

using static Coditech.Utilities.Helper.CoditechHelperUtility;
namespace Coditech.BusinessLogicLayer
{
    public abstract class BaseBusinessLogic
    {
        protected static NameValueCollection SortingByCreatedDate(string sortBy = CoditechConstant.DESCKey)
        {
            NameValueCollection sortlist = new NameValueCollection();
            sortlist.Add(SortKeys.CreatedDate, sortBy);
            return sortlist;
        }

        protected static NameValueCollection SortingByModifiedDate(string sortBy = CoditechConstant.DESCKey)
        {
            NameValueCollection sortlist = new NameValueCollection();
            sortlist.Add(SortKeys.ModifiedDate, sortBy);
            return sortlist;
        }

        protected static NameValueCollection SortingData(string sort, string sortBy)
        {
            NameValueCollection sortlist = new NameValueCollection();
            sortlist.Add(sort, sortBy);
            return sortlist;
        }
        protected void SetListPagingData(PageListViewModel pageListViewModel, BaseListModel listModel, DataTableModel dataTableModel, int totalRecordCount)
        {
            pageListViewModel.Page = Convert.ToInt32(listModel.PageIndex);
            pageListViewModel.RecordPerPage = Convert.ToInt32(listModel.PageSize);
            pageListViewModel.TotalPages = Convert.ToInt32(listModel.TotalPages);
            pageListViewModel.TotalResults = Convert.ToInt32(listModel.TotalResults);
            pageListViewModel.TotalRecordCount = Convert.ToInt32(totalRecordCount);
            pageListViewModel.SearchBy = dataTableModel.SearchBy ?? string.Empty;
            pageListViewModel.SortByColumn = dataTableModel.SortByColumn ?? string.Empty;
            pageListViewModel.SortBy = dataTableModel.SortBy ?? string.Empty;
        }

        #region Session
        /// <summary>
        /// Saves an object in session.
        /// </summary>
        /// <typeparam name="T">The type of object being saved.</typeparam>
        /// <param name="key">The key for the session object.</param>
        /// <param name="value">The value of the session object.</param>
        protected void SaveInSession<T>(string key, T value)
        {
            CoditechSessionHelper.SaveDataInSession<T>(key, value);
        }

        protected T GetFromSession<T>(string key)
        {
            return CoditechSessionHelper.GetDataFromSession<T>(key);
        }

        /// <summary>
        /// Removes an object from session.
        /// </summary>
        /// <param name="key">The key of the session object.</param>
        protected void RemoveInSession(string key)
        {
            CoditechSessionHelper.RemoveDataFromSession(key);
        }
        #endregion

        #region Cookie
        /// <summary>
        /// Gets a cookie value.
        /// </summary>
        /// <param name="key">The key for the value being retrieved.</param>
        /// <returns>The value for the key.</returns>
        protected string GetFromCookie(string key)
        {
            string value = CoditechCookieHelper.GetCookieValue<string>(key);
            return IsNull(value) ? string.Empty : value;
        }

        /// <summary>
        /// Saves a cookie value.
        /// </summary>
        /// <param name="key">The key for the cookie value.</param>
        /// <param name="value">The cookie value.</param>
        protected void SaveInCookie(string key, string value) =>
            CoditechCookieHelper.SetCookie(key, value, CoditechConstant.MinutesInAYear);

        ///// <summary>
        ///// Removes a cookie value.
        ///// </summary>
        ///// <param name="key">The key for the cookie value being removed.</param>
        //protected void RemoveCookie(string key) =>
        //     RARIndiaCookieHelper.RemoveCookie(key);

        #endregion

        /// <summary>
        /// Get BaseViewModel with HasError and ErrorMessage set.
        /// </summary>
        /// <param name="viewModel">View model to set.</param>
        /// <param name="errorMessage">Error message to set.</param>
        /// <returns>Returns BaseViewModel with HasError and ErrorMessage set.</returns>
        protected BaseViewModel GetViewModelWithErrorMessage(BaseViewModel viewModel, string errorMessage)
        {
            viewModel.HasError = true;
            viewModel.ErrorMessage = errorMessage;
            return viewModel;
        }

        protected int LoginUserId() => CoditechSessionHelper.GetDataFromSession<UserModel>(CoditechConstant.UserDataSession).UserMasterId;
    }
}

