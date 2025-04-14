using CountryAPP.Core.Contract.Persistence;
using CountryAPP.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CountryAPP.API.Endpoint.Controllers.V1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet("GetAllUser")]
    public async Task<IActionResult> GetAllUser()
    {
        try
        {
            var allUser = await this._accountRepository.GetAllUserInfoAsync();
            return Ok(allUser);
        }
        catch (Exception)
        {
            return BadRequest("Invalid Request");
        }
    }
    [HttpGet("GetById")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        try
        {
            var user = await _accountRepository.GetByIdAsync(id);
            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest("Invalid Request");
        }
    }
    [HttpPost("CreateOrEdit"), AllowAnonymous]
    public async Task<IActionResult> CreateOrEditUser([FromBody] UserModel model)
    {
        try
        {
            var userDto = await this._accountRepository.CreateOrEditUser(model);
            return Ok(userDto);
        }
        catch (Exception)
        {
            return BadRequest("Invalid Request");
        }
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var idRet = await this._accountRepository.DeleteUser(id);
            return Ok(idRet);
        }
        catch (Exception)
        {
            return BadRequest("Invalid Request");
        }
    }

    //[HttpPost("ChangePasswordSelf")]
    //public async Task<IActionResult> ChangePasswordSelf(ChangePasswordRequestSelf changePasswordRequestSelf)
    //{
    //    try
    //    {
    //        var idRet = await this._accountRepository.ChangePasswordSelf(changePasswordRequestSelf);
    //        return Ok(idRet);
    //    }
    //    catch (Exception)
    //    {
    //        return BadRequest("Invalid Request");
    //    }
    //}

}
