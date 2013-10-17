using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WindowsAzure.ResourceProviderDataLayer
{
    public class ProvisioningResult
    {
        public string CacheUrl { get; set; }
    }

    public class HashTagRedisProvisioningClient
    {
        private static string ProvisionServiceUrl = "http://hashtagredis.cloudapp.net/api/redis";


        public ProvisioningResult Provision(string instanceId)
        {
            using (HttpClientHandler httpMessageHandler = new HttpClientHandler())
            {
                // Serialize the payload
                string serializedMessage = JsonConvert.SerializeObject(new { instanceId = instanceId });
                using (HttpContent httpContent = new StringContent(serializedMessage, Encoding.UTF8, "application/json"))
                {
                    using (HttpClient httpClient = new HttpClient(httpMessageHandler))
                    {
                        Task<HttpResponseMessage> postTask = httpClient.PostAsync(new Uri(ProvisionServiceUrl + "?json=" + serializedMessage), httpContent);

                        postTask.Wait();

                        if (postTask.Result.IsSuccessStatusCode)
                        {
                            Task<string> readTheContentTask = postTask.Result.Content.ReadAsStringAsync();
                            readTheContentTask.Wait();

                            return new ProvisioningResult()
                            {
                                CacheUrl = JsonConvert.DeserializeObject<string>(readTheContentTask.Result)
                            };
                        }
                        else
                        {
                            throw new Exception("Cannot Provision - " + postTask.Result.ReasonPhrase);
                        }
                    }
                }
            }
        }

        public void DeProvision(string instanceId)
        {
            using (HttpClientHandler httpMessageHandler = new HttpClientHandler())
            {
                // Serialize the payload
                string serializedMessage = JsonConvert.SerializeObject(new { instanceId = instanceId });
                
                using (HttpClient httpClient = new HttpClient(httpMessageHandler))
                {
                    Task<HttpResponseMessage> deleteTask = httpClient.DeleteAsync(new Uri(ProvisionServiceUrl + "?json=" + serializedMessage));

                    deleteTask.Wait();
                }
            }
        }
    }
}
