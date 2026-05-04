namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class ProfessionsController : BaseApiController
    {
        private readonly IProfessionService _professionService;

        public ProfessionsController(IProfessionService professionService)
        {
            _professionService = professionService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfessionsAsync([FromQuery] ClassifierPaginationQueryDto query)
        {
            var result = await _professionService.GetProfessionsAsync(query);
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProfessionByIdAsync(long id)
        {
            var result = await _professionService.GetProfessionByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> CreateProfessionAsync(ProfessionCreateDto profession)
        {
            var result = await _professionService.CreateProfessionAsync(profession);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> UpdateProfessionAsync(long id, ProfessionUpdateDto profession)
        {
            var result = await _professionService.UpdateProfessionAsync(id, profession);
            return CustomResult(result);
        }
    }
}
