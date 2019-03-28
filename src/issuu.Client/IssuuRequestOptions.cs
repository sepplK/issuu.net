namespace issuu_dotnet
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

    }

}
