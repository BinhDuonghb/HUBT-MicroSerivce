using Auth_API.Src.Models;
using Auth_API.Src.Services.Identity;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Core;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Auth_API.Src.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController(IAuthService authService) : CoreController
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
            SignInResultModel? signInResult = result.ConvertTo<SignInResultModel>();
            if (signInResult != null)
            {
                return signInResult.Succeeded ? Ok(signInResult) : BadRequest(result.Data);
            }
            return BadRequest(result.Message);

        }
    }
}
