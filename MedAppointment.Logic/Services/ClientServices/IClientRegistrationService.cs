namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IClientRegistrationService
    {
        Task<Result<long>> RegisterUserAsync(BaseRegisterDto traditionalUserRegister);
    }
}
