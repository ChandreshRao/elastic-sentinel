using ElasticSentinel.Application.Common.Abstractions;
using ElasticSentinel.Application.Common.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Web;

namespace ElasticSentinel.Application.Features.ElasticQueries;

/// <summary>
/// Executes Elasticsearch queries and transforms the response
/// </summary>
internal sealed class ExecuteElasticQueryHandler : IExecuteElasticQueryHandler
{
    public async Task<List<Dictionary<string, string>>?> HandleAsync(
        ExecuteElasticQueryRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestId = Guid.NewGuid();
        var apiRequest = request.ApiRequest;
        var dictResponseMap = request.ResponseMap;
        var logger = request.Logger;

        try
        {
            if (apiRequest == null)
            {
                return null;
            }

            StringBuilder stringBuilder = new();
            stringBuilder.Append(apiRequest.ElasticHost);
            if (apiRequest.QuerySuffixes != null)
            {
                foreach (string suffix in apiRequest.QuerySuffixes)
                {
                    stringBuilder.Append('/');
                    stringBuilder.Append(HttpUtility.UrlEncode(suffix));
                }
            }
            string strUrl = stringBuilder.ToString();

            using var handler = new HttpClientHandler();
            handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(apiRequest.AuthType,
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{apiRequest.UserName}:{apiRequest.Password}")));

            if (apiRequest.QueryParams != null)
            {
                strUrl = QueryHelpers.AddQueryString(strUrl, apiRequest.QueryParams);
            }

            logger.LogInformation($"Request Id : {requestId} Query Posted to Elasticsearch");
            using var response = await client.GetAsync(strUrl, cancellationToken);
            logger.LogInformation($"Request Id : {requestId} response received from Elasticsearch - " + DateTime.UtcNow);
            string apiResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            if (apiResponse != null)
            {
                var lstDocs = GetQueryResponseDocs(apiResponse, dictResponseMap);
                return lstDocs;
            }

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError("Request Id " + requestId + " Error while running query : Error - " + ex.Message + " Inner exception: " + ex.InnerException?.Message, ex);
            return null;
        }
    }

    private static List<Dictionary<string, string>> MergeElements(
        List<Dictionary<string, string>> parentDictionaries,
        List<Dictionary<string, string>> childDictionaries)
    {
        foreach (var dictChild in childDictionaries)
        {
            foreach (var dictParent in parentDictionaries)
            {
                dictParent.Concat(dictChild);
            }
        }
        return parentDictionaries;
    }

    private static List<Dictionary<string, string>> MergeElements(
        List<Dictionary<string, string>> parentDictionaries,
        Dictionary<string, string> childDictionary)
    {
        foreach (var dictParent in parentDictionaries)
        {
            _ = dictParent.Concat(childDictionary);
        }
        return parentDictionaries;
    }

    private static List<Dictionary<string, string>>? GetQueryResponseDocs(
        string strResponse,
        Dictionary<string, Dictionary<string, string>> dictResponseFields)
    {
        var queryResponse = JObject.Parse(strResponse);

        if (queryResponse != null)
        {
            List<List<Dictionary<string, string>>> newListDocs = new();
            foreach (var root in dictResponseFields)
            {
                string strRoot = root.Key;
                var fields = root.Value;

                var docs = queryResponse.SelectTokens(strRoot);

                if (docs != null && docs.Any())
                {
                    List<Dictionary<string, string>> dictListDocs = new();
                    foreach (var doc in docs)
                    {
                        dictListDocs.Add(GetNestedPropertyObject(doc, fields));
                    }
                    newListDocs.Add(dictListDocs);
                }
            }

            List<Dictionary<string, string>>? lstTransformedDocs = newListDocs.FirstOrDefault();
            if (newListDocs.Count > 1)
            {
                newListDocs.Sort((a, b) => a.Count - b.Count);
                List<Dictionary<string, string>> lstCombinedDocs = new();
                for (int i = 1; i < newListDocs.Count; i++)
                {
                    if (newListDocs[i].Count > 1)
                    {
                        lstCombinedDocs = MergeElements(lstTransformedDocs!, newListDocs[i]);
                        continue;
                    }
                    lstCombinedDocs = MergeElements(lstTransformedDocs!, newListDocs[i].FirstOrDefault()!);
                }
                lstTransformedDocs = lstCombinedDocs;
            }
            return lstTransformedDocs;
        }
        return null;
    }

    private static Dictionary<string, string> GetNestedPropertyObject(
        JToken jToken,
        IDictionary<string, string> dictPropertyMapper)
    {
        Dictionary<string, string> dict = new();
        foreach (var entry in dictPropertyMapper)
        {
            var propertyValue = jToken.SelectToken(entry.Key)?.ToString();
            dict.Add(entry.Value, propertyValue ?? string.Empty);
        }
        return dict;
    }
}
