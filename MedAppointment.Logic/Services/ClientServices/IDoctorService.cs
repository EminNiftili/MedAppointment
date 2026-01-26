namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IDoctorService
    {
        Task<Result<PagedResultDto<DoctorDto>>> GetDoctorsAsync(PaginationQueryDto query, bool includeUnconfirmed);
        Task<Result<DoctorDto>> GetDoctorByIdAsync(long doctorId, bool includeUnconfirmed);
        Task<Result> RegisterAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister);
        Task<Result> ConfirmDoctorAsync(long doctorId, bool withAllSpecalties = true);
        Task<Result> ConfirmDoctorSpecialtiesAsync(long doctorId, long specialtyId);
    }
}
