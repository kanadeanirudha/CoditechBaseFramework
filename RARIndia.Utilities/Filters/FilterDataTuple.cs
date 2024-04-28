using System;

namespace Coditech.Utilities.Filters
{
    public class FilterDataTuple : Tuple<string, string, string>
    {
        public string FilterName { get { return Item1; } }
        public string FilterOperator { get { return Item2; } }
        public string FilterValue { get { return Item3; } }

        public FilterDataTuple(string filterName, string filterOperator, string filterValue) : base(filterName, filterOperator, filterValue)
        {
        }
    }
}
