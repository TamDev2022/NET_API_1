using Microsoft.AspNetCore.Mvc;
using NET_API_1.Interfaces.IServices;
using NET_API_1.Models.Entity;
using System.Security.Claims;

namespace NET_API_1.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [Route("AccessToken")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetToken(User user)
        {
            var accessToken = _tokenService.GenerateAccessToken(user);
            return new JsonResult(new { success = true, Token = accessToken, });
        }

        [Route("RefreshToken")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(string RefreshToken)
        {
            var token = await _tokenService.RefreshToken(RefreshToken);
            return new JsonResult(new { success = true, data = token.TokenHash });
        }
    }
}
