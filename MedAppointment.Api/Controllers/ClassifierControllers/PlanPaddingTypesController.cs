namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class PlanPaddingTypesController : BaseApiController
    {
        private readonly IPlanPaddingTypeService _planPaddingTypeService;

        public PlanPaddingTypesController(IPlanPaddingTypeService planPaddingTypeService)
        {
            _planPaddingTypeService = planPaddingTypeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlanPaddingTypesAsync([FromQuery] PlanPaddingTypePaginationQueryDto query)
        {
            var result = await _planPaddingTypeService.GetPlanPaddingTypesAsync(query);
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlanPaddingTypeByIdAsync(long id)
        {
            var result = await _planPaddingTypeService.GetPlanPaddingTypeByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> CreatePlanPaddingTypeAsync(PlanPaddingTypeCreateDto planPaddingType)
        {
            var result = await _planPaddingTypeService.CreatePlanPaddingTypeAsync(planPaddingType);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> UpdatePlanPaddingTypeAsync(long id, PlanPaddingTypeUpdateDto planPaddingType)
        {
            var result = await _planPaddingTypeService.UpdatePlanPaddingTypeAsync(id, planPaddingType);
            return CustomResult(result);
        }
    }
}
