using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_API_1.Interfaces.IServices;
using System.Collections.Generic;

namespace NET_API_1.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("list")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListUserAsync(int PageNumber = 1, int PageSize = 10)
        {
            var data = await _userService.GetListAsync(PageNumber, PageSize).ConfigureAwait(false);
            return new JsonResult(new { success = true, data });
        }
    }
}
