namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface IClassifierService
    {
        Task<Result<IEnumerable<CurrencyDto>>> GetCurrenciesAsync();
        Task<Result<CurrencyDto>> GetCurrencyByIdAsync(long id);
        Task<Result> CreateCurrencyAsync(CurrencyCreateDto currency);
        Task<Result> UpdateCurrencyAsync(long id, CurrencyUpdateDto currency);

        Task<Result<IEnumerable<PaymentTypeDto>>> GetPaymentTypesAsync();
        Task<Result<PaymentTypeDto>> GetPaymentTypeByIdAsync(long id);
        Task<Result> CreatePaymentTypeAsync(PaymentTypeCreateDto paymentType);
        Task<Result> UpdatePaymentTypeAsync(long id, PaymentTypeUpdateDto paymentType);

        Task<Result<IEnumerable<PeriodDto>>> GetPeriodsAsync();
        Task<Result<PeriodDto>> GetPeriodByIdAsync(long id);
        Task<Result> CreatePeriodAsync(PeriodCreateDto period);
        Task<Result> UpdatePeriodAsync(long id, PeriodUpdateDto period);

        Task<Result<IEnumerable<SpecialtyDto>>> GetSpecialtiesAsync();
        Task<Result<SpecialtyDto>> GetSpecialtyByIdAsync(long id);
        Task<Result> CreateSpecialtyAsync(SpecialtyCreateDto specialty);
        Task<Result> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty);
    }
}
