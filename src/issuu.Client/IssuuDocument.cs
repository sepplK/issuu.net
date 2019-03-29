using Newtonsoft.Json;
using System;

namespace issuu.Client
{

    public class IssuuDocument : IIssuuData
    {
        public IssuuDocument()
        {

        }

        internal IssuuDocument(IssuuSearchResultDocument doc)
        {
            DocumentId = doc.DocumentId;
            Name = doc.DocName;
            Title = doc.Title;
            Description = doc.Description;
            Username = doc.Username;
        }

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

        [JsonProperty("publishDate")]
        public DateTime PublishDate { get; set; }

        [JsonProperty("folders")]
        public string[] Folders { get; set; }

        public string Url
        {
            get
            {
                return $"https://issuu.com/{Username}/docs/{Name}";
            }
        }

        public string MediaUrl
        {
            get
            {
                return $"http://image.issuu.com/{DocumentId}/jpg/page_1.jpg";
            }
        }
    }

}
