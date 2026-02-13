namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface ILanguageService
    {
        Task<Result<LanguagePagedResultDto>> GetLanguagesAsync(LanguagePaginationQueryDto query);
        Task<Result<LanguageDto>> GetLanguageByIdAsync(long id);
        Task<Result> CreateLanguageAsync(LanguageCreateDto language);
        Task<Result> UpdateLanguageAsync(long id, LanguageUpdateDto language);
        Task<Result> DeleteLanguageAsync(long id);
    }
}
