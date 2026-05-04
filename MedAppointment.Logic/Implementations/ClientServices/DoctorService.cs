using MedAppointment.Logics.Extensions;
using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class DoctorService : IDoctorService
    {
        protected readonly ILocalizerService LocalizerService;
        protected readonly ILogger<DoctorService> Logger;
        protected readonly IUnitOfDoctor UnitOfDoctor;
        protected readonly IUnitOfClient UnitOfClient;
        protected readonly IUnitOfClassifier UnitOfClassifier;
        protected readonly IUnitOfSecurity UnitOfSecurity;
        protected readonly IClientRegistrationService ClientRegistration;
        protected readonly ITokenService TokenService;
        protected readonly IPrivateClientInfoService PrivateClientInfoService;
        protected readonly IValidator<PaginationQueryDto> PaginationQueryValidator;
        protected readonly IValidator<AdminDoctorSpecialtyCreateDto> AdminDoctorSpecialtyCreateValidator;
        protected readonly IMapper Mapper;
        protected readonly ITokenService TokenService;
        protected readonly IPrivateClientInfoService PrivateClientInfoService;
        protected readonly IUnitOfSecurity UnitOfSecurity;

        public DoctorService(
            ILocalizerService localizerService,
            ILogger<DoctorService> logger,
            IUnitOfDoctor unitOfDoctor,
            IUnitOfClient unitOfClient,
            IUnitOfClassifier unitOfClassifier,
            IUnitOfSecurity unitOfSecurity,
            IClientRegistrationService clientRegistration,
            ITokenService tokenService,
            IPrivateClientInfoService privateClientInfoService,
            IValidator<PaginationQueryDto> paginationQueryValidator,
            IValidator<AdminDoctorSpecialtyCreateDto> adminDoctorSpecialtyCreateValidator,
            IMapper mapper,
            ITokenService tokenService,
            IPrivateClientInfoService privateClientInfoService,
            IUnitOfSecurity unitOfSecurity)
        {
            LocalizerService = localizerService;
            Logger = logger;
            UnitOfClient = unitOfClient;
            UnitOfDoctor = unitOfDoctor;
            UnitOfClassifier = unitOfClassifier;
            UnitOfSecurity = unitOfSecurity;
            ClientRegistration = clientRegistration;
            TokenService = tokenService;
            PrivateClientInfoService = privateClientInfoService;
            PaginationQueryValidator = paginationQueryValidator;
            AdminDoctorSpecialtyCreateValidator = adminDoctorSpecialtyCreateValidator;
            Mapper = mapper;
            TokenService = tokenService;
            PrivateClientInfoService = privateClientInfoService;
            UnitOfSecurity = unitOfSecurity;
        }

        public async Task<Result> AddDoctorSpecialtyAsync(long doctorId, AdminDoctorSpecialtyCreateDto specialty)
        {
            Logger.LogTrace("Started doctor specialty add. DoctorId:{0}, SpecialtyId:{1}, IsConfirmed:{2}",
                doctorId, specialty.SpecialtyId, specialty.IsConfirmed);

            var result = Result.Create();

            if (!await ValidateModelAsync(AdminDoctorSpecialtyCreateValidator, specialty, result))
            {
                return result;
            }

            var doctorEntity = await GetDoctorOrFailAsync(doctorId, result);
            if (doctorEntity is null) return result;

            var specialtyEntity = await UnitOfClassifier.Specialty.GetByIdAsync(specialty.SpecialtyId);
            Logger.LogDebug("Specialty fetch completed. SpecialtyId:{0}, Exists:{1}", specialty.SpecialtyId, specialtyEntity is not null);
            if (specialtyEntity is null)
            {
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }

            var exists = doctorEntity.Specialties.Any(x => !x.IsDeleted && x.SpecialtyId == specialty.SpecialtyId);
            Logger.LogDebug("Doctor specialty existence check completed. DoctorId:{0}, SpecialtyId:{1}, Exists:{2}",
                doctorId, specialty.SpecialtyId, exists);
            if (exists)
            {
                result.AddMessage("ERR00051", "Classifier name already exists.", HttpStatusCode.Conflict);
                return result;
            }

            doctorEntity.Specialties.Add(new DoctorSpecialtyEntity
            {
                SpecialtyId = specialty.SpecialtyId,
                IsConfirm = specialty.IsConfirmed,
            });

            await SaveDoctorAsync(doctorEntity);

            Logger.LogInformation("Doctor specialty added. DoctorId:{0}, SpecialtyId:{1}, IsConfirmed:{2}",
                doctorId, specialty.SpecialtyId, specialty.IsConfirmed);
            result.Success(HttpStatusCode.NoContent);
            return result;
        }

        public async Task<Result<PagedResultDto<DoctorDto>>> GetDoctorsAsync(PaginationQueryDto query, bool includeUnconfirmed)
        {
            Logger.LogTrace("Started doctor list retrieval. IncludeUnconfirmed: {IncludeUnconfirmed}", includeUnconfirmed);
            var result = Result<PagedResultDto<DoctorDto>>.Create();

            if (!await ValidateModelAsync(PaginationQueryValidator, query, result))
            {
                Logger.LogDebug("Doctor list query validation failed.");
                return result;
            }
            Logger.LogDebug("Doctor list query validation succeeded.");

            var doctorEntities = await UnitOfDoctor.Doctor.GetAllAsync();
            Logger.LogDebug("Doctor entities fetched. Count: {Count}", doctorEntities.Count());
            var filteredDoctors = includeUnconfirmed
                ? doctorEntities.ToList()
                : doctorEntities.Where(x => x.IsConfirm).ToList();
            Logger.LogDebug("Doctor entities filtered. Count: {Count}", filteredDoctors.Count);

            var totalCount = filteredDoctors.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);
            Logger.LogDebug("Pagination calculated. TotalCount: {TotalCount}, TotalPages: {TotalPages}", totalCount, totalPages);

            var doctors = filteredDoctors
                .OrderBy(x => x.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => Mapper.Map<DoctorDto>(x, options =>
                {
                    options.Items["IncludeUnconfirmed"] = includeUnconfirmed;
                }))
                .ToList();
            Logger.LogDebug("Doctor page mapped. Count: {Count}", doctors.Count);

            result.Success(new PagedResultDto<DoctorDto>
            {
                Items = doctors,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            });

            Logger.LogInformation("Doctor list retrieved. Count: {Count}, PageNumber: {PageNumber}", doctors.Count, query.PageNumber);
            return result;
        }

        public async Task<Result<DoctorDto>> GetDoctorByIdAsync(long doctorId, bool includeUnconfirmed)
        {
            Logger.LogTrace("Started doctor retrieval by id. DoctorId: {DoctorId}, IncludeUnconfirmed: {IncludeUnconfirmed}", doctorId, includeUnconfirmed);
            var result = Result<DoctorDto>.Create();
            var doctorEntity = await UnitOfDoctor.Doctor.GetByIdAsync(doctorId);
            Logger.LogDebug("Doctor entity fetch completed. DoctorId: {DoctorId}", doctorId);

            if (doctorEntity is null || (!includeUnconfirmed && !doctorEntity.IsConfirm))
            {
                Logger.LogInformation("Doctor not found or not confirmed. DoctorId: {DoctorId}", doctorId);
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return result;
            }

            var doctorDto = Mapper.Map<DoctorDto>(doctorEntity, options =>
            {
                options.Items["IncludeUnconfirmed"] = includeUnconfirmed;
            });
            Logger.LogDebug("Doctor entity mapped. DoctorId: {DoctorId}", doctorId);
            result.Success(doctorDto);
            Logger.LogInformation("Doctor retrieved by id. DoctorId: {DoctorId}", doctorId);
            return result;
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

        public async Task<Result> RemoveDoctorSpecialtyAsync(long doctorId, long specialtyId)
        {
            Logger.LogTrace("Started doctor specialty remove. DoctorId:{0}, SpecialtyId:{1}", doctorId, specialtyId);

            var result = Result.Create();

            var doctorEntity = await GetDoctorOrFailAsync(doctorId, result);
            if (doctorEntity is null) return result;

            var specialtyEntity = doctorEntity.Specialties.FirstOrDefault(x => !x.IsDeleted && x.SpecialtyId == specialtyId);
            if (specialtyEntity is null)
            {
                Logger.LogInformation("Doctor specialty cannot found for delete. DoctorId:{0}, SpecialtyId:{1}", doctorId, specialtyId);
                result.AddMessage("ERR00057", "Doctor specialty cannot found", HttpStatusCode.NotFound);
                return result;
            }

            specialtyEntity.IsDeleted = true;
            await SaveDoctorAsync(doctorEntity);

            Logger.LogInformation("Doctor specialty soft deleted. DoctorId:{0}, SpecialtyId:{1}", doctorId, specialtyId);
            result.Success(HttpStatusCode.NoContent);
            return result;
        }

        public async Task<Result> EnsureDoctorIsVerifiedAsync(long doctorId)
        {
            Logger.LogTrace("Started ensure doctor is verified. DoctorId: {DoctorId}", doctorId);
            var result = Result.Create();

            var doctorEntity = await UnitOfDoctor.Doctor.GetByIdAsync(doctorId);
            Logger.LogDebug("Doctor entity fetch completed. DoctorId: {DoctorId}", doctorId);

            if (doctorEntity is null)
            {
                Logger.LogInformation("Doctor not found. DoctorId: {DoctorId}", doctorId);
                result.AddMessage("ERR00056", "Doctor cannot found", HttpStatusCode.NotFound);
                return result;
            }

            if (!doctorEntity.IsConfirm)
            {
                Logger.LogInformation("Doctor is not verified. DoctorId: {DoctorId}", doctorId);
                result.AddMessage("ERR00125", "Only verified doctors can create day plans from schema.", HttpStatusCode.Forbidden);
                return result;
            }

            Logger.LogDebug("Doctor is verified. DoctorId: {DoctorId}", doctorId);
            result.Success();
            return result;
        }

        public async Task<Result<TokenDto>> RegisterAsync(DoctorRegisterDto<TraditionalUserRegisterDto> doctorRegister)
        {
<<<<<<< HEAD
            Logger.LogTrace("Started Doctor registration");
            var userRegisterResult = await ClientRegistration.RegisterUserAsync(doctorRegister.User);
            Logger.LogInformation("Doctor user registration completed. IsSuccess {0}", userRegisterResult.IsSuccess());
            if (!userRegisterResult.IsSuccess())
=======
            var result = Result<TokenDto>.Create();
            Logger.LogTrace("Started Doctor registration");
            var userRegisterResult = await ClientRegistration.RegisterUserAsync(doctorRegister.User);
            Logger.LogInformation("Doctor user registration completed. IsSuccess {0}", userRegisterResult.IsSuccess());
            result.MergeMessages(userRegisterResult);
            result.MergeStatusCode(userRegisterResult);
            if (!result.IsSuccess())
>>>>>>> e038d6701358b0b4c616fb46616b20bb6e12397a
            {
                Logger.LogDebug("User registration is failed");
                var failUser = Result<TokenDto>.Create();
                failUser.MergeMessages(userRegisterResult);
                failUser.MergeStatusCode(userRegisterResult);
                return failUser;
            }

            Logger.LogTrace("Fetching registering user. User Id: {0}", userRegisterResult.Model);
            var userEntity = await UnitOfClient.User.FindFirstAsync(x => x.Id == userRegisterResult.Model);
            if (userEntity == null)
            {
                Logger.LogError("Doctor user registered but cannot found user entity.");
                var failMissing = Result<TokenDto>.Create();
                failMissing.AddMessage("ERR00024", "User cannot found", HttpStatusCode.Conflict);
                return failMissing;
            }
            Logger.LogInformation("Registered user found");

            var titleResult = await LocalizerService.AddResourceAsync($"doctor_{doctorRegister.User.Name.ToASCIIFromUnicode()}_title", doctorRegister.Title);
            var descriptionResult = await LocalizerService.AddResourceAsync($"doctor_{doctorRegister.User.Name.ToASCIIFromUnicode()}_desc", doctorRegister.Description);

            if (!titleResult.IsSuccess() || !descriptionResult.IsSuccess())
            {
<<<<<<< HEAD
                var failLoc = Result<TokenDto>.Create();
                failLoc.MergeMessages(titleResult);
                failLoc.MergeStatusCode(titleResult);
                failLoc.MergeMessages(descriptionResult);
                failLoc.MergeStatusCode(descriptionResult);
                return failLoc;
=======
                result.MergeMessages(titleResult);
                result.MergeStatusCode(titleResult);
                result.MergeMessages(descriptionResult);
                result.MergeStatusCode(descriptionResult);
                return result;
>>>>>>> e038d6701358b0b4c616fb46616b20bb6e12397a
            }

            userEntity.Doctor = new DoctorEntity
            {
                TitleTextId = titleResult.Model,
                DescriptionTextId = descriptionResult.Model,
                ProfessionId = doctorRegister.ProfessionId,
                PresentationVideoUrl = doctorRegister.PresentationVideoUrl,
                IsConfirm = false,
                Specialties = doctorRegister.Specialties.Select(x => new DoctorSpecialtyEntity
                {
                    SpecialtyId = x,
                    IsConfirm = false
                }).ToList(),
                ServiceGenderTypes = doctorRegister.ServiceGenderIds
                    .Select(genderId => new DoctorServiceGenderTypeEntity
                    {
                        GenderId = genderId
                    }).ToList(),
                ServiceLanguages = doctorRegister.ServiceLanguageIds
                    .Select(languageId => new DoctorServiceLanguageEntity
                    {
                        LanguageId = languageId
                    }).ToList()
            };
            Logger.LogInformation("Doctor entity created.");
            UnitOfClient.User.Update(userEntity);
            await UnitOfDoctor.SaveChangesAsync();
            Logger.LogInformation("Doctor entity added");

<<<<<<< HEAD
            var userTypes = await PrivateClientInfoService.GetUserTypesAsync(userEntity.Id);
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.NameIdentifier, userEntity.Id.ToString() },
                { ClaimTypes.Role, userTypes.Select(userType => userType.ToString()).ToArray() }
            };
            if (userEntity.Doctor?.Id > 0)
            {
                claims["DoctorId"] = userEntity.Doctor.Id.ToString();
            }

            var accessToken = TokenService.GetToken(out var expiredDate, claims);
            var refreshToken = TokenService.GenerateRefreshToken();
            var deviceDto = doctorRegister.DeviceInfo
                ?? doctorRegister.User.DeviceInfo
                ?? new DeviceDto
            {
                Name = "Web registration",
                DeviceType = DeviceType.Windows,
                AppType = ApplicationType.Web,
                OSName = null,
                OSVersion = null,
                UUID = "doctor-registration"
            };
            var newSession = new SessionEntity
            {
                UserId = userEntity.Id,
                Device = new DeviceEntity
                {
                    Name = deviceDto.Name,
                    AppType = (byte)deviceDto.AppType,
                    DeviceType = (byte)deviceDto.DeviceType,
                    OSName = deviceDto.OSName,
                    OSVersion = deviceDto.OSVersion,
                    UUID = deviceDto.UUID
=======
            var tokenResult = await GenerateTokenForDoctorAsync(userRegisterResult.Model, doctorRegister.User.DeviceInfo);
            result.MergeResult(tokenResult);
            if (result.IsSuccess())
            {
                result.Model = tokenResult.Model;
            }
            return result;
        }

        private async Task<Result<TokenDto>> GenerateTokenForDoctorAsync(long userId, DeviceDto deviceInfo)
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
>>>>>>> e038d6701358b0b4c616fb46616b20bb6e12397a
                },
                Tokens = new List<TokenEntity>
                {
                    new TokenEntity
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        IsExpired = false,
<<<<<<< HEAD
                        ExpiredDate = expiredDate
=======
                        ExpiredDate = expiredDate,
>>>>>>> e038d6701358b0b4c616fb46616b20bb6e12397a
                    }
                }
            };
            UnitOfSecurity.Session.Add(newSession);
            await UnitOfSecurity.SaveChangesAsync();
<<<<<<< HEAD

            var ok = Result<TokenDto>.Create();
            ok.Success(new TokenDto(accessToken, refreshToken), HttpStatusCode.OK);
            ok.AddMessage("ERR00055", "Doctor registered successfully", HttpStatusCode.OK);
            Logger.LogTrace("Doctor registration finished with session.");
            return ok;
=======
            Logger.LogInformation("Doctor registration session created for UserId {UserId}", userId);

            result.Success(new TokenDto(accessToken, refreshToken));
            return result;
>>>>>>> e038d6701358b0b4c616fb46616b20bb6e12397a
        }

        private async Task<DoctorEntity?> GetDoctorOrFailAsync(long doctorId, Result result)
        {
            var doctorEntity = await UnitOfDoctor.Doctor.GetByIdAsync(doctorId);
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
            UnitOfDoctor.Doctor.Update(doctorEntity);
            await UnitOfDoctor.SaveChangesAsync();
        }

        private async Task<bool> ValidateModelAsync<TDto, TResult>(IValidator<TDto> validator, TDto model, Result<TResult> result)
        {
            Logger.LogInformation("Model validation started for {Validator}.", typeof(TDto).Name);
            var validationResult = await validator.ValidateAsync(model);
            Logger.LogInformation("Model validation finished for {Validator}.", typeof(TDto).Name);

            if (validationResult == null)
            {
                Logger.LogError("Validation result is null for {Validator}.", typeof(TDto).Name);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return false;
            }

            if (!validationResult.IsValid)
            {
                Logger.LogDebug("Validation failed for {Validator} with errors: {Errors}", typeof(TDto).Name, validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateModelAsync<TDto>(IValidator<TDto> validator, TDto model, Result result)
        {
            Logger.LogInformation("Model validation started for {Validator}.", typeof(TDto).Name);
            var validationResult = await validator.ValidateAsync(model);
            Logger.LogInformation("Model validation finished for {Validator}.", typeof(TDto).Name);

            if (validationResult == null)
            {
                Logger.LogError("Validation result is null for {Validator}.", typeof(TDto).Name);
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return false;
            }

            if (!validationResult.IsValid)
            {
                Logger.LogDebug("Validation failed for {Validator} with errors: {Errors}", typeof(TDto).Name, validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return false;
            }

            return true;
        }
    }
}
