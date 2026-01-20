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
    }
}
