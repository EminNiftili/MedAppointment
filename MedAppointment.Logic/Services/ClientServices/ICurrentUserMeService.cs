namespace MedAppointment.Logics.Services.ClientServices
{
    public interface ICurrentUserMeService
    {
        Task<Result<object>> GetCurrentUserMeAsync(long userId);
    }
}
