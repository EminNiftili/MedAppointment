namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class LanguagesController : BaseApiController
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetLanguagesAsync([FromQuery] LanguagePaginationQueryDto query)
        {
            var result = await _languageService.GetLanguagesAsync(query);
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLanguageByIdAsync(long id)
        {
            var result = await _languageService.GetLanguageByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> CreateLanguageAsync(LanguageCreateDto language)
        {
            var result = await _languageService.CreateLanguageAsync(language);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> UpdateLanguageAsync(long id, LanguageUpdateDto language)
        {
            var result = await _languageService.UpdateLanguageAsync(id, language);
            return CustomResult(result);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> DeleteLanguageAsync(long id)
        {
            var result = await _languageService.DeleteLanguageAsync(id);
            return CustomResult(result);
        }
    }
}
