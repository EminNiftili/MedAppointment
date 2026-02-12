namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface IPaymentTypeService
    {
        Task<Result<PaymentTypePagedResultDto>> GetPaymentTypesAsync(ClassifierPaginationQueryDto query);
        Task<Result<PaymentTypeDto>> GetPaymentTypeByIdAsync(long id);
        Task<Result> CreatePaymentTypeAsync(PaymentTypeCreateDto paymentType);
        Task<Result> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto paymentType);
    }
}
