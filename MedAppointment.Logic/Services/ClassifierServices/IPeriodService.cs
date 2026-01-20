namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface IPeriodService
    {
        Task<Result<IEnumerable<PeriodDto>>> GetPeriodsAsync();
        Task<Result<PeriodDto>> GetPeriodByIdAsync(long id);
        Task<Result> CreatePeriodAsync(PeriodCreateDto period);
        Task<Result> UpdatePeriodAsync(long id, PeriodUpdateDto period);
    }
}
