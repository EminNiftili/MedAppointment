using MedAppointment.DataTransferObjects.DoctorDtos;

namespace MedAppointment.Api.Controllers.ServiceControllers
{
    [Authorize(Roles = RoleNames.DoctorRole)]
    public class DoctorCalendarController : BaseApiController
    {
        private readonly IDoctorCalendarService _doctorCalendarService;

        public DoctorCalendarController(IDoctorCalendarService doctorCalendarService)
        {
            _doctorCalendarService = doctorCalendarService;
        }

        [AllowAnonymous]
        [HttpGet("week")]
        public async Task<IActionResult> GetWeeklyCalendarAsync([FromQuery] DoctorCalendarWeekQueryDto query)
        {
            var result = await _doctorCalendarService.GetWeeklyCalendarAsync(query);
            return CustomResult(result);
        }

        [HttpPut("day-plan")]
        public async Task<IActionResult> EditDayPlanAsync([FromBody] EditDayPlanDto dto)
        {
            var result = await _doctorCalendarService.EditDayPlanAsync(dto);
            return CustomResult(result);
        }
    }
}
