using Common.Contract.RequestsAndResponses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Contract.Logic
{
    public static class HttpClientHelper
    {
        public static async Task<TJsonResult> SendGetRequest<TJsonResult>(HttpClient httpClient, string endpoint, Dictionary<string, string> queryParams, string accessToken, CancellationToken cancellationToken) where TJsonResult : BaseResponse
        {
            return await SendHttpRequestAsync<TJsonResult>(httpClient, HttpMethod.Get, endpoint, accessToken, queryParams, cancellationToken: cancellationToken);
        }

        public static async Task<TJsonResult> SendPostRequest<TJsonResult>(HttpClient httpClient, string endpoint, Dictionary<string, string> bodyParams, CancellationToken cancellationToken) where TJsonResult : BaseResponse
        {
            var httpContent = new FormUrlEncodedContent(bodyParams);
            return await SendHttpRequestAsync<TJsonResult>(httpClient, HttpMethod.Post, endpoint, httpContent: httpContent, cancellationToken: cancellationToken);
        }

        //public static async Task SendPutRequest(HttpClient httpClient, string endpoint, Dictionary<string, string> queryParams, object body, string accessToken, CancellationToken cancellationToken)
        //{
        //    var bodyJson = JsonConvert.SerializeObject(body);
        //    var httpContent = new StringContent(bodyJson, Encoding.UTF8, "application/json");
        //    await SendHttpRequestAsync<dynamic>(httpClient, HttpMethod.Put, endpoint, accessToken, queryParams, httpContent, cancellationToken: cancellationToken);
        //}

        private static async Task<TJsonResult> SendHttpRequestAsync<TJsonResult>(HttpClient httpClient, HttpMethod httpMethod, string endpoint, string accessToken = null,
            Dictionary<string, string> queryParams = null, HttpContent httpContent = null, CancellationToken cancellationToken = default) where TJsonResult : BaseResponse
        {
            var url = queryParams != null
                ? AddQueryString(endpoint, queryParams)
                : endpoint;

            var request = new HttpRequestMessage(httpMethod, url);

            if (accessToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            if (httpContent != null)
            {
                request.Content = httpContent;
            }

            using var response = await httpClient.SendAsync(request, cancellationToken);

            var resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Code {response.StatusCode} - {resultJson}");
            }

            var result = JsonConvert.DeserializeObject<TJsonResult>(resultJson);
            return result;
        }

        #region decompiled code from Microsoft.AspNetCore.WebUtilities

        private static string AddQueryString(string uri, IDictionary<string, string> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (queryString == null)
            {
                throw new ArgumentNullException("queryString");
            }

            return AddQueryString(uri, (IEnumerable<KeyValuePair<string, string>>)queryString);
        }

        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (queryString == null)
            {
                throw new ArgumentNullException("queryString");
            }

            int num = uri.IndexOf('#');
            string text = uri;
            string value = "";
            if (num != -1)
            {
                value = uri.Substring(num);
                text = uri.Substring(0, num);
            }

            bool flag = text.IndexOf('?') != -1;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(text);
            foreach (KeyValuePair<string, string> item in queryString)
            {
                stringBuilder.Append(flag ? '&' : '?');
                stringBuilder.Append(UrlEncoder.Default.Encode(item.Key));
                stringBuilder.Append('=');
                stringBuilder.Append(UrlEncoder.Default.Encode(item.Value));
                flag = true;
            }

            stringBuilder.Append(value);
            return stringBuilder.ToString();
        }

        #endregion
    }
}
