using System;

namespace issuu_dotnet
{
    public class IssuuClient
    {

        public IssuuClient()
        {

        }

        public IssuuClient(IssuuCredentials credentials)
        {
            Credentials = credentials;
        }

        public IssuuCredentials Credentials { get; set; }


    }

}
