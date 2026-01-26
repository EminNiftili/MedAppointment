namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class PeriodsController : BaseApiController
    {
        private readonly IPeriodService _periodService;

        public PeriodsController(IPeriodService periodService)
        {
            _periodService = periodService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPeriodsAsync()
        {
            var result = await _periodService.GetPeriodsAsync();
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPeriodByIdAsync(long id)
        {
            var result = await _periodService.GetPeriodByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> CreatePeriodAsync(PeriodCreateDto period)
        {
            var result = await _periodService.CreatePeriodAsync(period);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> UpdatePeriodAsync(long id, PeriodUpdateDto period)
        {
            var result = await _periodService.UpdatePeriodAsync(id, period);
            return CustomResult(result);
        }
    }
}
