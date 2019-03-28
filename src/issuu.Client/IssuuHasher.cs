using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace issuu_dotnet
{

    public class IssuuHasher
    {

        public static string CalculateSignature(List<KeyValuePair<string, string>> values, string apiSecret)
        {
            var paramsJoined = string.Join("", values.OrderBy(p => p.Key).Select(p => p.Key + p.Value));

            var hash = CalculateMD5Hash(apiSecret + paramsJoined);

            return hash.ToLower();
        }

        private static string CalculateMD5Hash(string input)
        {
            // Source: https://devblogs.microsoft.com/csharpfaq/how-do-i-calculate-a-md5-hash-from-a-string/
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }


}
