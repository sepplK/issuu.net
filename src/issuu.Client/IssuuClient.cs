using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace issuu_dotnet
{

    public class IssuuClient
    {

        public IssuuClient()
        {
        }

        public IssuuClient(Action<IssuuOptions> configure) : this()
        {
            configure.Invoke(Options);
        }

        public IssuuOptions Options { get; set; } = new IssuuOptions();


        public Task<IssuuResultSet<T>> GetDataAsync<T>(Action<IssuuRequestOptions> configure = null) where T : IIssuuData
        {
            var options = new IssuuRequestOptions();
            configure?.Invoke(options);
            return GetDataAsync<T>(options);
        }

        public async Task<IssuuResultSet<T>> GetDataAsync<T>(IssuuRequestOptions options = null) where T : IIssuuData
        {

            var urlParams = new Dictionary<string, string>();

            var properties = typeof(T).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<JsonPropertyAttribute>()
                })
                .Where(p => p.Attribute != null)
                .ToList();

            urlParams["action"] = "issuu.documents.list";
            urlParams["apiKey"] = Options.Credentials.ApiKey;
            urlParams["access"] = "public";
            urlParams["responseParams"] = string.Join(",", properties.Select(p => p.Attribute.PropertyName));
            urlParams["format"] = "json";

            urlParams["startIndex"] = options.StartIndex.ToString();
            urlParams["pageSize"] = options.PageSize.ToString();

            var urlParamsSorted = urlParams.OrderBy(p => p.Key).ToList();

            var signature = IssuuHasher.CalculateSignature(urlParamsSorted, Options.Credentials.ApiSecret);

            urlParamsSorted.Add(new KeyValuePair<string, string>("signature", signature));

            var url = $@"{Options.ApiUrl}?{string.Join("&", urlParamsSorted.Select(p => p.Key + "=" + p.Value))}";

            var webClient = new WebClient();
            var resultString = await webClient.DownloadStringTaskAsync(url);
            JObject resultJson = JsonConvert.DeserializeObject(resultString) as JObject;

            var result = resultJson["rsp"]["_content"]["result"].ToObject<IssuuResultSet<T>>();

            return result;
        }

    }

}
