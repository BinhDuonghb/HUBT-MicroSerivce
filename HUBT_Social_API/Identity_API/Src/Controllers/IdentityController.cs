using AutoMapper;
using HUBT_Social_Base;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Settings;
using HUBT_Social_Identity_Service.Services;
using HUBT_Social_Identity_Service.Services.IdentityCustomeService;
using Identity_API.Src.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace Identity_API.Src.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController(IHubtIdentityService<AUser, ARole> identityService, IMapper mapper, IOptions<JwtSetting> options) : DataLayerController(mapper, options)
    {
        private readonly IUserService<AUser, ARole> _identityService = identityService.UserService;
        [HttpGet("user")]
        public IActionResult GetUser()
        {

            List<AUser>? listUser = _identityService.GetAll();

            if (listUser == null) return BadRequest("User Not Found");

            if (listUser.Count > 0)
            {
                var userDTOs = listUser.Select(user => _mapper.Map<AUserDTO>(user)).ToList();

                return Ok(userDTOs);
                
            }
            return BadRequest("User Not Found");

        }
        [HttpPut("update-user")]
        public async Task<IActionResult> Update(AUser user)
        {
            if (await _identityService.UpdateUserAsync(user))
            {
                return Ok(user);
            }
            return BadRequest("User NotFound");
        }
        [HttpDelete("Delete-user")]
        public async Task<IActionResult> Delete(AUser user)
        {
            if  (await _identityService.DeleteUserAsync(user))
            {
                return Ok(user);
            }
            return BadRequest("User NotFound");
        }

    }
}
