using Microsoft.AspNetCore.Mvc;
using SafeNotes.Application.Models.Users;
using SafeNotes.Application.Services;

namespace SafeNotes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.RegisterUser(request, cancellationToken);
            if(result.Success)
            {
                return Ok();
            }

            return BadRequest(result.GetError());
        }

        [HttpGet("confirm-email/{emailConfirmationCode}")]
        public async Task<IActionResult> RegisterUser(string emailConfirmationCode, CancellationToken cancellationToken)
        {
            var result = await _userService.ConfirmEmail(emailConfirmationCode, cancellationToken);
            if (result.Success)
            {
                return Ok();
            }

            return BadRequest(result.GetError());
        }
    }
}
