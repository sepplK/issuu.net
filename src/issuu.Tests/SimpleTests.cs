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
    public class SimpleTests
    {

        public static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddIssuuClient();

            services.Configure<IssuuOptions>(options =>
            {

                // Test credentials
                var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

                while (directory != null)
                {
                    var configFile = new FileInfo($@"{directory}\IssuuCredentials.json");
                    if (configFile.Exists)
                    {
                        options.Credentials = JsonConvert.DeserializeObject<IssuuCredentials>(File.ReadAllText(configFile.FullName));
                        break;
                    }

                    directory = directory.Parent;
                }


            });

            return services.BuildServiceProvider();
        }

        [Fact]
        public async void PagingTest()
        {
            var serviceProvider = BuildServiceProvider();

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
            var serviceProvider = BuildServiceProvider();

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
