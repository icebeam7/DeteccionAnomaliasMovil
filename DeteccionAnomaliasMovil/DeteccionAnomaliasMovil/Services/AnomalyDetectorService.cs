using System;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Newtonsoft.Json;

using DeteccionAnomaliasMovil.Models;
using DeteccionAnomaliasMovil.Helpers;

namespace DeteccionAnomaliasMovil.Services
{
    public static class AnomalyDetectorService
    {
        private static readonly HttpClient client = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(Constants.Endpoint);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.SubscriptionKey);
            return client;
        }

        public async static Task<PriceResult> AnalyzeData(PriceInfo priceInfo)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    | SecurityProtocolType.Tls11 
                    | SecurityProtocolType.Tls;

                var jsonInfo = JsonConvert.SerializeObject(priceInfo);
                var content = new StringContent(jsonInfo, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(Constants.DetectAnomaliesServiceURL, content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var priceResult = JsonConvert.DeserializeObject<PriceResult>(jsonResult);
                    return priceResult;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}
