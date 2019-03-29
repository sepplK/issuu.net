using Newtonsoft.Json;
using System;

namespace issuu.Client
{

    public class IssuuResultSet<T>  where T : IIssuuData
    {
        public IssuuResultSet()
        {
            ResultTime = DateTime.Now;
        }

        [JsonIgnore]
        public IssuuClient Client { get; set; }

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

        [JsonIgnore]
        public string Url { get; set; }

        [JsonIgnore]
        internal DateTime ResultTime { get; set; }
    }
}
