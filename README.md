# issuu.net

A simple c# api client for your issuu.com premium account

<img src="https://ci.appveyor.com/api/projects/status/xnhkw6dwit645nnd/branch/master?svg=true">


#### Usage

1. Get your issuu premium account on www.issuu.com
2. Add NuGet package "issuu.Client"

#### NuGet
    Install-Package issuu.Client
    

#### Dependency Injection

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddIssuuClient(options =>
        {
            options.Credentials.ApiKey = "your-api-key";
            options.Credentials.ApiSecret = "your-api-secret";
        });
    }

#### Load Documents

    public class MyTest
    {
        private readonly IssuuClient _client;

        public MyClient(IssuuClient client)
        {
            _client = client;
        }

        public async Task ExecuteAsync()
        {

            var response = await _client.GetDocumentsAsync(o =>
            {
                o.PageSize = 20;
                o.StartIndex = 0;
            });

            foreach (var result in response.Results)
            {
                Console.WriteLine(result.Document.Title);
            }

        }
    }
