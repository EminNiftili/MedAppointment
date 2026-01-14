namespace MedAppointment.Logics.Implementations.ClientServices
{
    public class ClientRegistrationService : IClientRegistrationService
    {
        protected readonly IUnitOfClient unitOfClient;
        private readonly ILogger<ClientRegistrationService> _logger;

        public ClientRegistrationService(IUnitOfClient unitOfClient, 
            ILogger<ClientRegistrationService> logger)
        {
            this._logger = logger;
            this.unitOfClient = unitOfClient;
        }

        public async Task RegisterTraditionalUserAsync(TraditionalUserRegisterDto traditionalUserRegister)
        {
            _logger.Log(LogLevel.Trace, "Register traditional user domain service started: {0}", traditionalUserRegister);
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
            _logger.Log(LogLevel.Trace, "Entity created");
            try
            {
                unitOfClient.Person.Add(person);
                await unitOfClient.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Exception when register user: {0}", traditionalUserRegister);
            }
        }
    }
}
