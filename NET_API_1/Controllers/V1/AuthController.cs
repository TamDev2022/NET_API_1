using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Models.DTO;
using NET_API_1.Models.Entity;
using NET_API_1.Models.Request;

namespace NET_API_1.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        public readonly ILogger<AuthController> _logger;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AuthController(ILogger<AuthController> logger, ITokenService tokenService, IUserService userService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _userService = userService;
        }

        [Route("SignIn")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignInAsync(UserSignInModel user)
        {
            if (user == null)
            {
                return BadRequest(404);
            }
            var data = await _userService.SignInAsync(user).ConfigureAwait(false);
            return new JsonResult(new { success = true, data });
        }

        [Route("SignUp")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUpAsync([FromForm] UserSignUpModel model)
        {
            var data = await _userService.SignUpAsync(model).ConfigureAwait(false);
            return new JsonResult(new { success = true, data });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyAccountAsync(string code, UserDTO userDTO)
        {
            var data = await _userService.ConfirmAccountAsync(code, userDTO).ConfigureAwait(false);
            return new JsonResult(new { success = true, message = "Xác nhận thành công" });
        }
    }
}
