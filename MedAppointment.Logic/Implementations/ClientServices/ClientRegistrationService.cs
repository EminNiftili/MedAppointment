namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class ClientRegistrationService : IClientRegistrationService
    {
        protected readonly IUnitOfClient unitOfClient;
        protected readonly IValidator<TraditionalUserRegisterDto> TraditionalUserRegisterValidator;
        private readonly ILogger<ClientRegistrationService> _logger;

        public ClientRegistrationService(IUnitOfClient unitOfClient,
            ILogger<ClientRegistrationService> logger,
            IValidator<TraditionalUserRegisterDto> traditionalUserRegister)
        {
            this.TraditionalUserRegisterValidator = traditionalUserRegister;
            this._logger = logger;
            this.unitOfClient = unitOfClient;
        }

        public async Task<Result> RegisterTraditionalUserAsync(TraditionalUserRegisterDto traditionalUserRegister)
        {
            Result result = Result.Create();
            _logger.Log(LogLevel.Trace, "Register traditional user. service started: {0}", traditionalUserRegister);
            _logger.Log(LogLevel.Information, "Model Validation Starting");
            var validatorResult = await TraditionalUserRegisterValidator.ValidateAsync(traditionalUserRegister);
            _logger.Log(LogLevel.Information, "Model Validation Finished");
            if (validatorResult == null)
            {
                _logger.Log(LogLevel.Error, "Validation result is null");
                result.AddMessage("ERR00100", "Unexpected error contact with admin");
                return result;
            }
            else if (!validatorResult.IsValid)
            {
                _logger.Log(LogLevel.Debug, "Validation is failed more details: {0}", validatorResult.Errors);
                result.CheckFluentValidation(validatorResult);
                return result;
            }

            PersonEntity person = new PersonEntity
            {
                Name = traditionalUserRegister.Name,
                Surname = traditionalUserRegister.Surname,
                FatherName = traditionalUserRegister.FatherName,
                Email = traditionalUserRegister.Email,
                PhoneNumber = traditionalUserRegister.PhoneNumber,
                BirthDate = traditionalUserRegister.BirthDate,
                User = new UserEntity
                {
                    Provider = 0,
                    TraditionalUser = new TraditionalUserEntity
                    {
                        PasswordHash = traditionalUserRegister.Password
                    }
                }
            };
            _logger.Log(LogLevel.Debug, "Entity created: {0}", person);
            try
            {
                await unitOfClient.Person.AddAsync(person);
                await unitOfClient.SaveChangesAsync();

                result.SetStatusCode(HttpStatusCode.NoContent);
                _logger.Log(LogLevel.Trace, "Traditional register finished. Success register for {0}", traditionalUserRegister.Email);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception when register user: {0}", traditionalUserRegister);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest, ex);
            }
            return result;
        }
    }
}
