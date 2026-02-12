using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;

namespace MedAppointment.Logics.Services.ClassifierServices
{
    public interface IPlanPaddingTypeService
    {
        Task<Result<PlanPaddingTypePagedResultDto>> GetPlanPaddingTypesAsync(PlanPaddingTypePaginationQueryDto query);
        Task<Result<PlanPaddingTypeDto>> GetPlanPaddingTypeByIdAsync(long id);
        Task<Result> CreatePlanPaddingTypeAsync(PlanPaddingTypeCreateDto planPaddingType);
        Task<Result> UpdatePlanPaddingTypeAsync(long id, PlanPaddingTypeUpdateDto planPaddingType);
    }
}
