using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace isuuu.Tests
{

    public class CacheTests
    {

        [Fact]
        public async void DICacheTest()
        {
            var serviceProvider = TestHelpers.BuildServiceProvider((services) =>
            {
                services.AddMemoryCache();
            });

            var options = new IssuuRequestOptions();
            options.PageSize = 5;
            options.Cache = true;

            var client = serviceProvider.GetService<IssuuClient>();

            var firstResult = await client.GetDocumentsAsync(options);

            Assert.Equal(5, firstResult.Results.Count());

            await Task.Delay(3000);

            var secondResult = await client.GetDocumentsAsync(options);

            Assert.Equal(firstResult.ResultTime, secondResult.ResultTime);

            await Task.Delay(3000);

            var thirdResult = await client.GetDocumentsAsync(options);

            Assert.Equal(firstResult.ResultTime, thirdResult.ResultTime);
        }

    }
}
