namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IDoctorService
    {
        Task<Result<PagedResultDto<DoctorDto>>> GetDoctorsAsync(PaginationQueryDto query, bool includeUnconfirmed);
        Task<Result<DoctorDto>> GetDoctorByIdAsync(long doctorId, bool includeUnconfirmed);
        Task<Result> RegisterAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister);
        Task<Result> ConfirmDoctorAsync(long doctorId, bool withAllSpecalties = true);
        Task<Result> ConfirmDoctorSpecialtiesAsync(long doctorId, long specialtyId);
        Task<Result> AddDoctorSpecialtyAsync(long doctorId, AdminDoctorSpecialtyCreateDto specialty);
        Task<Result> RemoveDoctorSpecialtyAsync(long doctorId, long specialtyId);

        /// <summary>
        /// Ensures the doctor exists and is verified (confirmed). Use when an operation requires a verified doctor.
        /// Returns success if doctor exists and IsConfirm; otherwise NotFound or Forbidden with appropriate message.
        /// </summary>
        Task<Result> EnsureDoctorIsVerifiedAsync(long doctorId);
    }
}
