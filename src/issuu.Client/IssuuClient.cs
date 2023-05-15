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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace issuu.Client
{

    public class IssuuClient
    {

        public IssuuClient()
        {

        }

        public IssuuClient(IOptions<IssuuOptions> options, IServiceProvider serviceProvider)
        {
            Options = options.Value;
            ServiceProvider = serviceProvider;
        }

        public IssuuOptions Options { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public Task<IssuuResultSet<IssuuDocument>> GetDocumentsAsync(Action<IssuuRequestOptions> configure = null, CancellationToken cancellationToken = default)
        {
            return GetDataAsync<IssuuDocument>(configure, cancellationToken);
        }

        public async Task<IssuuDocument> GetDocumentByIdAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var page = 0;
            var pageSize = 100;
            var loadedCount = 0;

            while (page < 10)
            {
                var results = await GetDocumentsAsync(options =>
                {
                    options.PageSize = pageSize;
                    options.StartIndex = page * pageSize;
                }, cancellationToken);

                loadedCount += results.Results.Count();

                var doc = results.Results
                    .FirstOrDefault(r => r.Document?.DocumentId == documentId)?.Document;

                if(doc != null)
                {
                    return doc;
                }

                if(loadedCount >= results.TotalCount || !results.Results.Any())
                {
                    break;
                }

                page++;
            }

            return null;
        }

        public Task<IssuuResultSet<IssuuDocument>> GetDocumentsAsync(IssuuRequestOptions options, CancellationToken cancellationToken = default)
        {
            return GetDataAsync<IssuuDocument>(options, cancellationToken);
        }

        public Task<IssuuResultSet<T>> GetDataAsync<T>(Action<IssuuRequestOptions> configure = null, CancellationToken cancellationToken = default) where T : IIssuuData
        {
            var options = new IssuuRequestOptions();
            configure?.Invoke(options);
            return GetDataAsync<T>(options, cancellationToken);
        }

        /// <summary>
        /// Return all documents 
        /// </summary>
        public async Task<IssuuResultSet<T>> GetAllDataAsync<T>(CancellationToken cancellationToken = default) where T : IIssuuData
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
                }, cancellationToken));
            }

            var asyncResults = await Task.WhenAll(tasks);

            result.Results = result.Results.Concat(asyncResults.SelectMany(r => r.Results)).ToArray();
            
            return result;
        }

        public async Task<IssuuResultSet<T>> GetDataAsync<T>(IssuuRequestOptions options = null, CancellationToken cancellationToken = default) where T : IIssuuData
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

            var cacheKey = $"IssuuResultSet_{url.GetHashCode()}";
            var cache = ServiceProvider.GetService<IMemoryCache>();
            if (options.Cache && cache != null)
            {
                var cachedResultSet = cache.Get(cacheKey) as IssuuResultSet<T>;
                if(cachedResultSet != null)
                {
                    return cachedResultSet;
                }
            }

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url, cancellationToken))
            {
                var resultString = await response.Content.ReadAsStringAsync();
                JObject resultJson = JsonConvert.DeserializeObject(resultString) as JObject;

                var resultContent = resultJson["rsp"]["_content"];

                if (resultContent["error"] != null)
                {
                    var issuuError = resultContent["error"].ToObject<IssuuExceptionDetails>();
                    throw new IssuuException(issuuError);
                }

                var result = resultJson["rsp"]["_content"]["result"].ToObject<IssuuResultSet<T>>();
                result.Client = this;
                result.Url = url;

                if (options.Cache && cache != null)
                {
                    cache.Set(cacheKey, result, DateTime.Now.AddMilliseconds(options.CacheTime));
                }

                return result;
            }

        }

    }

}
