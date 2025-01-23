using AutoMapper;
using HUBT_Social_Base;
using HUBT_Social_Core.Models.Requests;
using HUBT_Social_Core.Models.Requests.LoginRequest;
using HUBT_Social_Core.Settings;
using HUBT_Social_Identity_Service.Services;
using HUBT_Social_Identity_Service.Services.IdentityCustomeService;
using Identity_API.Src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Identity_API.Src.Controllers
{
    [Route("api/identity/auth")]
    [ApiController]
    public class IdentityAuthController(IHubtIdentityService<AUser, ARole> identityService,
        IMapper mapper,
        IOptions<JwtSetting> options) : DataLayerController(mapper, options)
    {
        private readonly IAuthenService<AUser, ARole> _identityAuthService = identityService.AuthService;

        [HttpPost("vertifile-account")]
        public async Task<IActionResult> SignInAsync([FromBody] LoginByUserNameRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Identifier) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(ModelState);
            }
            var (result, _) = await _identityAuthService.LoginAsync(model);

            return Ok(result);
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> SignUpAsync([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(LocalValue.Get(KeyStore.InvalidInformation));
            var (result, _) = await _identityAuthService.RegisterAsync(model);

            return Ok(result);
        }
    }
}
