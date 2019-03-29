using issuu.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.IO;

namespace isuuu.Tests
{
    public class TestHelpers
    {

        public static IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure = null)
        {
            var services = new ServiceCollection();

            services.AddIssuuClient(options =>
            {
                // Credentials via environment variable
                var envCredentials = Environment.GetEnvironmentVariable("IssuuApiCredentials", EnvironmentVariableTarget.Machine);
                if(string.IsNullOrEmpty(envCredentials))
                {
                    envCredentials = Environment.GetEnvironmentVariable("IssuuApiCredentials", EnvironmentVariableTarget.Process);
                }

                if(!string.IsNullOrEmpty(envCredentials))
                {
                    options.Credentials = JsonConvert.DeserializeObject<IssuuCredentials>(envCredentials);
                    return;
                }

                // Credentials via json file
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

            configure?.Invoke(services);

            return services.BuildServiceProvider();
        }
    }
}
