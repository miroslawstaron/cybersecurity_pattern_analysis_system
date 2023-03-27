protected IEnumerable<SelectQuery> GetPaginatedSelectQueries(int partialRowCount, Table table, IEnumerable<Column> selectColumns, Condition topBorderFilter = null, Condition bottomBorderFilter = null, int customPageSize = 0)
{
    var psize = customPageSize > 0 ? customPageSize : this.pagesize;
    var dataQueries = new List<SelectQuery>();
    var pages = partialRowCount / psize;
    var lastPageSize = partialRowCount % psize;
    // add one page if there is a rest
    if (lastPageSize > 0) { pages++; }
    var sortOrderFields = this.GetDefaultSortOrder();
    for (var i = 0; i < pages; i++)
    {
        var from = i * psize;
        // first pages : last page
        var to = (i < pages - 1 || lastPageSize < 1) ? (psize * (i + 1)) : from + lastPageSize;
        var fetchAmount = to - from;

        dataQueries.Add(new SelectQuery(selectColumns, table, Condition.Combine(bottomBorderFilter, topBorderFilter), sortOrderFields) { Range = new Range(from, to) });
    }
    return dataQueries;
}