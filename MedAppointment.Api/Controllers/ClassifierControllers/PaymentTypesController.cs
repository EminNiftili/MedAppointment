namespace MedAppointment.Api.Controllers.ClassifierControllers
{
    public class PaymentTypesController : BaseApiController
    {
        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypesController(IPaymentTypeService paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentTypesAsync()
        {
            var result = await _paymentTypeService.GetPaymentTypesAsync();
            return CustomResult(result);
        }

        [HttpGet("{id:long}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentTypeByIdAsync(long id)
        {
            var result = await _paymentTypeService.GetPaymentTypeByIdAsync(id);
            return CustomResult(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> CreatePaymentTypeAsync(PaymentTypeCreateDto paymentType)
        {
            var result = await _paymentTypeService.CreatePaymentTypeAsync(paymentType);
            return CustomResult(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = RoleNames.SystemAdminRole)]
        public async Task<IActionResult> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto paymentType)
        {
            var result = await _paymentTypeService.UpdatePaymentTypeAsync(id, paymentType);
            return CustomResult(result);
        }
    }
}
