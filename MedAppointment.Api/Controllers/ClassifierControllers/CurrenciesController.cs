namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class CurrenciesController : BaseApiController
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrenciesAsync()
        {
            var result = await _currencyService.GetCurrenciesAsync();
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrencyByIdAsync(long id)
        {
            var result = await _currencyService.GetCurrencyByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> CreateCurrencyAsync(CurrencyCreateDto currency)
        {
            var result = await _currencyService.CreateCurrencyAsync(currency);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> UpdateCurrencyAsync(long id, CurrencyUpdateDto currency)
        {
            var result = await _currencyService.UpdateCurrencyAsync(id, currency);
            return CustomResult(result);
        }
    }
}
