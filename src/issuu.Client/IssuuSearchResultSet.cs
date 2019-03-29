using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace issuu.Client
{

    public class IssuuSearchResultSet
    {
        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("maxScore")]
        public double MaxScore { get; set; }

        [JsonProperty("numFound")]
        public int NumFound { get; set; }

        [JsonProperty("docs")]
        public IssuuSearchResultDocument[] Docs { get; set; }
    }

    public class IssuuSearchResultDocument
    {
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("docname")]
        public string DocName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

}
