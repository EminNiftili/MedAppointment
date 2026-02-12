namespace MedAppointment.Api.Controllers.UserControllers
{
    [Authorize(Roles = RoleNames.SystemAdminRole)]
    public class AdminUsersController : BaseApiController
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUsersController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpDelete("{userId:long}")]
        public async Task<IActionResult> DeleteUserAsync(long userId)
        {
            var result = await _adminUserService.RemoveUserAsync(userId);
            return CustomResult(result);
        }

        [HttpGet("{userId:long}")]
        public async Task<IActionResult> GetUserByIdAsync(long userId)
        {
            var result = await _adminUserService.GetUserDetailsByIdAsync(userId);
            return CustomResult(result);
        }
    }
}
