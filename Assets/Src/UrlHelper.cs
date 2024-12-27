using System.Collections.Generic;
using System.Linq;

namespace Src
{
    public static class UrlHelper
    {
        public static string AddQueryParametersToUrl(string url, Dictionary<string, string> queryParameters)
        {
            var queryString = string.Join("&", queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            return $"{url}?{queryString}";
        }
    }
}