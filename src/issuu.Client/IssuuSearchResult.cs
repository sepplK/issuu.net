using System;
using System.Collections.Generic;
using System.Text;

namespace issuu.Client
{

    public class IssuuSearchResultSet
    {
        public int Start { get; set; }
        public double MaxScore { get; set; }
        public int NumFound { get; set; }

        public IssuuSearchResultDoc[] Docs { get; set; }
    }

    public class IssuuSearchResultDoc : IIssuuData
    {
        public string Username { get; set; }
        public string DocName { get; set; }
        public string DocumentId { get; set; }
    }
}
