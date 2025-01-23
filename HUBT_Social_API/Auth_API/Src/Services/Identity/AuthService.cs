using HUBT_Social_Base;
using HUBT_Social_Base.Service;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using HUBT_Social_Core.Settings.@enum;

namespace Auth_API.Src.Services.Identity
{
    public class AuthService(IHttpService httpService, string basePath) : BaseService(httpService, basePath), IAuthService
    {
        public async Task<ResponseDTO> SignIn(LoginByUserNameRequest request)
        {
            string path = "auth/vertifile-account";
            return await SendRequestAsync(path,ApiType.POST, request);
        }

        public async Task<ResponseDTO> SignUp(RegisterRequest request)
        {
            string path = "auth/create-account";
            return await SendRequestAsync(path, ApiType.POST, request);
        }
    }
}
