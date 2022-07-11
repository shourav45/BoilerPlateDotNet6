using DTO;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService service;
        public UserController()
        {
            service = new UserService();
        }


        [HttpPost]
        [Route("~/create/new/user/account")]
        public async Task<IActionResult> createNewUserAccount(UserAccountDTO user)
        {
            try
            {
                return getResponse(await service.createUserAccount(user));
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [HttpPost]
        [Route("~/reset/user/password")]
        public async Task<IActionResult> resetUserPassword(UserAccountDTO user)
        {
            try
            {
                return getResponse(await service.resetUserPassword(user));
            }
            catch (Exception ex) { return getResponse(ex); }
        }

        [HttpGet]
        [Route("~/delete/user/account/{username}")]
        public async Task<IActionResult> deleteUserAccount(string username)
        {
            try
            {
                return getResponse(await service.deleteUserAccount(username));
            }
            catch (Exception ex) { return getResponse(ex); }
        }
    }
}
