using Microsoft.AspNetCore.Mvc;
using SafeNotes.Application.Models.Auth;
using SafeNotes.Application.Services;

namespace SafeNotes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.Login(request, cancellationToken);
            if (result.Success)
            {
                return Ok(result.GetOk());
            }

            return BadRequest(result.GetError());
        }
    }
}
