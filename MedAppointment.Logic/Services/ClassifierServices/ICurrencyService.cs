namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface ICurrencyService
    {
        Task<Result<CurrencyPagedResultDto>> GetCurrenciesAsync(CurrencyPaginationQueryDto query);
        Task<Result<CurrencyDto>> GetCurrencyByIdAsync(long id);
        Task<Result> CreateCurrencyAsync(CurrencyCreateDto currency);
        Task<Result> UpdateCurrencyAsync(long id, CurrencyUpdateDto currency);
    }
}
