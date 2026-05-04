namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IClientRegistrationService
    {
        Task<Result<long>> RegisterUserAsync(BaseRegisterDto userRegister);
        Task<Result<TokenDto>> RegisterAndLoginAsync(BaseRegisterDto userRegister);
    }
}
