using MedAppointment.DataTransferObjects.DoctorDtos;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface ISpecialtyService
    {
        Task<Result<SpecialtyPagedResultDto>> GetSpecialtiesAsync(ClassifierPaginationQueryDto query);
        Task<Result<SpecialtyDto>> GetSpecialtyByIdAsync(long id);
        Task<Result> CreateSpecialtyAsync(SpecialtyCreateDto specialty);
        Task<Result> UpdateSpecialtyAsync(long id, SpecialtyUpdateDto specialty);
    }
}
