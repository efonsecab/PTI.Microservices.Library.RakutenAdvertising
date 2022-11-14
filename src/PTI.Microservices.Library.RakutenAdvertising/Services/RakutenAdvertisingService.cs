using Microsoft.Extensions.Logging;
using PTI.Microservices.Library.Configuration;
using PTI.Microservices.Library.Interceptors;
using PTI.Microservices.Library.Models.RakutenAdvertisingService.GetBearerToken;
using PTI.Microservices.Library.Models.RakutenAdvertisingService.SearchProduct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PTI.Microservices.Library.Services
{
    /// <summary>
    /// Service in charge of exposing acces to Rakuten Advertising functionality
    /// </summary>
    public sealed class RakutenAdvertisingService
    {
        private ILogger<RakutenAdvertisingService> Logger { get; }
        private RakutenAdvertisingConfiguration RakutenAdvertisingConfiguration { get; }
        private CustomHttpClient CustomHttpClient { get; }

        /// <summary>
        /// Creates a new instance of <see cref="RakutenAdvertisingService"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="rakutenAdvertisingConfiguration"></param>
        /// <param name="customHttpClient"></param>
        public RakutenAdvertisingService(ILogger<RakutenAdvertisingService> logger,
            RakutenAdvertisingConfiguration rakutenAdvertisingConfiguration,
            CustomHttpClient customHttpClient)
        {
            this.Logger = logger;
            this.RakutenAdvertisingConfiguration = rakutenAdvertisingConfiguration;
            this.CustomHttpClient = customHttpClient;
        }

        /// <summary>
        /// Gets the Bearer Token
        /// </summary>
        /// <returns></returns>
        public async Task<GetBearerTokenResponse> GetBearerTokenAsync()
        {
            string requestUrl = $"https://api.rakutenmarketing.com/token";
            this.CustomHttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("basic",
                this.RakutenAdvertisingConfiguration.TokenRequestAuthorizationSecret);
            List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            formData.Add(new KeyValuePair<string, string>("username", this.RakutenAdvertisingConfiguration.Username));
            formData.Add(new KeyValuePair<string, string>("password", this.RakutenAdvertisingConfiguration.Password));
            formData.Add(new KeyValuePair<string, string>("scope", this.RakutenAdvertisingConfiguration.Id));
            System.Net.Http.FormUrlEncodedContent body = new System.Net.Http.FormUrlEncodedContent(formData);
            var response = await this.CustomHttpClient.PostAsync(requestUrl, body);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GetBearerTokenResponse>();
                return result;
            }
            else
            {
                string reason = response.ReasonPhrase;
                string detailedError = await response.Content.ReadAsStringAsync();
                throw new Exception($"Reason: {reason}. Details: {detailedError}");
            }

        }

        /// <summary>
        /// Searches product by the given keyword
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="advertiserId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<SearchProductResponse> SearchProductAsync(string keyword, string advertiserId, int page = 1)
        {
            string requestUrl = $"{this.RakutenAdvertisingConfiguration.BaseUrl}/productsearch/1.0" +
                $"?keyword={keyword}" +
                $"&mid={advertiserId}" +
                $"&pagenumber={page}";
            var getBearerTokenResult = await this.GetBearerTokenAsync();
            this.CustomHttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", getBearerTokenResult.access_token);
            var response = await this.CustomHttpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var stringresponse = await response.Content.ReadAsStringAsync();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SearchProductResponse));
                SearchProductResponse result = null;
                using (TextReader reader = new StringReader(stringresponse))
                {
                    try
                    {
                        result = xmlSerializer.Deserialize(reader) as SearchProductResponse;
                    }
                    catch (Exception ex)
                    {
                        this.Logger.LogError(ex.Message, ex);
                        throw;
                    }
                }
                return result;
            }
            else
            {
                string reason = response.ReasonPhrase;
                string detailedError = await response.Content.ReadAsStringAsync();
                throw new Exception($"Reason: {reason}. Details: {detailedError}");
            }
        }
    }
}
