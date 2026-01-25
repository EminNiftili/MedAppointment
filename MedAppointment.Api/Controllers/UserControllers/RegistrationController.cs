namespace MedAppointment.Api.Controllers.UserControllers
{
    [AllowAnonymous]
    public class RegistrationController : BaseApiController
    {
        private readonly IClientRegistrationService _registrationService;

        public RegistrationController(IClientRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost]
        public async Task<IActionResult> TraditionalAsync(TraditionalUserRegisterDto traditionalUserRegister)
        {
            var result = await _registrationService.RegisterUserAsync(traditionalUserRegister);
            return CustomResult(result);
        }

        [HttpPost("Google")]
        public IActionResult GoogleAsync()
        {
            return BadRequest();
        }
        [HttpPost("Facebook")]
        public IActionResult FacebookAsync()
        {
            return BadRequest();
        }
        [HttpPost("Apple")]
        public IActionResult AppleAsync()
        {
            return BadRequest();
        }
    }
}
