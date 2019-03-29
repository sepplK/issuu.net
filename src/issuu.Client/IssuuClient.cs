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
using Microsoft.Extensions.Options;

namespace issuu.Client
{

    public class IssuuClient
    {

        public IssuuClient()
        {

        }

        public IssuuClient(IOptions<IssuuOptions> options)
        {
            Options = options.Value;
        }

        public IssuuOptions Options { get; set; }

        public Task<IssuuResultSet<IssuuDocument>> GetDocumentsAsync(Action<IssuuRequestOptions> configure = null)
        {
            return GetDataAsync<IssuuDocument>(configure);
        }

        public Task<IssuuResultSet<T>> GetDataAsync<T>(Action<IssuuRequestOptions> configure = null) where T : IIssuuData
        {
            var options = new IssuuRequestOptions();
            configure?.Invoke(options);
            return GetDataAsync<T>(options);
        }

        /// <summary>
        /// Return all documents 
        /// </summary>
        public async Task<IssuuResultSet<T>> GetAllDataAsync<T>() where T : IIssuuData
        {
            var allDocuments = new List<T>();

            var pageSize = 100;

            var result = await GetDataAsync<T>(o =>
            {
                o.PageSize = pageSize;
            });

            var remainingCount = result.TotalCount - pageSize;
            var pages = Math.Ceiling(remainingCount / (decimal)pageSize);

            var tasks = new List<Task<IssuuResultSet<T>>>();

            for (int page = 1; page <= pages; page++)
            {
                tasks.Add(GetDataAsync<T>(o =>
                {
                    o.PageSize = pageSize;
                    o.StartIndex = page * pageSize;
                }));
            }

            var asyncResults = await Task.WhenAll(tasks);

            result.Results = result.Results.Concat(asyncResults.SelectMany(r => r.Results)).ToArray();

            return result;
        }

        public Task<IssuuResultSet<IssuuDocument>> SearchAsync(string query, Action<IssuuRequestOptions> configure = null)
        {
            var options = new IssuuRequestOptions();
            configure?.Invoke(options);

            return SearchAsync(query, options);
        }

        public async Task<IssuuResultSet<IssuuDocument>> SearchAsync(string query, IssuuRequestOptions options = null)
        {
            if (options == null) options = new IssuuRequestOptions();

            var urlParams = new Dictionary<string, string>();

            urlParams["q"] = query;

            if (!string.IsNullOrEmpty(Options.Credentials.ApiUsername))
            {
                urlParams["username"] = Options.Credentials.ApiUsername;
            }

            urlParams["startIndex"] = options.StartIndex.ToString();
            urlParams["pageSize"] = options.PageSize.ToString();

            var url = $@"{Options.SearchApiUrl}?{string.Join("&", urlParams.Select(p => p.Key + "=" + p.Value))}";

            var webClient = new WebClient();
            var resultString = await webClient.DownloadStringTaskAsync(url);
            JObject resultJson = JsonConvert.DeserializeObject(resultString) as JObject;

            var resultContent = resultJson["response"];

            var searchResultSet = resultContent.ToObject<IssuuSearchResultSet>();

            var result = new IssuuResultSet<IssuuDocument>();

            result.Results = searchResultSet.Docs
                .Select(d => new IssuuResult<IssuuDocument>(new IssuuDocument(d)))
                .ToArray();

            result.PageSize = options.PageSize;
            result.TotalCount = searchResultSet.NumFound;
            result.StartIndex = searchResultSet.Start;

            return result;
        }

        public async Task<IssuuResultSet<T>> GetDataAsync<T>(IssuuRequestOptions options = null) where T : IIssuuData
        {
            if (options == null) options = new IssuuRequestOptions();

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
            urlParams["documentSortBy"] = options.SortBy;
            urlParams["resultOrder"] = options.SortOrder.ToString().ToLower();

            urlParams["startIndex"] = options.StartIndex.ToString();
            urlParams["pageSize"] = options.PageSize.ToString();

            urlParams["title"] = "sommer";

            var urlParamsSorted = urlParams.OrderBy(p => p.Key).ToList();

            var signature = IssuuHasher.CalculateSignature(urlParamsSorted, Options.Credentials.ApiSecret);

            urlParamsSorted.Add(new KeyValuePair<string, string>("signature", signature));

            var url = $@"{Options.ApiUrl}?{string.Join("&", urlParamsSorted.Select(p => p.Key + "=" + p.Value))}";

            var webClient = new WebClient();
            var resultString = await webClient.DownloadStringTaskAsync(url);
            JObject resultJson = JsonConvert.DeserializeObject(resultString) as JObject;

            var resultContent = resultJson["rsp"]["_content"];

            if (resultContent["error"] != null)
            {
                var issuuError = resultContent["error"].ToObject<IssuuExceptionDetails>();
                throw new IssuuException(issuuError);
            }

            var result = resultJson["rsp"]["_content"]["result"].ToObject<IssuuResultSet<T>>();

            return result;
        }

    }

}
