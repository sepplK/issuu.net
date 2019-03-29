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

        public IssuuDocument[] Docs { get; set; }
    }

}
