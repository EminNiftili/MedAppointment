namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IDoctorService
    {
        Task<Result> RegisterAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister);
    }
}
