﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Brew_Stash.RestClient
{
    public class RestClient<T>
    {
        public async Task<T> GetAsync(string WebServiceUrl)
        {
            try
            {
                var httpClient = new HttpClient();

                var json = await httpClient.GetStringAsync(WebServiceUrl);

                var taskModels = JsonConvert.DeserializeObject<T>(json);

                return taskModels;
            }
            catch (Exception e)
            {
                Log("GetAsync error: " + e.Message);
                return default(T);
            }

        }

        private void Log(string v)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PostAsync(T t, string WebServiceUrl)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(int id, T t, string WebServiceUrl)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(WebServiceUrl + id, httpContent);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, T t, string WebServiceUrl)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(WebServiceUrl + id);

            return response.IsSuccessStatusCode;
        }
    }
}
