using System.Net;
using System.Security.Claims;
using MedAppointment.Logics.Patterns.ResultPattern;

namespace MedAppointment.Api.Controllers.UserControllers
{
    public class UsersController : BaseApiController
    {
        private readonly ICurrentUserMeService _currentUserMeService;

        public UsersController(ICurrentUserMeService currentUserMeService)
        {
            _currentUserMeService = currentUserMeService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMeAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                var badResult = Result.Create();
                badResult.AddMessage("ERR00166", "Invalid or missing user identity.", HttpStatusCode.Unauthorized);
                return CustomResult(badResult);
            }

            var result = await _currentUserMeService.GetCurrentUserMeAsync(userId);
            return CustomResult(result);
        }
    }
}
