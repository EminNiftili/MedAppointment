
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
            Logger.LogTrace("Started doctor confirm. DoctorId:{0}, WithAllSpecialties:{1}",
                doctorId, withAllSpecialties);

            var result = Result.Create();

            var doctorEntity = await GetDoctorOrFailAsync(doctorId, result);
            if (doctorEntity is null) return result;

            if (!doctorEntity.IsConfirm)
            {
                doctorEntity.IsConfirm = true;
                Logger.LogDebug("Doctor tagged as confirmed. DoctorId:{0}", doctorId);
            }

            if (withAllSpecialties && doctorEntity.Specialties is not null && doctorEntity.Specialties.Count > 0)
            {
                foreach (var s in doctorEntity.Specialties)
                {
                    if (!s.IsConfirm)
                    {
                        s.IsConfirm = true;
                        Logger.LogDebug(
                            "Doctor specialty tagged as confirmed. DoctorId:{0}, SpecialtyId:{1}",
                            doctorId, s.SpecialtyId);
                    }
                }
            }

            await SaveDoctorAsync(doctorEntity);

            Logger.LogDebug("Confirm tags applied. DoctorId:{0}", doctorId);
            result.Success(HttpStatusCode.NoContent);
            return result;
        }

        public async Task<Result> ConfirmDoctorSpecialtiesAsync(long doctorId, long specialtyId)
        {
            Logger.LogTrace("Started doctor specialty confirm. DoctorId:{0}, SpecialtyId:{1}",
                doctorId, specialtyId);

            var result = Result.Create();

            var doctorEntity = await GetDoctorOrFailAsync(doctorId, result);
            if (doctorEntity is null) return result;

            if (!doctorEntity.IsConfirm)
            {
                Logger.LogDebug("Doctor is not confirmed yet. DoctorId:{0}", doctorId);
                result.AddMessage("ERR00058", "Doctor is not confirmed yet. Doctor cannot confirm specialty before doctor confirm", HttpStatusCode.Conflict);
                return result;
            }

            var specialtyEntity = doctorEntity.Specialties?
                .FirstOrDefault(x => x.SpecialtyId == specialtyId);

            if (specialtyEntity is null)
            {
                Logger.LogDebug("Doctor specialty cannot found. DoctorId:{0}, SpecialtyId:{1}",
                    doctorId, specialtyId);

                result.AddMessage("ERR00057", "Doctor specialty cannot found", HttpStatusCode.NotFound);
                return result;
            }

            if (!specialtyEntity.IsConfirm)
            {
                specialtyEntity.IsConfirm = true;
                Logger.LogDebug("Doctor specialty tagged as confirmed. DoctorId:{0}, SpecialtyId:{1}",
                    doctorId, specialtyId);
            }

            await SaveDoctorAsync(doctorEntity);

            Logger.LogDebug("Specialty confirm applied. DoctorId:{0}, SpecialtyId:{1}",
                doctorId, specialtyId);

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


        private async Task<DoctorEntity?> GetDoctorOrFailAsync(long doctorId, Result result)
        {
            var doctorEntity = await UnitOfClient.Doctor.GetByIdAsync(doctorId);
            Logger.LogDebug("Doctor fetch completed. DoctorId:{0}", doctorId);

            if (doctorEntity is null)
            {
                Logger.LogDebug("Doctor cannot found. DoctorId:{0}", doctorId);
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return null;
            }

            return doctorEntity;
        }

        private async Task SaveDoctorAsync(DoctorEntity doctorEntity)
        {
            UnitOfClient.Doctor.Update(doctorEntity);
            await UnitOfClient.SaveChangesAsync();
        }
    }
}
