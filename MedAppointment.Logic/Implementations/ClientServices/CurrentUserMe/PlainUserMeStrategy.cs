namespace MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe
{
    internal class PlainUserMeStrategy : IUserMeResponseStrategy
    {
        private readonly ILogger<PlainUserMeStrategy> _logger;
        private readonly IUnitOfClient _unitOfClient;
        private readonly IPrivateClientInfoService _privateClientInfoService;

        public PlainUserMeStrategy(
            ILogger<PlainUserMeStrategy> logger,
            IUnitOfClient unitOfClient,
            IPrivateClientInfoService privateClientInfoService)
        {
            _logger = logger;
            _unitOfClient = unitOfClient;
            _privateClientInfoService = privateClientInfoService;
        }

        public bool CanHandle(UserType[] userTypes)
        {
            return userTypes.Length == 1 && userTypes[0] == UserType.User;
        }

        public async Task<Result<object>> BuildAsync(long userId)
        {
            _logger.LogTrace("Building plain user me response. UserId: {UserId}", userId);
            var result = Result<object>.Create();

            var user = await _unitOfClient.User.GetByIdAsync(userId);
            _logger.LogDebug("User loaded for me. UserId: {UserId}, Found: {Found}", userId, user != null);

            if (user is null || user.Person is null)
            {
                _logger.LogInformation("User or person not found for me. UserId: {UserId}", userId);
                result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                return result;
            }

            var userTypes = await _privateClientInfoService.GetUserTypesAsync(userId);
            var dto = new UserMeDto
            {
                Id = user.Id,
                Provider = user.Provider,
                Name = user.Person.Name ?? string.Empty,
                Surname = user.Person.Surname ?? string.Empty,
                FatherName = user.Person.FatherName ?? string.Empty,
                Email = user.Person.Email ?? string.Empty,
                PhoneNumber = user.Person.PhoneNumber ?? string.Empty,
                BirthDate = user.Person.BirthDate,
                ImagePath = user.Person.Image?.FilePath,
                UserTypes = userTypes
            };

            result.Success(dto, HttpStatusCode.OK);
            _logger.LogInformation("Plain user me response built. UserId: {UserId}", userId);
            return result;
        }
    }
}
