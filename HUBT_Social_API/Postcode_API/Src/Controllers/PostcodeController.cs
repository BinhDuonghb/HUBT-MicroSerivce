using AutoMapper;
using HUBT_Social_Base;
using HUBT_Social_Core.Models.DTOs.EmailDTO;
using HUBT_Social_Core.Settings;
using HUBT_Social_Email_Service.Services;
using HUBT_Social_MongoDb_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using Postcode_API.Src.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Postcode_API.Src.Controllers
{
    [Route("api/postcode")]
    [ApiController]
    public class PostcodeController(
        IMongoService<Postcode> postcode,
        IOptions<JwtSetting> option,
        IEmailService emailService,
        IMapper mapper) : DataLayerController(mapper, option)
    {
        private readonly IMongoService<Postcode> _PostcodeService = postcode;
        private readonly IEmailService _emailService = emailService;
        [HttpPost]
        public async Task<IActionResult> SendPostcodeAsync(EmailRequest request)
        {
            return Ok(await _emailService.SendEmailAsync(request));
        }
    }
}
