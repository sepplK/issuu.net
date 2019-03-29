namespace issuu.Client
{
    public class IssuuRequestOptions
    {
        /// <summary>
        /// The page size (default 10)
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// The zero based start index
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// The sort field property (default PublishDate)
        /// </summary>
        public string SortBy { get; set; } = "publishDate";

        /// <summary>
        /// The sort order (default desc)
        /// </summary>
        public IssuuSortOrders SortOrder { get; set; } = IssuuSortOrders.Desc;

        /// <summary>
        /// Returns a document search result (only for Document results)
        /// </summary>
        public string SearchQuery { get; set; }

    }

    public enum IssuuSortOrders
    {
        Asc = 0,
        Desc = 1
    }

}
