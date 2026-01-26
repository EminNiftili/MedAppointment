namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IDoctorService
    {
        Task<Result> RegisterAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister);
        Task<Result> ConfirmDoctorAsync(long doctorId, bool withAllSpecalties = true);
        Task<Result> ConfirmDoctorSpecialtiesAsync(long doctorId, long specialtyId);
    }
}
