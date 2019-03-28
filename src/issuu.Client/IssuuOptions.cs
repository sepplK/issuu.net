using Newtonsoft.Json;
using System.IO;

namespace issuu_dotnet
{

    public class IssuuOptions
    {
        public IssuuOptions()
        {
            LoadDefaultCredentials();
        }

        /// <summary>
        /// The API credentials
        /// </summary>
        public IssuuCredentials Credentials { get; set; }

        /// <summary>
        /// The API url (http://api.issuu.com/1_0)
        /// </summary>
        public string ApiUrl { get; set; } = "http://api.issuu.com/1_0";


        private void LoadDefaultCredentials()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (directory != null)
            {
                var configFile = new FileInfo($@"{directory}\IssuuCredentials.json");
                if (configFile.Exists)
                {
                    Credentials = JsonConvert.DeserializeObject<IssuuCredentials>(File.ReadAllText(configFile.FullName));
                    break;
                }

                directory = directory.Parent;
            }
        }

    }

}
