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
        /// The API url (http://api.issuu.com/1_0)
        /// </summary>
        public string ApiUrl { get; set; } = "http://api.issuu.com/1_0";

    }

}
