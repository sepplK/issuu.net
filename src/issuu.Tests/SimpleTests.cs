using issuu.Client;
using System;
using System.Linq;
using Xunit;

namespace isuuu.Tests
{
    public class SimpleTests
    {

        [Fact]
        public async void PagingTest()
        {
            var client = new IssuuClient();

            // Take first 5
            var documents5 = await client.GetDocumentsAsync(o =>
            {
                o.PageSize = 5;
                o.StartIndex = 0;
            });

            Assert.Equal(5, documents5.Results.Count());

            // Take last 3
            var startIndex = documents5.TotalCount - 3;
            var documentsLast = await client.GetDocumentsAsync(o =>
            {
                o.PageSize = 15;
                o.StartIndex = startIndex;
            });

            Assert.Equal(3, documentsLast.Results.Count());
            Assert.False(documentsLast.More);
            Assert.Equal(startIndex, documentsLast.StartIndex);
        }

        [Fact]
        public async void ExceptionTest()
        {
            var client1 = new IssuuClient(options =>
            {
                options.Credentials.ApiKey = "invalidapikey";
            });

            try
            {
                var results = await client1.GetDocumentsAsync();
            }
            catch (Exception ex) 
            {
                Assert.IsType<IssuuException>(ex);

                var issuuException = ex as IssuuException;

                Assert.Equal("201", issuuException.Error.Code);
                Assert.Equal("Invalid field format", issuuException.Error.Message);
                Assert.Equal("apiKey", issuuException.Error.Field);
            }

        }


    }
}
