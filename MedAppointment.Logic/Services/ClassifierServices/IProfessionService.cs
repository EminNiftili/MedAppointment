using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface IProfessionService
    {
        Task<Result<ProfessionPagedResultDto>> GetProfessionsAsync(ClassifierPaginationQueryDto query);
        Task<Result<ProfessionDto>> GetProfessionByIdAsync(long id);
        Task<Result> CreateProfessionAsync(ProfessionCreateDto profession);
        Task<Result> UpdateProfessionAsync(long id, ProfessionUpdateDto profession);
    }
}
