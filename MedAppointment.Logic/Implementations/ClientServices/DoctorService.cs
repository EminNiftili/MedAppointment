
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

        public async Task<Result> ConfirmDoctorAsync(long doctorId, bool withAllSpecialties = true)
        {
            Result result = Result.Create();
            Logger.LogTrace("Started Doctor confirm.");
            var doctorEntity = await UnitOfClient.Doctor.GetByIdAsync(doctorId);
            Logger.LogDebug("Doctor user fetch request completed.");
            if(doctorEntity == null)
            {
                Logger.LogDebug("Doctor cannot found.");
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return result;
            }
            doctorEntity.IsConfirm = true;
            Logger.LogDebug("Doctor tagged as confirmed.");
            if (withAllSpecialties)
            {
                foreach (var doctorSpecialtyEntity in doctorEntity.Specialties)
                {
                    Logger.LogDebug("Doctor (id:{0}) specialties (id:{1}) tagged as confirm.", doctorEntity.Id, doctorSpecialtyEntity.SpecialtyId);
                    doctorSpecialtyEntity.IsConfirm = true;
                }
            }
            UnitOfClient.Doctor.Update(doctorEntity);
            await UnitOfClient.SaveChangesAsync();
            Logger.LogDebug("All tags applied.");
            result.Success(HttpStatusCode.NoContent);
            return result;



        }

        public async Task<Result> ConfirmDoctorSpecialtiesAsync(long doctorId, long specialtyId)
        {
            Result result = Result.Create();
            Logger.LogTrace("Started Doctor confirm.");
            var doctorEntity = await UnitOfClient.Doctor.GetByIdAsync(doctorId);
            Logger.LogDebug("Doctor user fetch request completed.");
            if (doctorEntity == null)
            {
                Logger.LogDebug("Doctor cannot found.");
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return result;
            }
            if (!doctorEntity.IsConfirm)
            {
                Logger.LogDebug("Doctor cannot found.");
                result.AddMessage("ERR00058", "Doctor cannot confirm specialty befaore Doctor Confirm", HttpStatusCode.Conflict);
                return result;
            }

            var doctorSpecialtyEntity = doctorEntity.Specialties.FirstOrDefault(x => x.SpecialtyId == specialtyId);
            if(doctorSpecialtyEntity == null)
            {
                Logger.LogDebug("Doctor cannot found.");
                result.AddMessage("ERR00057", "Doctor specialty cannot found", HttpStatusCode.NotFound);
                return result;
            }
            doctorSpecialtyEntity.IsConfirm = true;
            Logger.LogDebug("Doctor (id:{0}) specialty (id:{1}) tagged as confirm.", doctorEntity.Id, doctorSpecialtyEntity.SpecialtyId);

            UnitOfClient.Doctor.Update(doctorEntity);
            await UnitOfClient.SaveChangesAsync();
            Logger.LogDebug("Specialty confirm applied.");
            result.Success(HttpStatusCode.NoContent);
            return result;

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
