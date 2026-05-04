namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class ClientRegistrationService : IClientRegistrationService
    {
        protected readonly IUnitOfClient unitOfClient;
        protected readonly IUnitOfSecurity UnitOfSecurity;
        protected readonly IValidator<TraditionalUserRegisterDto> TraditionalUserRegisterValidator;
        protected readonly ILogger<ClientRegistrationService> Logger;
        protected readonly IHashService Hasher;
        protected readonly ITokenService TokenService;
        protected readonly IPrivateClientInfoService PrivateClientInfoService;

        public ClientRegistrationService(IUnitOfClient unitOfClient,
            ILogger<ClientRegistrationService> logger,
            IValidator<TraditionalUserRegisterDto> traditionalUserRegister,
            IHashService hasher,
            ITokenService tokenService,
            IPrivateClientInfoService privateClientInfoService,
            IUnitOfSecurity unitOfSecurity)
        {
            this.TraditionalUserRegisterValidator = traditionalUserRegister;
            this.Logger = logger;
            this.unitOfClient = unitOfClient;
            Hasher = hasher;
            TokenService = tokenService;
            PrivateClientInfoService = privateClientInfoService;
            UnitOfSecurity = unitOfSecurity;
        }

        public async Task<Result<long>> RegisterUserAsync(BaseRegisterDto userRegister)
        {
            Result<long> result = Result<long>.Create(-1);
            if (userRegister is null)
            {
                result.AddMessage("ERR00101", "Object is null", HttpStatusCode.BadRequest);
                return result;
            }

            return userRegister switch
            {
                TraditionalUserRegisterDto t => await TraditionalUserRegistrationAsync(t),
                // GoogleRegisterDto g => await RegisterGoogleAsync(g),
                // AppleRegisterDto a => await RegisterAppleAsync(a),
                _ => ReturnInvlaidRegistrationType()
            };

            Result<long> ReturnInvlaidRegistrationType()
            {
                result.AddMessage("ERR00101", "Object type is unknown", HttpStatusCode.Conflict);
                return result;
            }
        }

        public async Task<Result<TokenDto>> RegisterAndLoginAsync(BaseRegisterDto userRegister)
        {
            var result = Result<TokenDto>.Create();

            var registerResult = await RegisterUserAsync(userRegister);
            result.MergeMessages(registerResult);
            result.MergeStatusCode(registerResult);
            if (!result.IsSuccess())
            {
                return result;
            }

            var userId = registerResult.Model;
            return await GenerateTokenForUserAsync(userId, userRegister.DeviceInfo);
        }

        protected async Task<Result<TokenDto>> GenerateTokenForUserAsync(long userId, DeviceDto deviceInfo)
        {
            var result = Result<TokenDto>.Create();

            var userTypes = await PrivateClientInfoService.GetUserTypesAsync(userId);
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, userId.ToString() },
                { ClaimTypes.Role, userTypes.Select(ut => ut.ToString()).ToArray() }
            };

            var accessToken = TokenService.GetToken(out var expiredDate, claims);
            var refreshToken = TokenService.GenerateRefreshToken();

            var newSession = new SessionEntity
            {
                UserId = userId,
                Device = new DeviceEntity
                {
                    Name = deviceInfo.Name,
                    AppType = (byte)deviceInfo.AppType,
                    DeviceType = (byte)deviceInfo.DeviceType,
                    OSName = deviceInfo.OSName,
                    OSVersion = deviceInfo.OSVersion,
                    UUID = deviceInfo.UUID,
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

            result.Success(new TokenDto(accessToken, refreshToken));
            return result;
        }

        private async Task<Result<long>> TraditionalUserRegistrationAsync(TraditionalUserRegisterDto traditionalUserRegister)
        {

            Result<long> result = Result<long>.Create(-1);
            Logger.Log(LogLevel.Trace, "Register traditional user. service started: {0}", traditionalUserRegister);
            Logger.Log(LogLevel.Information, "Model Validation Starting");
            var validatorResult = await TraditionalUserRegisterValidator.ValidateAsync(traditionalUserRegister);
            Logger.Log(LogLevel.Information, "Model Validation Finished");
            if (validatorResult == null)
            {
                Logger.Log(LogLevel.Error, "Validation result is null");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return result;
            }
            else if (!validatorResult.IsValid)
            {
                Logger.Log(LogLevel.Debug, "Validation is failed more details: {0}", validatorResult.Errors);
                result.SetFluentValidationAndBadRequest(validatorResult);
                return result;
            }

            var existedPersonByEmail = await unitOfClient.Person.FindByUsernameAsync(traditionalUserRegister.Email);
            if (existedPersonByEmail != null)
            {
                Logger.Log(LogLevel.Information, "Founded existing email for registration");
                result.AddMessage("ERR00022", "Email already registered!", HttpStatusCode.BadRequest);
                return result;
            }
            var existedPersonByPhoneNumber = await unitOfClient.Person.FindByUsernameAsync(traditionalUserRegister.PhoneNumber);
            if (existedPersonByPhoneNumber != null)
            {
                Logger.Log(LogLevel.Information, "Founded existing phone number for registration");
                result.AddMessage("ERR00023", "Phone Number already registered!", HttpStatusCode.BadRequest);
                return result;
            }

            var hashedPassword = Hasher.HashText(traditionalUserRegister.Password, traditionalUserRegister.Email);
            PersonEntity person = new PersonEntity
            {
                Name = traditionalUserRegister.Name,
                Surname = traditionalUserRegister.Surname,
                FatherName = traditionalUserRegister.FatherName,
                GenderId = traditionalUserRegister.GenderId,
                Email = traditionalUserRegister.Email,
                PhoneNumber = traditionalUserRegister.PhoneNumber,
                BirthDate = traditionalUserRegister.BirthDate,
                User = new UserEntity
                {
                    Provider = 0,
                    TraditionalUser = new TraditionalUserEntity
                    {
                        PasswordHash = hashedPassword
                    }
                }
            };
            Logger.Log(LogLevel.Debug, "Entity created: {0}", person);
            try
            {
                await unitOfClient.Person.AddAsync(person);
                await unitOfClient.SaveChangesAsync();
                result.Model = person.User.Id;
                result.SetStatusCode(HttpStatusCode.NoContent);
                Logger.Log(LogLevel.Trace, "Traditional register finished. Success register for {0}", traditionalUserRegister.Email);
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Error, ex, "Exception when register user: {0}", traditionalUserRegister);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }
            return result;
        }
    }
}
