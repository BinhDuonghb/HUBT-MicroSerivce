using HUBT_Social_Base;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Base.Service;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using HUBT_Social_Core.Settings.@enum;
using System.Net;
using System.Xml.Linq;

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
        //public async Task<ResponseDTO> TwoFactorSignIn(LoginByUserNameRequest request)
        //{
        //    string path = "auth/vertifile-account";
        //    return await SendRequestAsync(path, ApiType.POST, request);
        //}

        //public async Task<ResponseDTO> TwoFactorSignUp(RegisterRequest request)
        //{
        //    string path = "auth/create-account";
        //    return await SendRequestAsync(path, ApiType.POST, request);
        //}
        public async Task<bool> IsUsed(RegisterRequest request)
        {
            string path = $"user";
            ResponseDTO result = await SendRequestAsync(path, ApiType.GET);
            List<AUserDTO>? userDTO = result.ConvertTo<List<AUserDTO>>();
            if (userDTO != null && result.StatusCode == HttpStatusCode.OK)
            {

                if (!string.IsNullOrEmpty(request.Email))
                {
                    List<AUserDTO>? userDTO1;
                    userDTO1 = userDTO.Where(user => user.Email == request.Email).ToList();
                    if (userDTO1.Count != 0) return true;   
                }
                if (!string.IsNullOrEmpty(request.UserName))
                {
                    List<AUserDTO>? userDTO2;
                    userDTO2 = userDTO.Where(user => user.UserName == request.UserName).ToList();
                    if (userDTO2.Count != 0) return true;
                }
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }
            return false;
        }
        public async Task<ResponseDTO> TokenSubcriber(string userId)
        {
            string path = "token";
            return await SendRequestAsync(path, ApiType.POST, userId);
        }
    }
}
