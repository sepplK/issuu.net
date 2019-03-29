namespace issuu.Client
{
    public class IssuuCredentials
    {
        /// <summary>
        /// Only required for the search api
        /// </summary>
        public string ApiUsername { get; set; }

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }

}
