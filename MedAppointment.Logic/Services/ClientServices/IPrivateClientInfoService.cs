namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IPrivateClientInfoService
    {
        Task<UserType[]> GetUserTypesAsync(long userId);
    }
}
