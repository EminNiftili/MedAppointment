namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class ClassifiersController : BaseApiController
    {
        private readonly IClassifierService _classifierService;

        public ClassifiersController(IClassifierService classifierService)
        {
            _classifierService = classifierService;
        }

        [HttpGet("currencies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrenciesAsync()
        {
            var result = await _classifierService.GetCurrenciesAsync();
            return CustomResult(result);
        }

        [HttpGet("currencies/{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrencyByIdAsync(long id)
        {
            var result = await _classifierService.GetCurrencyByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost("currencies")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> CreateCurrencyAsync(CurrencyCreateDto currency)
        {
            var result = await _classifierService.CreateCurrencyAsync(currency);
            return CustomResult(result);
        }

        [HttpPut("currencies/{id:long}")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> UpdateCurrencyAsync(long id, CurrencyUpdateDto currency)
        {
            var result = await _classifierService.UpdateCurrencyAsync(id, currency);
            return CustomResult(result);
        }

        [HttpGet("payment-types")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentTypesAsync()
        {
            var result = await _classifierService.GetPaymentTypesAsync();
            return CustomResult(result);
        }

        [HttpGet("payment-types/{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentTypeByIdAsync(long id)
        {
            var result = await _classifierService.GetPaymentTypeByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost("payment-types")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> CreatePaymentTypeAsync(PaymentTypeCreateDto paymentType)
        {
            var result = await _classifierService.CreatePaymentTypeAsync(paymentType);
            return CustomResult(result);
        }

        [HttpPut("payment-types/{id:long}")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto paymentType)
        {
            var result = await _classifierService.UpdatePaymentTypeAsync(id, paymentType);
            return CustomResult(result);
        }

        [HttpGet("periods")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPeriodsAsync()
        {
            var result = await _classifierService.GetPeriodsAsync();
            return CustomResult(result);
        }

        [HttpGet("periods/{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPeriodByIdAsync(long id)
        {
            var result = await _classifierService.GetPeriodByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost("periods")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> CreatePeriodAsync(PeriodCreateDto period)
        {
            var result = await _classifierService.CreatePeriodAsync(period);
            return CustomResult(result);
        }

        [HttpPut("periods/{id:long}")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> UpdatePeriodAsync(long id, PeriodUpdateDto period)
        {
            var result = await _classifierService.UpdatePeriodAsync(id, period);
            return CustomResult(result);
        }

        [HttpGet("specialties")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialtiesAsync()
        {
            var result = await _classifierService.GetSpecialtiesAsync();
            return CustomResult(result);
        }

        [HttpGet("specialties/{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialtyByIdAsync(long id)
        {
            var result = await _classifierService.GetSpecialtyByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost("specialties")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> CreateSpecialtyAsync(SpecialtyCreateDto specialty)
        {
            var result = await _classifierService.CreateSpecialtyAsync(specialty);
            return CustomResult(result);
        }

        [HttpPut("specialties/{id:long}")]
        [Authorize(Roles = "SystemAdmin,OrganizationAdmin")]
        public async Task<IActionResult> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty)
        {
            var result = await _classifierService.UpdateSpecialtyAsync(id, specialty);
            return CustomResult(result);
        }
    }
}
