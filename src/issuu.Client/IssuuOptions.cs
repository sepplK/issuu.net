using Newtonsoft.Json;
using System.IO;

namespace issuu.Client
{

    public class IssuuOptions
    {

        /// <summary>
        /// The API credentials
        /// </summary>
        public IssuuCredentials Credentials { get; set; } = new IssuuCredentials();

        /// <summary>
        /// The API url (https://api.issuu.com/1_0)
        /// </summary>
        public string ApiUrl { get; set; } = "https://api.issuu.com/1_0";

        /// <summary>
        /// The Search API url (https://search.issuu.com/api/2_0/document)
        /// </summary>
        public string SearchApiUrl { get; set; } = "https://search.issuu.com/api/2_0/document";

    }

}
