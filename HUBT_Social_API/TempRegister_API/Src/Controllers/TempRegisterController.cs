using AutoMapper;
using HUBT_Social_Base;
using HUBT_Social_Core.Models.DTOs;
using HUBT_Social_Core.Models.DTOs.IdentityDTO;
using HUBT_Social_Core.Settings;
using HUBT_Social_MongoDb_Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TempRegister_API.Src.Models;

namespace TempRegister_API.Src.Controllers
{
    [Route("api/TempRegister")]
    [ApiController]
    public class TempRegisterController(
        IMongoService<TempUserRegister> tempUserRegister,
        IOptions<JwtSetting> option,
        IMapper mapper) : DataLayerController(mapper, option)
    {
        private readonly IMongoService<TempUserRegister> _tempUserRegister = tempUserRegister;

        [HttpGet("get-temp-user")]
        public async Task<IActionResult> Get(string id)
        {
            TempUserRegister? tempUser  = await _tempUserRegister.GetById(id);
            if (tempUser != null)
            {
                TempUserDTO tempUserDTO = _mapper.Map<TempUserDTO>(tempUser);
                return Ok(tempUserDTO);
            }
            return BadRequest("Not Found");
        }
        [HttpPut("update-temp-user")]
        public async Task<IActionResult> Update(TempUserRegister tempUser)
        {
            
            if (await _tempUserRegister.Update(tempUser))
            {
                return Ok(tempUser);
            }
            return BadRequest("Not Found");
        }
        [HttpDelete("delete-temp-user")]
        public async Task<IActionResult> Delete(TempUserRegister tempUser)
        {

            if (await _tempUserRegister.Delete(tempUser))
            {
                return Ok("Delete Success");
            }
            return BadRequest("Not Found");
        }
        [HttpPost("create-temp-user")]
        public async Task<IActionResult> Create(TempUserRegister tempUser)
        {

            if (await _tempUserRegister.Create(tempUser))
            {
                return Ok(tempUser);
            }
            return BadRequest("Not Found");
        }
    }
}
