namespace MedAppointment.Api.Controllers.UserControllers
{
    public class DoctorController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterTraditionalDoctorAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister)
        {
            var result = await _doctorService.RegisterAsync(doctorRegister);
            return CustomResult(result);
        }

        [Authorize(Roles = RoleNames.SystemAdminRole)]
        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmDoctorAsync()
        {
            return Ok();
        }

    }
}
