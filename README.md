# issuu.net

A simple API client for issuu. You need a premium Account for API access.

### NuGet
    Install-Package issuu.Client -Version 1.0.31

### Usage
    var client = new IssuuClient(options =>
    {
        options.Credentials.ApiKey = "your-api-key";
        options.Credentials.ApiSecret = "your-api-secret";
    });

    var documents = await client.GetDocumentsAsync(o =>
    {
        o.PageSize = 20;
        o.StartIndex = 0;
    });
