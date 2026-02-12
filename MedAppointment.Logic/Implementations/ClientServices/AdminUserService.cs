namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class AdminUserService : IAdminUserService
    {
        private readonly ILogger<AdminUserService> _logger;
        private readonly IUnitOfClient _unitOfClient;
        private readonly IPrivateClientInfoService _privateClientInfoService;

        public AdminUserService(
            ILogger<AdminUserService> logger,
            IUnitOfClient unitOfClient,
            IPrivateClientInfoService privateClientInfoService)
        {
            _logger = logger;
            _unitOfClient = unitOfClient;
            _privateClientInfoService = privateClientInfoService;
        }

        public async Task<Result<AdminUserDetailsDto>> GetUserDetailsByIdAsync(long userId)
        {
            _logger.LogTrace("Admin user details request started. UserId:{0}", userId);
            var result = Result<AdminUserDetailsDto>.Create();

            try
            {
                var user = await _unitOfClient.User.GetByIdAsync(userId);
                _logger.LogDebug("User fetch completed. UserId:{0}, Exists:{1}", userId, user is not null);

                if (user is null || user.Person is null)
                {
                    _logger.LogInformation("User cannot be found for details. UserId:{0}", userId);
                    result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                    return result;
                }

                var userTypes = await _privateClientInfoService.GetUserTypesAsync(userId);
                _logger.LogDebug("User types fetched. UserId:{0}, TypesCount:{1}", userId, userTypes.Length);

                result.Success(new AdminUserDetailsDto
                {
                    Id = user.Id,
                    Provider = user.Provider,
                    Name = user.Person.Name,
                    Surname = user.Person.Surname,
                    FatherName = user.Person.FatherName,
                    Email = user.Person.Email,
                    PhoneNumber = user.Person.PhoneNumber,
                    BirthDate = user.Person.BirthDate,
                    ImagePath = user.Person.Image?.Path,
                    UserTypes = userTypes,
                });

                _logger.LogInformation("Admin user details prepared successfully. UserId:{0}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception while getting admin user details. UserId:{0}", userId);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
                return result;
            }
        }

        public async Task<Result> RemoveUserAsync(long userId)
        {
            _logger.LogTrace("Admin user remove request started. UserId:{0}", userId);
            var result = Result.Create();

            try
            {
                var user = await _unitOfClient.User.GetByIdAsync(userId, false);
                _logger.LogDebug("User fetch for delete completed. UserId:{0}, Exists:{1}", userId, user is not null);

                if (user is null)
                {
                    _logger.LogInformation("User cannot be found for delete. UserId:{0}", userId);
                    result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                    return result;
                }

                await _unitOfClient.User.RemoveAsync(userId);
                await _unitOfClient.SaveChangesAsync();

                _logger.LogInformation("User soft deleted successfully. UserId:{0}", userId);
                result.Success(HttpStatusCode.NoContent);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception while deleting user. UserId:{0}", userId);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
                return result;
            }
        }
    }
}
