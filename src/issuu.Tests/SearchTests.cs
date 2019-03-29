using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Xunit;

namespace isuuu.Tests
{

    public class SearchTests
    {

        [Fact]
        public async void SimpleSearchTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

            var first5Results = await client.GetDocumentsAsync(options =>
            {
                options.SearchQuery = "sommer";
                options.PageSize = 5;
            });

            Assert.Equal(5, first5Results.Results.Count());
            Assert.True(first5Results.TotalCount > first5Results.Results.Count());
        }

        [Fact]
        public async void SearchPagingTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

            var first5Results = await client.GetDocumentsAsync(options =>
            {
                options.SearchQuery = "sommer";
                options.PageSize = 5;
            });

            Assert.Equal(5, first5Results.Results.Count());

            var last3Results = await client.GetDocumentsAsync(options =>
            {
                options.SearchQuery = "sommer";
                options.PageSize = 3;
                options.StartIndex = first5Results.TotalCount - 3;
            });

            Assert.Equal(3, last3Results.Results.Count());
        }

        [Fact]
        public async void IntegratedSearchTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

            var first5Results = await client.GetDataAsync<IssuuDocument>(options =>
            {
                options.PageSize = 5;
                options.SearchQuery = "sommer";
            });

            Assert.Contains("sommer2011", first5Results.Results.First().Document.Name);

            Assert.Equal(5, first5Results.Results.Count());
        }

    }
}
