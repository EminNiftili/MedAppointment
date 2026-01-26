using MedAppointment.DataTransferObjects.DoctorDtos;

namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface ISpecialtyService
    {
        Task<Result<IEnumerable<SpecialtyDto>>> GetSpecialtiesAsync();
        Task<Result<SpecialtyDto>> GetSpecialtyByIdAsync(long id);
        Task<Result> CreateSpecialtyAsync(SpecialtyCreateDto specialty);
        Task<Result> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty);
    }
}
