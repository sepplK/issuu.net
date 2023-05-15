using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace isuuu.Tests
{

    public class ListTests
    {

        [Fact]
        public async void ListTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

            // Take first 50
            var docs = await client.GetDocumentsAsync(o =>
            {
                o.PageSize = 50;
                o.StartIndex = 0;
            });

            Assert.Equal(50, docs.Results.Count());
        }

        [Fact]
        public async void SingleDocumentTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider((services) =>
            {
                services.AddMemoryCache();
            });

            var options = new IssuuRequestOptions();
            options.Cache = true;

            var client = serviceProvider.GetService<IssuuClient>();

            var docId = "170512115916-a5a1d224ccda04e11c95aeb7e5366d4c";
            var doc = await client.GetDocumentByIdAsync(docId);

            await Task.Delay(2000);

            var cachedDoc = await client.GetDocumentByIdAsync(docId);

            Assert.Equal(docId, doc.DocumentId);
        }

        [Fact]
        public async void PagingTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

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
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

            try
            {
                var results = await client.GetDocumentsAsync();
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
