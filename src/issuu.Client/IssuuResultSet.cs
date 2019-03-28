using Newtonsoft.Json;

namespace issuu.Client
{
    public class IssuuResultSet<T>  where T : IIssuuData
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("more")]
        public bool More { get; set; }

        [JsonProperty("_content")]
        public IssuuResult<T>[] Results { get; set; }


    }
}
