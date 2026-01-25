
namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class DoctorService : IDoctorService
    {
        protected readonly ILogger<DoctorService> Logger;
        protected readonly IUnitOfClient UnitOfClient;
        protected readonly IClientRegistrationService ClientRegistration;

        public DoctorService(ILogger<DoctorService> logger, 
            IUnitOfClient unitOfClient,
            IClientRegistrationService clientRegistration)
        {
            Logger = logger;
            UnitOfClient = unitOfClient;
            ClientRegistration = clientRegistration;
        }

        public async Task<Result> RegisterAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister)
        {
            Result result = Result.Create();
            Logger.LogTrace("Started Doctor registration");
            var userRegisterResult = await ClientRegistration.RegisterUserAsync(doctorRegister.User);
            Logger.LogInformation("Doctor user registration completed. IsSuccess {0}", userRegisterResult.IsSuccess());
            result.MergeResult(userRegisterResult);
            if (!result.IsSuccess())
            {
                Logger.LogDebug("User registration is failed");
                return result;
            }
            Logger.LogTrace("Fetching registering user. User Id: {0}", userRegisterResult.Model);
            var userEntity = await UnitOfClient.User.FindFirstAsync(x => x.Id == userRegisterResult.Model);
            if(userEntity == null)
            {
                Logger.LogError("Doctor user registered but cannot found user entity.");
                result.AddMessage("ERR00024", "User cannot found", HttpStatusCode.Conflict);
                return result;
            }
            Logger.LogInformation("Registered user found");

            userEntity.Doctor = new DoctorEntity
            {
                Title = doctorRegister.Title,
                Description = doctorRegister.Description,
                IsConfirm = false,
                Specialties = doctorRegister.Specialties.Select(x => new DoctorSpecialtyEntity
                {
                    SpecialtyId = x,
                    IsConfirm = false
                }).ToList()
            };
            Logger.LogInformation("Doctor entity created.");
            UnitOfClient.User.Update(userEntity);
            await UnitOfClient.SaveChangesAsync();
            Logger.LogInformation("Doctor entity added");
            result.AddMessage("ERR00055", "Doctor registered successfully", HttpStatusCode.OK);
            return result;
        }
    }
}
