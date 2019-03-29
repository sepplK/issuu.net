# issuu.net

1. Get your Premium Account on www.issuu.com
2. Add NuGet Package "issuu.Client"

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
