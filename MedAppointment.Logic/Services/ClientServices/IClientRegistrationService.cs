namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IClientRegistrationService
    {
        Task<Result> RegisterTraditionalUserAsync(TraditionalUserRegisterDto traditionalUserRegister);
    }
}
