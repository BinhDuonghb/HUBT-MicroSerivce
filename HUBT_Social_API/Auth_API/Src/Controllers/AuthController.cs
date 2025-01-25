using Auth_API.Src.Models;
using Auth_API.Src.Services.Identity;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Core;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Auth_API.Src.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

     
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(RegisterRequest request)
        {
            ResponseDTO result = await _authService.SignUp(request);
            SignUpResultModel? signInResult = result.ConvertTo<SignUpResultModel>();
            if (signInResult != null)
            {
                return signInResult.Succeeded ? Ok(signInResult): BadRequest(result.Data);
            }
            return BadRequest(result.Message);

        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(LoginByUserNameRequest request)
        {
            ResponseDTO result = await _authService.SignIn(request);
            if (result.StatusCode == HttpStatusCode.BadRequest) 
                return BadRequest(result.Message);
            DataSignIn? dataSignIn = result.ConvertTo<DataSignIn>();
            if (dataSignIn != null && dataSignIn.Result != null && dataSignIn.User != null)
            {
                AUserDTO user = dataSignIn.User;
                SignInResultModel signInResult = dataSignIn.Result;
                if (signInResult.Succeeded)
                {
                    ResponseDTO TokenResult = await _authService.TokenSubcriber(user.Id.ToString());
                    TokenResponseDTO? tokenResponse = TokenResult.ConvertTo<TokenResponseDTO>();
                    return tokenResponse != null ? Ok(tokenResponse) : BadRequest(result.Message);
                }
                return dataSignIn.Result.Succeeded ? Ok(dataSignIn) : BadRequest(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
