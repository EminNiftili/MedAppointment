namespace MedAppointment.Logics.Implementations.SecurityServices
{
    internal class LoginService : ILoginService
    {
        protected readonly ILogger<LoginService> Logger;
        protected readonly IUnitOfClient UnitOfClient;
        protected readonly IUnitOfService UnitOfService;
        protected readonly IUnitOfSecurity UnitOfSecurity;
        protected readonly IHashService HashService;
        protected readonly IValidator<TraditionalUserLoginDto> TraditionalUserLoginValidator;
        protected readonly IValidator<RefreshTokenRequestDto> RefreshTokenRequestValidator;
        protected readonly ITokenService TokenService;
        protected readonly IPrivateClientInfoService PrivateClientInfoService;
        public LoginService(ILogger<LoginService> logger, 
            IUnitOfClient unitOfClient, 
            IUnitOfService unitOfService,
            IHashService hashService,
            IValidator<TraditionalUserLoginDto> traditionalUserLoginValidator,
            IValidator<RefreshTokenRequestDto> refreshTokenRequestValidator,
            ITokenService tokenService,
            IPrivateClientInfoService privateClientInfoService, 
            IUnitOfSecurity unitOfSecurity)
        {
            Logger = logger;
            UnitOfClient = unitOfClient;
            UnitOfService = unitOfService;
            UnitOfSecurity = unitOfSecurity;
            HashService = hashService;
            TraditionalUserLoginValidator = traditionalUserLoginValidator;
            RefreshTokenRequestValidator = refreshTokenRequestValidator;
            TokenService = tokenService;
            PrivateClientInfoService = privateClientInfoService;
        }

        public async Task<Result<TokenDto>> TraditionalLoginAsync(TraditionalUserLoginDto traditionalUserLogin)
        {
            var result = Result<TokenDto>.Create();
            Logger.LogTrace("Started Traditional login");
            
            var traditionalLoginValidatorResult = await TraditionalUserLoginValidator.ValidateAsync(traditionalUserLogin);
            if (traditionalLoginValidatorResult == null)
            {
                Logger.LogError("Validation result returned error");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return result;
            }
            else if (!traditionalLoginValidatorResult.IsValid)
            {
                Logger.LogDebug("Validation result is invalid for traditional login");
                result.SetFluentValidationAndBadRequest(traditionalLoginValidatorResult);
                return result;
            }


            var person = await UnitOfClient.Person.FindByUsernameAsync(traditionalUserLogin.Username!, true);
            if(person == null)
            {
                Logger.LogInformation("User cannot found");
                result.AddMessage("ERR00024", "User does not exist", HttpStatusCode.NotFound);
                return result;
            }
            if(person?.User?.TraditionalUser == null)
            {
                Logger.LogDebug("User found. But used another provider.");
                result.AddMessage("ERR00026", "User found. But used another provider.", HttpStatusCode.Conflict);
                return result;
            }
            var hashedPassword = HashService.HashText(traditionalUserLogin.Password!, person.Email);
            if (!person.User.TraditionalUser.PasswordHash.Equals(hashedPassword))
            {
                Logger.LogInformation("Password is incorrect");
                result.AddMessage("ERR00025", "Password is incorrect", HttpStatusCode.Unauthorized);
                return result;
            }

            var userTypes = await PrivateClientInfoService.GetUserTypesAsync(person.User.Id);
            var claims = new Dictionary<string, object>();
            Logger.LogInformation("Retrieve user types");
            var roleNames = userTypes.Select(userType => userType.ToString()).ToArray();
            claims.Add(ClaimTypes.Role, roleNames);
            Logger.LogTrace("All claims generated");

            var accessToken = TokenService.GetToken(out var expiredDate, claims);
            var refreshToken = TokenService.GenerateRefreshToken();
            Logger.LogInformation("Tokens generated");
            var newSession = new SessionEntity
            {
                UserId = person.User.Id,
                Device = new DeviceEntity
                {
                    Name = traditionalUserLogin.DeviceInfo.Name,
                    AppType = (byte)traditionalUserLogin.DeviceInfo.AppType,
                    DeviceType = (byte)traditionalUserLogin.DeviceInfo.DeviceType,
                    OSName  = traditionalUserLogin.DeviceInfo.OSName,
                    OSVersion = traditionalUserLogin.DeviceInfo.OSVersion,
                    UUID = traditionalUserLogin.DeviceInfo.UUID,
                },
                Tokens = new List<TokenEntity> 
                { 
                    new TokenEntity 
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        IsExpired = false,
                        ExpiredDate = expiredDate,
                    } 
                }
            };
            UnitOfSecurity.Session.Add(newSession);
            await UnitOfSecurity.SaveChangesAsync();
            Logger.LogDebug("New session saved");

            result.Success(new TokenDto(accessToken, refreshToken));
            Logger.LogTrace("Finished Traditional login!");
            return result;
        }

        public async Task<Result<TokenDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest)
        {
            var result = Result<TokenDto>.Create();
            Logger.LogTrace("Started refresh token workflow");

            var refreshTokenValidatorResult = await RefreshTokenRequestValidator.ValidateAsync(refreshTokenRequest);
            if (refreshTokenValidatorResult == null)
            {
                Logger.LogError("Validation result returned error");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return result;
            }
            else if (!refreshTokenValidatorResult.IsValid)
            {
                Logger.LogDebug("Validation result is invalid for refresh token");
                result.SetFluentValidationAndBadRequest(refreshTokenValidatorResult);
                return result;
            }

            var oldTokenEntity = await UnitOfSecurity.Token.FindFirstAsync(token => token.RefreshToken == refreshTokenRequest.RefreshToken, true);
            if (oldTokenEntity?.Session == null || oldTokenEntity.IsExpired)
            {
                Logger.LogInformation("Refresh token not found or session is invalid");
                result.AddMessage("ERR00054", "Refresh token is invalid or expired.", HttpStatusCode.Unauthorized);
                return result;
            }

            var userTypes = await PrivateClientInfoService.GetUserTypesAsync(oldTokenEntity.Session.UserId);
            var claims = new Dictionary<string, object>();
            Logger.LogInformation("Retrieve user types");
            var roleNames = userTypes.Select(userType => userType.ToString()).ToArray();
            claims.Add(ClaimTypes.Role, roleNames);
            Logger.LogTrace("All claims generated");

            var accessToken = TokenService.GetToken(out var expiredDate, claims);
            var refreshToken = TokenService.GenerateRefreshToken();
            Logger.LogInformation("New tokens generated");
            oldTokenEntity.IsExpired = true;
            var newTokenEntity = new TokenEntity
            {
                SessionId = oldTokenEntity.SessionId,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiredDate = expiredDate,
                IsExpired = false
            };
            UnitOfSecurity.Token.Update(oldTokenEntity);
            UnitOfSecurity.Token.Add(newTokenEntity);
            await UnitOfSecurity.SaveChangesAsync();
            Logger.LogDebug("New token linked with session");

            result.Success(new TokenDto(accessToken, refreshToken));
            Logger.LogTrace("Finished refresh token workflow");
            return result;
        }
    }
}
