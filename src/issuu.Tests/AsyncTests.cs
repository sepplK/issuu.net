using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace isuuu.Tests
{
    public class AsyncTests
    {

        [Fact]
        public async void AllDocumentsTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider();

            var client = serviceProvider.GetService<IssuuClient>();

            var allDocuments = await client.GetAllDataAsync<IssuuDocument>();

            Assert.True(allDocuments.TotalCount > 0);
            Assert.Equal(allDocuments.TotalCount, allDocuments.Results.Count());
        }

    }
}
