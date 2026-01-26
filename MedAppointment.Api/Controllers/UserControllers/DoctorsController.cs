using MedAppointment.DataTransferObjects.PaginationDtos;

namespace MedAppointment.Api.Controllers.UserControllers
{
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationQueryDto query)
        {
            var result = await _doctorService.GetDoctorsAsync(query, User.IsInRole(RoleNames.SystemAdminRole));
            return CustomResult(result);
        }

        [AllowAnonymous]
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var result = await _doctorService.GetDoctorByIdAsync(id, User.IsInRole(RoleNames.SystemAdminRole));
            return CustomResult(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterTraditionalDoctorAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister)
        {
            var result = await _doctorService.RegisterAsync(doctorRegister);
            return CustomResult(result);
        }

        [Authorize(Roles = RoleNames.SystemAdminRole)]
        [HttpPut("confirm/{doctorId:long}")]
        public async Task<IActionResult> ConfirmDoctorAsync(long doctorId)
        {
            var result = await _doctorService.ConfirmDoctorAsync(doctorId, true);
            return CustomResult(result);
        }


        [Authorize(Roles = RoleNames.SystemAdminRole)]
        [HttpPut("confirmSpecialty/{doctorId:long}/{specialtyId:long}")]
        public async Task<IActionResult> ConfirmDoctorAsync(long doctorId, long specialtyId)
        {
            var result = await _doctorService.ConfirmDoctorSpecialtiesAsync(doctorId, specialtyId);
            return CustomResult(result);
        }

    }
}
