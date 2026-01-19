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
    }
}
