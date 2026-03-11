using MedAppointment.DataTransferObjects.CredentialDtos;
using MedAppointment.Logics.Services.SecurityServices;

namespace MedAppointment.Api.Controllers.UserControllers
{
    [AllowAnonymous]
    public class LoginController : BaseApiController
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> TraditionalLoginAsync(TraditionalUserLoginDto traditionalUserLogin)
        {
            var result = await _loginService.TraditionalLoginAsync(traditionalUserLogin);
            
            return SuccessAuthResult(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequestDto? refreshTokenRequest)
        {
            var request = refreshTokenRequest ?? new RefreshTokenRequestDto();
            if (string.IsNullOrWhiteSpace(request.RefreshToken)
                && HttpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshTokenFromCookie))
            {
                request.RefreshToken = refreshTokenFromCookie;
            }

            var result = await _loginService.RefreshTokenAsync(request);

            return SuccessAuthResult(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var accessToken = authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
                ? authHeader["Bearer ".Length..]
                : string.Empty;
            var result = await _loginService.LogoutAsync(accessToken);
            if (result.IsSuccess())
            {
                HttpContext.Response.Cookies.Delete("RefreshToken");
                return NoContent();
            }
            return CustomResult(result);
        }
    }
}
