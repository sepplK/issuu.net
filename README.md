# issuu.net

A simple API client for issuu. You need a premium Account for API access.

### NuGet
    Install-Package issuu.Client

### Usage


**Dependency Injection**

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddIssuuClient();
    }

**Load Documents**

    public class MyClient
    {
        private readonly IssuuClient _client;

        public MyClient(IssuuClient client)
        {
            _client = client;
            _client.Options.Credentials.ApiKey = "your-api-key";
            _client.Options.Credentials.ApiSecret = "your-api-secret";
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
