using Microsoft.AspNetCore.Mvc;
using RestaurantAPI2.Models;
using RestaurantAPI2.Services;

namespace RestaurantAPI2.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService accountService)
        {
            _service = accountService;
        }

        [HttpPost("reg")]
        public ActionResult Register([FromBody] RegisterUserDTO dto)
        {

            _service.RegisterUser(dto);
            return Ok();
        }
    }
}
