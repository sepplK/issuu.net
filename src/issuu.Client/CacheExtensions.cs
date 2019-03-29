//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.Extensions.DependencyInjection;

//namespace issuu.Client
//{
//    public static class CacheExtensions
//    {


//        public IssuuResultSet<T> Cache<T>(this IssuuResultSet<T> resultSet, IssuuCacheOptions options) where T : IIssuuData
//        {
//            options = options ?? new IssuuCacheOptions();

//            var cacheKey = $"IssuuResultSet_{resultSet.Url.GetHashCode()}";

//            var cache = resultSet.Client.ServiceProvider.GetService<IMemoryCache>();
//            return cache.GetOrCreate(cacheKey, (k) =>
//            {
//                k.AbsoluteExpiration = DateTime.Now.AddSeconds(options.Expiration);

//                return resultSet;
//            });
//        }

//    }

//    public class IssuuCacheOptions
//    {

//        /// <summary>
//        /// Expiration in ms (default 60000)
//        /// </summary>
//        public int Expiration { get; set; } = 60000;

//    }
//}
