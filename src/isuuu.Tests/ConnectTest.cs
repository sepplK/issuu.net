using issuu_dotnet;
using System;
using System.Linq;
using Xunit;

namespace isuuu.Tests
{
    public class ConnectTest
    {

        [Fact]
        public async void Connect()
        {
            var client = new IssuuClient();

            // Take first 5
            var documents5 = await client.GetDataAsync<IssuuDocument>(o =>
            {
                o.PageSize = 5;
                o.StartIndex = 0;
            });

            Assert.Equal(5, documents5.Results.Count());

            // Take last 3
            var startIndex = documents5.TotalCount - 3;
            var documentsLast = await client.GetDataAsync<IssuuDocument>(o =>
            {
                o.PageSize = 15;
                o.StartIndex = startIndex;
            });

            Assert.Equal(3, documentsLast.Results.Count());
            Assert.False(documentsLast.More);
            Assert.Equal(startIndex, documentsLast.StartIndex);
        }
    }
}
