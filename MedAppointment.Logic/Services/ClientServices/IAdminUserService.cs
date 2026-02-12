namespace MedAppointment.Logics.Services.ClientServices
{
    public interface IAdminUserService
    {
        Task<Result> RemoveUserAsync(long userId);
        Task<Result<AdminUserDetailsDto>> GetUserDetailsByIdAsync(long userId);
    }
}
