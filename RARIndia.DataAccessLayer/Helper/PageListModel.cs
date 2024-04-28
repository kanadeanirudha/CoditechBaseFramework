using Coditech.DataAccessLayer.Constants;
using Coditech.Utilities.Filters;
using Coditech.Utilities.Helper;

using System;
using System.Collections.Specialized;

namespace Coditech.DataAccessLayer.Helper
{
    public class PageListModel
    {
        #region private variables
        private readonly FilterCollection _filters;
        private readonly NameValueCollection _sorts;
        #endregion

        #region constructor
        public PageListModel(FilterCollection filters, NameValueCollection sorts, int pagingStart, int pagingLength)
        {
            _filters = filters;
            _sorts = sorts;
            PagingStart = pagingStart;
            PagingLength = pagingLength;
        }
        #endregion

        #region Public Properties
        public int PagingStart;
        public int PagingLength;
        public int TotalRowCount;
        public string OrderBy
        {
            get
            {
                return DynamicClauseHelper.GenerateDynamicOrderByClause(_sorts);
            }
        }

        public string SPWhereClause
        {
            get
            {
                return DynamicClauseHelper.GenerateDynamicWhereClauseForSP(_filters.ToFilterDataCollection());
            }
        }

        public EntityWhereClauseModel EntityWhereClause
        {
            get
            {
                return DynamicClauseHelper.GenerateDynamicWhereClauseWithFilter(_filters.ToFilterDataCollection());
            }
        }
        #endregion
    }
}
