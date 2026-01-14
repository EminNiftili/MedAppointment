namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IClientRegistrationService
    {
        Task RegisterTraditionalUserAsync(TraditionalUserRegisterDto traditionalUserRegister);
    }
}
