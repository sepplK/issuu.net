using Newtonsoft.Json;

namespace issuu.Client
{
    public class IssuuDocument : IIssuuData
    {

        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("ep")]
        public string EP { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("folders")]
        public string[] Folders { get; set; }

        public string MediaUrl
        {
            get
            {
                return $"http://image.issuu.com/{DocumentId}/jpg/page_1.jpg";
            }
        }
    }

}
