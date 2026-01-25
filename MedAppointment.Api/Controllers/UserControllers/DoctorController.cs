using System.Threading.Tasks;

namespace MedAppointment.Api.Controllers.UserControllers
{
    [AllowAnonymous]
    public class DoctorController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterTraditionalDoctor(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister)
        {
            var result = await _doctorService.RegisterAsync(doctorRegister);
            return CustomResult(result);
        }
    }
}
