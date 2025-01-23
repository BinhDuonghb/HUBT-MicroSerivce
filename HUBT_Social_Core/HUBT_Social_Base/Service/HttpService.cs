using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Settings;
using HUBT_Social_Core.Settings.@enum;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;


namespace HUBT_Social_Base.Service
{
    public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<ResponseDTO> SendAsync(RequestDTO request)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                
                if (!string.IsNullOrEmpty(request.AccessToken))
                {
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);
                }

                message.RequestUri = new Uri(request.Url);
                if(request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
                HttpResponseMessage? apiResponse = null;

                message.Method = request.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,

                    _ => HttpMethod.Get,
                };
                apiResponse = await client.SendAsync(message);
                ResponseDTO? response = apiResponse.StatusCode switch 
                { 
                    HttpStatusCode.NotFound => new() {Message = LocalValue.Get(KeyStore.ApiNotFound)  },
                    HttpStatusCode.Unauthorized => new() { Message = LocalValue.Get(KeyStore.UnAuthorize) },
                    HttpStatusCode.Forbidden => new() { Message = LocalValue.Get(KeyStore.ApiForbibben) },
                    HttpStatusCode.InternalServerError => new() { Message = LocalValue.Get(KeyStore.InternalServerError) },
                    _ => JsonConvert.DeserializeObject<ResponseDTO>(await apiResponse.Content.ReadAsStringAsync())
                };
                if (response != null)
                {
                    return response;
                }
                return 
                    new ResponseDTO 
                        { 
                            Message = LocalValue.Get(KeyStore.ApiError) 
                        };
                
            }
            catch (Exception)
            {
                return 
                    new ResponseDTO 
                        {  
                            Message = LocalValue.Get(KeyStore.ApiError) 
                        };
            }
        }
    }
}
