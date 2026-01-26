namespace MedAppointment.Logics.Implementations.ClientServices
{
    internal class DoctorService : IDoctorService
    {
        protected readonly ILogger<DoctorService> Logger;
        protected readonly IUnitOfClient UnitOfClient;
        protected readonly IClientRegistrationService ClientRegistration;
        protected readonly IValidator<PaginationQueryDto> PaginationQueryValidator;
        protected readonly IMapper Mapper;

        public DoctorService(ILogger<DoctorService> logger, 
            IUnitOfClient unitOfClient,
            IClientRegistrationService clientRegistration,
            IValidator<PaginationQueryDto> paginationQueryValidator,
            IMapper mapper)
        {
            Logger = logger;
            UnitOfClient = unitOfClient;
            ClientRegistration = clientRegistration;
            PaginationQueryValidator = paginationQueryValidator;
            Mapper = mapper;
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

            var doctorEntities = await UnitOfClient.Doctor.GetAllAsync();
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
            var doctorEntity = await UnitOfClient.Doctor.GetByIdAsync(doctorId);
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
    }
}
