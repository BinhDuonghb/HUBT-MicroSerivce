using Auth_API.Src.Models;
using Auth_API.Src.Services.Identity;
using Auth_API.Src.Services.TempUser;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Core;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using HUBT_Social_Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Auth_API.Src.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService,ITempUserRegister tempUserRegister) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITempUserRegister _tempUserRegister = tempUserRegister;


        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(RegisterRequest request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(LocalValue.Get(KeyStore.InvalidCredentials));
            ResponseDTO result = await _tempUserRegister.StoreIn(request);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                if (!await _authService.IsUsed(request))
                {
                    return Ok(result.Message);
                }
                return BadRequest(LocalValue.Get(KeyStore.UserAlreadyExists));
                
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
                    return tokenResponse != null ? Ok(tokenResponse) : BadRequest(TokenResult.Message);
                }
                return dataSignIn.Result.Succeeded ? Ok(dataSignIn) : BadRequest(result.Message);
            }
            return BadRequest(result.Message);

        }
    }
}
