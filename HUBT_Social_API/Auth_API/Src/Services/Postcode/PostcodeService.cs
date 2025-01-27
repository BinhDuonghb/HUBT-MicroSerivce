using HUBT_Social_Base;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Base.Service;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.EmailDTO;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Settings.@enum;
using System.Net;

namespace Auth_API.Src.Services.Postcode
{
    public class PostcodeService(IHttpService httpService, string basePath) : BaseService(httpService, basePath), IPostcodeService
    {
        
        public async Task<ResponseDTO> CreatePostcodeAsync(CreatePostcodeRequest request)
        {
            return await SendRequestAsync("create-postcode", ApiType.POST, request);
        }
        public async Task<PostCodeDTO?> GetCurrentPostCode(PostcodeRequest request)
        {
            ResponseDTO? response = await SendRequestAsync($"current-postcode?" +
                $"UserAgent={request.UserAgent}&IpAddress={request.IpAddress}",
                ApiType.GET);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                PostCodeDTO? postcode = response.ConvertTo<PostCodeDTO>();
                return postcode;
            } 
            return null;
        }
        public async Task<ResponseDTO> SendPostcodeAsync(EmailRequest request)
        {
            return await SendRequestAsync("send-postcode", ApiType.POST, request);
        }
    }
}
