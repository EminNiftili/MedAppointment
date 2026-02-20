namespace MedAppointment.Logics.Implementations.PlanManagerServices
{
    internal class DoctorPlanManagerService : IDoctorPlanManagerService
    {
        private readonly ILogger<DoctorPlanManagerService> _logger;
        private readonly IDoctorService _doctorService;
        private readonly ITimeSlotService _timeSlotService;
        private readonly IUnitOfService _unitOfService;
        private readonly IUnitOfClassifier _unitOfClassifier;
        private readonly IUnitOfDoctor _unitOfDoctor;
        private readonly IValidator<CreateDayPlansFromSchemaDto> _createDayPlansValidator;
        private readonly IValidator<DoctorSchemaCreateDto> _doctorSchemaCreateValidator;

        public DoctorPlanManagerService(
            ILogger<DoctorPlanManagerService> logger,
            IDoctorService doctorService,
            ITimeSlotService timeSlotService,
            IUnitOfService unitOfService,
            IUnitOfClassifier unitOfClassifier,
            IUnitOfDoctor unitOfDoctor,
            IValidator<CreateDayPlansFromSchemaDto> createDayPlansValidator,
            IValidator<DoctorSchemaCreateDto> doctorSchemaCreateValidator)
        {
            _logger = logger;
            _doctorService = doctorService;
            _timeSlotService = timeSlotService;
            _unitOfService = unitOfService;
            _unitOfClassifier = unitOfClassifier;
            _unitOfDoctor = unitOfDoctor;
            _createDayPlansValidator = createDayPlansValidator;
            _doctorSchemaCreateValidator = doctorSchemaCreateValidator;
        }

        public async Task<Result> CreateDayPlansFromWeeklySchemaAsync(CreateDayPlansFromSchemaDto dto)
        {
            var doctorId = dto.WeeklySchema.DoctorId;
            _logger.LogTrace("Started CreateDayPlansFromWeeklySchema. DoctorId: {DoctorId}, StartDate: {StartDate}",
                doctorId, dto.StartDate);

            var result = Result.Create();

            if (!await ValidateDtoAsync(dto, result))
            {
                _logger.LogDebug("CreateDayPlansFromSchema validation failed for DoctorId: {DoctorId}", doctorId);
                return result;
            }
            _logger.LogDebug("CreateDayPlansFromSchema DTO validation succeeded.");

            var verifiedDoctorResult = await _doctorService.EnsureDoctorIsVerifiedAsync(doctorId);
            if (!verifiedDoctorResult.IsSuccess())
            {
                _logger.LogDebug("Doctor verify check failed. DoctorId: {DoctorId}", doctorId);
                result.MergeResult(verifiedDoctorResult);
                return result;
            }
            _logger.LogDebug("Doctor is verified. DoctorId: {DoctorId}", doctorId);

            var daySchemas = dto.WeeklySchema.DaySchemas?.ToList() ?? new List<DaySchemaDto>();
            if (daySchemas.Count != 7)
            {
                _logger.LogInformation("Weekly schema must have exactly 7 day schemas. Found: {Count}.", daySchemas.Count);
                result.AddMessage("ERR00127", "Weekly schema must have exactly 7 day schemas (one per weekday).", HttpStatusCode.BadRequest);
                return result;
            }
            var distinctDays = daySchemas.Select(x => x.DayOfWeek).Distinct().ToList();
            if (distinctDays.Count != 7 || distinctDays.Any(d => d < 1 || d > 7))
            {
                _logger.LogInformation("Invalid week data: day schemas must cover weekdays 1-7 exactly once.");
                result.AddMessage("ERR00127", "Weekly schema must have exactly 7 day schemas (one per weekday).", HttpStatusCode.BadRequest);
                return result;
            }
            _logger.LogDebug("Day schemas validated: 7 weekdays present.");

            var periodIds = daySchemas.Select(x => x.PeriodId).Distinct().ToList();
            var periods = (await _unitOfClassifier.Period.FindAsync(x => periodIds.Contains(x.Id))).ToDictionary(x => x.Id);
            foreach (var daySchema in daySchemas)
            {
                if (!periods.ContainsKey(daySchema.PeriodId))
                {
                    _logger.LogInformation("Period not found for DaySchema. PeriodId: {PeriodId}", daySchema.PeriodId);
                    result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                    return result;
                }
            }

            var paddingTypeIds = daySchemas.Where(x => x.PlanPaddingTypeId.HasValue).Select(x => x.PlanPaddingTypeId!.Value).Distinct().ToList();
            var paddingTypes = paddingTypeIds.Count > 0
                ? (await _unitOfClassifier.PlanPaddingType.FindAsync(x => paddingTypeIds.Contains(x.Id))).ToDictionary(x => x.Id)
                : new Dictionary<long, PlanPaddingTypeEntity>();
            var currency = await _unitOfClassifier.Currency.GetByIdAsync(dto.CurrencyId);
            if (currency is null)
            {
                _logger.LogInformation("Currency not found. CurrencyId: {CurrencyId}", dto.CurrencyId);
                result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                return result;
            }
            _logger.LogDebug("Classifiers (Period, PlanPaddingType, Currency) loaded from DB.");
            if(dto.StartDate.DayOfWeek != DayOfWeek.Monday)
            {
                _logger.LogInformation("Selected Start Date Day of Week is invalid. This date should be Monday. Date: {0}", dto.StartDate);
                result.AddMessage("ERR00133", "Selected Start Date Day of Week is invalid. This date should be Monday.", HttpStatusCode.Conflict);
                return result;
            }
            //var mondayOfWeek = GetMondayOfWeek(dto.StartDate.Date);

            foreach (var daySchema in daySchemas.OrderBy(x => x.DayOfWeek))
            {
                var period = periods[daySchema.PeriodId];
                PlanPaddingTypeEntity? paddingType = null;
                if (daySchema.PlanPaddingTypeId.HasValue && paddingTypes.TryGetValue(daySchema.PlanPaddingTypeId.Value, out var pt))
                    paddingType = pt;

                var belongDate = dto.StartDate;
                var dayBreaksForSchema = daySchema.DayBreaks.Select(b => (b.StartTime, b.EndTime)).ToList();

                var dayPlan = new DayPlanEntity
                {
                    DoctorId = doctorId,
                    SpecialtyId = daySchema.SpecialtyId,
                    PeriodId = daySchema.PeriodId,
                    BelongDate = belongDate,
                    DayOfWeek = daySchema.DayOfWeek,
                    OpenTime = daySchema.OpenTime,
                    IsClosed = daySchema.IsClosed,
                    CloseTime = daySchema.OpenTime
                };

                if (daySchema.IsClosed)
                {
                    _unitOfService.DayPlan.Add(dayPlan);
                    _logger.LogTrace("DayPlan added (closed day). BelongDate: {BelongDate}, DayOfWeek: {DayOfWeek}", belongDate, daySchema.DayOfWeek);
                    continue;
                }

                var slotsResult = _timeSlotService.GenerateDaySlots(
                    openTime: daySchema.OpenTime,
                    periodTimeMinutes: period.PeriodTime,
                    paddingTimeMinutes: paddingType?.PaddingTime,
                    paddingPosition: paddingType != null && Enum.IsDefined(typeof(PlanPaddingPosition), (PlanPaddingPosition)paddingType.PaddingPosition)
                        ? (PlanPaddingPosition?)paddingType.PaddingPosition
                        : null,
                    periodCount: daySchema.PeriodCount,
                    breaks: dayBreaksForSchema);
                if (!slotsResult.IsSuccess())
                {
                    _logger.LogInformation("Period or break overlap detected for day. BelongDate: {BelongDate}, DayOfWeek: {DayOfWeek}",
                        belongDate, daySchema.DayOfWeek);
                    result.MergeResult(slotsResult);
                    return result;
                }

                var slots = slotsResult.Model!;
                if (slots.Count > 0)
                    dayPlan.CloseTime = slots[^1].End;

                _unitOfService.DayPlan.Add(dayPlan);

                foreach (var (start, end) in slots)
                {
                    var periodPlan = new PeriodPlanEntity
                    {
                        DayPlan = dayPlan,
                        PeriodStart = start,
                        PeriodStop = end,
                        IsOnlineService = daySchema.IsOnlineService,
                        IsOnSiteService = daySchema.IsOnSiteService,
                        PricePerPeriod = dto.PricePerPeriod,
                        CurrencyId = dto.CurrencyId,
                        IsBusy = true
                    };
                    _unitOfService.PeriodPlan.Add(periodPlan);
                }
                _logger.LogDebug("DayPlan and {PeriodCount} PeriodPlans added. BelongDate: {BelongDate}, DayOfWeek: {DayOfWeek}",
                    slots.Count, belongDate, daySchema.DayOfWeek);
            }

            await _unitOfService.SaveChangesAsync();
            _logger.LogInformation("CreateDayPlansFromWeeklySchema completed. DoctorId: {DoctorId}, StartDate: {StartDate}",
                doctorId, dto.StartDate);
            result.Success(HttpStatusCode.Created);
            return result;
        }

        public async Task<Result<long>> AddDoctorSchemaAsync(DoctorSchemaCreateDto dto)
        {
            var doctorId = dto.DoctorId;
            _logger.LogTrace("Started AddDoctorSchema. DoctorId: {DoctorId}, Name: {Name}", doctorId, dto.Name);

            var result = Result<long>.Create();

            var validationResult = await _doctorSchemaCreateValidator.ValidateAsync(dto);
            if (validationResult == null)
            {
                _logger.LogError("Validation result is null for DoctorSchemaCreateDto.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return result;
            }
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("DoctorSchemaCreateDto validation failed. Errors: {Errors}", validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return result;
            }
            _logger.LogDebug("DoctorSchemaCreateDto validation succeeded.");

            var verifiedDoctorResult = await _doctorService.EnsureDoctorIsVerifiedAsync(doctorId);
            if (!verifiedDoctorResult.IsSuccess())
            {
                _logger.LogDebug("Doctor verify check failed. DoctorId: {DoctorId}", doctorId);
                result.MergeResult(verifiedDoctorResult);
                return result;
            }
            _logger.LogDebug("Doctor is verified. DoctorId: {DoctorId}", doctorId);

            var daySchemasList = dto.DaySchemas?.ToList() ?? new List<DaySchemaCreateDto>();
            var specialtyIds = daySchemasList.Select(x => x.SpecialtyId).Distinct().ToList();
            var periodIds = daySchemasList.Select(x => x.PeriodId).Distinct().ToList();
            var paddingTypeIds = daySchemasList.Where(x => x.PlanPaddingTypeId.HasValue).Select(x => x.PlanPaddingTypeId!.Value).Distinct().ToList();

            var specialties = (await _unitOfClassifier.Specialty.FindAsync(x => specialtyIds.Contains(x.Id))).ToDictionary(x => x.Id);
            foreach (var sid in specialtyIds)
            {
                if (!specialties.ContainsKey(sid))
                {
                    _logger.LogInformation("Specialty not found. SpecialtyId: {SpecialtyId}", sid);
                    result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                    return result;
                }
            }

            var periods = (await _unitOfClassifier.Period.FindAsync(x => periodIds.Contains(x.Id))).ToDictionary(x => x.Id);
            foreach (var pid in periodIds)
            {
                if (!periods.ContainsKey(pid))
                {
                    _logger.LogInformation("Period not found. PeriodId: {PeriodId}", pid);
                    result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                    return result;
                }
            }

            if (paddingTypeIds.Count > 0)
            {
                var paddingTypes = (await _unitOfClassifier.PlanPaddingType.FindAsync(x => paddingTypeIds.Contains(x.Id))).ToDictionary(x => x.Id);
                foreach (var ptid in paddingTypeIds)
                {
                    if (!paddingTypes.ContainsKey(ptid))
                    {
                        _logger.LogInformation("PlanPaddingType not found. PlanPaddingTypeId: {PlanPaddingTypeId}", ptid);
                        result.AddMessage("ERR00050", "Classifier item not found.", HttpStatusCode.NotFound);
                        return result;
                    }
                }
            }
            _logger.LogDebug("Classifiers (Specialty, Period, PlanPaddingType) validated.");

            var weeklySchema = new WeeklySchemaEntity
            {
                DoctorId = doctorId,
                Name = dto.Name,
                ColorHex = dto.ColorHex,
                DayPlans = daySchemasList.Select(ds => new DaySchemaEntity
                {
                    SpecialtyId = ds.SpecialtyId,
                    PeriodId = ds.PeriodId,
                    PlanPaddingTypeId = ds.PlanPaddingTypeId,
                    DayOfWeek = ds.DayOfWeek,
                    OpenTime = ds.OpenTime,
                    PeriodCount = ds.PeriodCount,
                    IsClosed = ds.IsClosed,
                    IsOnlineService = ds.IsOnlineService,
                    IsOnSiteService = ds.IsOnSiteService,
                    DayBreaks = (ds.DayBreaks ?? new List<DayBreakCreateDto>()).Select(db => new DayBreakEntity
                    {
                        Name = db.Name,
                        IsVisible = db.IsVisible,
                        StartTime = db.StartTime,
                        EndTime = db.EndTime
                    }).ToList()
                }).ToList()
            };

            _unitOfDoctor.WeeklySchema.Add(weeklySchema);
            await _unitOfDoctor.SaveChangesAsync();

            _logger.LogInformation("Doctor schema added. DoctorId: {DoctorId}, WeeklySchemaId: {WeeklySchemaId}", doctorId, weeklySchema.Id);
            result.AddMessage("ERR00149", "Doctor weekly schema created successfully.", HttpStatusCode.Created);
            result.Success(weeklySchema.Id, HttpStatusCode.Created);
            return result;
        }

        private async Task<bool> ValidateDtoAsync(CreateDayPlansFromSchemaDto dto, Result result)
        {
            _logger.LogTrace("Validating CreateDayPlansFromSchemaDto.");
            var validationResult = await _createDayPlansValidator.ValidateAsync(dto);
            if (validationResult == null)
            {
                _logger.LogError("Validation result is null for CreateDayPlansFromSchemaDto.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.BadRequest);
                return false;
            }
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("CreateDayPlansFromSchemaDto validation failed. Errors: {Errors}", validationResult.Errors);
                result.SetFluentValidationAndBadRequest(validationResult);
                return false;
            }
            return true;
        }
    }
}
