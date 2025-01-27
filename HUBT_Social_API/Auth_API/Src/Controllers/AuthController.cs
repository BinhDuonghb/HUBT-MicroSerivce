using Auth_API.Src.Models;
using Auth_API.Src.Services.Identity;
using Auth_API.Src.Services.Postcode;
using Auth_API.Src.Services.TempUser;
using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Base.Helpers;
using HUBT_Social_Core;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.EmailDTO;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using HUBT_Social_Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Auth_API.Src.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService,
        ITempUserRegister tempUserRegister,
        IPostcodeService postcodeService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITempUserRegister _tempUserRegister = tempUserRegister;
        private readonly IPostcodeService _postcodeService = postcodeService;

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(RegisterRequest request)
        {
            string userAgent = Request.Headers.UserAgent.ToString();
            string? ipAddress = ServerHelper.GetIPAddress(HttpContext);
            if (ipAddress == null) return BadRequest(LocalValue.Get(KeyStore.InvalidInformation));
            
            if (!ModelState.IsValid) 
                return BadRequest(LocalValue.Get(KeyStore.InvalidCredentials));
            ResponseDTO resultTemp = await _tempUserRegister.StoreIn(request);
            if (resultTemp.StatusCode == HttpStatusCode.OK)
            {
                if (!await _authService.IsUsed(request))
                {
                    CreatePostcodeRequest createPostcodeRequest = new()
                    {
                        IpAddress = ipAddress,
                        UserAgent = userAgent,
                        Receiver = request.Email,
                    };
                    ResponseDTO resultCreatePostcode = await _postcodeService.CreatePostcodeAsync(createPostcodeRequest);
                    PostCodeDTO? postCodeDTO = resultCreatePostcode.ConvertTo<PostCodeDTO>();
                    if (postCodeDTO == null)
                        return BadRequest(resultCreatePostcode.Message);
                    EmailRequest emailRequest = new()
                    {
                        Code = postCodeDTO.Code,
                        Subject = LocalValue.Get(KeyStore.EmailVerificationCodeSubject),
                        ToEmail = request.Email,
                        FullName = request.UserName,
                        Device = userAgent,
                        Location = await ServerHelper.GetLocationFromIpAsync(ipAddress),
                        DateTime = ServerHelper.ConvertToCustomString(DateTime.UtcNow)
                    };
                    ResponseDTO resultSendEmail = await _postcodeService.SendPostcodeAsync(emailRequest);
                    if (resultSendEmail.StatusCode == HttpStatusCode.OK) 
                        return Ok(resultSendEmail.Message);
                }
                return BadRequest(LocalValue.Get(KeyStore.UserAlreadyExists));
                
            } 
            return BadRequest(resultTemp.Message);
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
