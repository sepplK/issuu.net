using Newtonsoft.Json;
using System;

namespace issuu_dotnet
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

    public class IssuuException : Exception
    {

        public IssuuException(IssuuError error)
        {
            Error = error;
        }

        public IssuuError Error { get; }

        public override string Message => Error.ToString();

    }

    public class IssuuError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }

        public override string ToString()
        {
            return $"Error {Code}, {Message}, Field {Field}";
        }
    }
}
