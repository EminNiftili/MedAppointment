using MedAppointment.DataTransferObjects.PaginationDtos.UserPagination;

namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IAdminUserService
    {
        Task<Result<UserPagedResultDto>> GetUsersAsync(UserPaginationQueryDto query);
        Task<Result> RemoveUserAsync(long userId);
        Task<Result<AdminUserDetailsDto>> GetUserDetailsByIdAsync(long userId);
    }
}
