namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class SpecialtiesController : BaseApiController
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialtiesAsync()
        {
            var result = await _specialtyService.GetSpecialtiesAsync();
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialtyByIdAsync(long id)
        {
            var result = await _specialtyService.GetSpecialtyByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> CreateSpecialtyAsync(SpecialtyCreateDto specialty)
        {
            var result = await _specialtyService.CreateSpecialtyAsync(specialty);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty)
        {
            var result = await _specialtyService.UpdateSpecialtyAsync(id, specialty);
            return CustomResult(result);
        }
    }
}
