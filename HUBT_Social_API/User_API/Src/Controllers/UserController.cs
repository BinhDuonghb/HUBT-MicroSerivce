using HUBT_Social_Base.ASP_Extentions;
using HUBT_Social_Core;
using HUBT_Social_Core.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_API.Src.Service;

namespace User_API.Src.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : CoreController
    {
        private readonly IUserService _identityService = userService;
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] string? id)
        {
            ResponseDTO result = await _identityService.GetUser();
            List<AUserDTO>? userDTO = result.ConvertTo<List<AUserDTO>>();
            if (userDTO != null)
            {

                if (!string.IsNullOrEmpty(name))
                {
                    userDTO = userDTO.Where(user => user.UserName == name).ToList();
                }

                if (!string.IsNullOrEmpty(id))
                {
                    userDTO = userDTO.Where(user => user.Id.ToString() == id).ToList();
                }
                return Ok(userDTO);

            }
            return BadRequest(result.Message);

        }
    }
}
