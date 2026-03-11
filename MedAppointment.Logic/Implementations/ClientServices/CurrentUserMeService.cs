namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class CurrentUserMeService : ICurrentUserMeService
    {
        private readonly ILogger<CurrentUserMeService> _logger;
        private readonly IPrivateClientInfoService _privateClientInfoService;
        private readonly IEnumerable<IUserMeResponseStrategy> _strategies;

        public CurrentUserMeService(
            ILogger<CurrentUserMeService> logger,
            IPrivateClientInfoService privateClientInfoService,
            IEnumerable<IUserMeResponseStrategy> strategies)
        {
            _logger = logger;
            _privateClientInfoService = privateClientInfoService;
            _strategies = strategies;
        }

        public async Task<Result<object>> GetCurrentUserMeAsync(long userId)
        {
            _logger.LogTrace("Get current user me started. UserId: {UserId}", userId);
            var result = Result<object>.Create();

            try
            {
                var userTypes = await _privateClientInfoService.GetUserTypesAsync(userId);
                _logger.LogDebug("User types resolved. UserId: {UserId}, Types: {Types}", userId, string.Join(",", userTypes.Select(t => t.ToString())));

                var strategy = _strategies.FirstOrDefault(s => s.CanHandle(userTypes));
                if (strategy is null)
                {
                    _logger.LogInformation("No strategy found for user types. UserId: {UserId}, Types: {Types}", userId, string.Join(",", userTypes.Select(t => t.ToString())));
                    result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
                    return result;
                }

                var strategyResult = await strategy.BuildAsync(userId);
                if (!strategyResult.IsSuccess())
                {
                    result.MergeMessages(strategyResult);
                    result.MergeStatusCode(strategyResult);
                    _logger.LogDebug("Strategy returned error. UserId: {UserId}", userId);
                    return result;
                }
                result.MergeResult(strategyResult);
                _logger.LogInformation("Current user me retrieved. UserId: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception while getting current user me. UserId: {UserId}", userId);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.InternalServerError, ex);
                return result;
            }
        }
    }
}
