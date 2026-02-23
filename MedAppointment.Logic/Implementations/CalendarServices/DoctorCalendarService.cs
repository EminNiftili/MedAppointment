namespace MedAppointment.Logics.Implementations.CalendarServices
{
    internal class DoctorCalendarService : IDoctorCalendarService
    {
        private readonly ILogger<DoctorCalendarService> _logger;
        private readonly IUnitOfService _unitOfService;
        private readonly IUnitOfDoctor _unitOfDoctor;
        private readonly IUnitOfClassifier _unitOfClassifier;
        private readonly IValidator<DoctorCalendarWeekQueryDto> _queryValidator;
        private readonly IValidator<EditDayPlanDto> _editDayPlanValidator;

        public DoctorCalendarService(
            ILogger<DoctorCalendarService> logger,
            IUnitOfService unitOfService,
            IUnitOfDoctor unitOfDoctor,
            IUnitOfClassifier unitOfClassifier,
            IValidator<DoctorCalendarWeekQueryDto> queryValidator,
            IValidator<EditDayPlanDto> editDayPlanValidator)
        {
            _logger = logger;
            _unitOfService = unitOfService;
            _unitOfDoctor = unitOfDoctor;
            _unitOfClassifier = unitOfClassifier;
            _queryValidator = queryValidator;
            _editDayPlanValidator = editDayPlanValidator;
        }

        public async Task<Result<DoctorCalendarWeekResponseDto>> GetWeeklyCalendarAsync(DoctorCalendarWeekQueryDto query)
        {
            _logger.LogTrace("GetWeeklyCalendarAsync started. DoctorId: {DoctorId}, WeekStartDate: {WeekStartDate}",
                query.DoctorId, query.WeekStartDate);

            var result = Result<DoctorCalendarWeekResponseDto>.Create();

            var validationResult = await _queryValidator.ValidateAsync(query);
            if (validationResult == null)
            {
                _logger.LogError("Validation result is null for DoctorCalendarWeekQueryDto.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.InternalServerError);
                return result;
            }
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("DoctorCalendarWeekQueryDto validation failed. Errors: {ErrorCount}", validationResult.Errors.Count);
                result.SetFluentValidationAndBadRequest(validationResult);
                return result;
            }
            _logger.LogDebug("DoctorCalendarWeekQueryDto validation succeeded.");

            var doctorId = query.DoctorId;
            var weekStart = query.WeekStartDate.Date;
            var weekEnd = weekStart.AddDays(7);

            var doctor = await _unitOfDoctor.Doctor.GetByIdAsync(doctorId);
            if (doctor == null || doctor.IsDeleted)
            {
                _logger.LogInformation("Doctor not found or deleted. DoctorId: {DoctorId}", doctorId);
                result.AddMessage("ERR00153", "Doctor not found.", HttpStatusCode.NotFound);
                return result;
            }
            _logger.LogDebug("Doctor found. DoctorId: {DoctorId}", doctorId);

            var dayPlans = (await _unitOfService.DayPlan.FindAsync(x =>
                x.DoctorId == doctorId &&
                x.BelongDate >= weekStart &&
                x.BelongDate < weekEnd &&
                !x.IsDeleted)).ToList();

            _logger.LogDebug("Day plans loaded for week. Count: {Count}", dayPlans.Count);

            var dayPlanIds = dayPlans.Select(x => x.Id).ToList();
            var periodPlans = dayPlanIds.Count > 0
                ? (await _unitOfService.PeriodPlan.FindAsync(x =>
                    dayPlanIds.Contains(x.DayPlanId) && !x.IsDeleted)).ToList()
                : new List<PeriodPlanEntity>();

            _logger.LogDebug("Period plans loaded. Count: {Count}", periodPlans.Count);

            var dayPlanById = dayPlans.ToDictionary(x => x.Id);
            var periodPlansByDayPlanId = periodPlans
                .Where(pp => pp.DayPlan != null)
                .GroupBy(pp => pp.DayPlanId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var daysList = new List<DoctorCalendarDayDto>();
            for (var d = 0; d < 7; d++)
            {
                var date = weekStart.AddDays(d);
                var dayPlansForDate = dayPlans.Where(dp => dp.BelongDate.Date == date).ToList();
                var isClosed = dayPlansForDate.Count == 0;
                var slots = new List<DoctorCalendarPeriodSlotDto>();

                foreach (var dayPlan in dayPlansForDate)
                {
                    if (!periodPlansByDayPlanId.TryGetValue(dayPlan.Id, out var periods))
                        continue;
                    var periodEntity = dayPlan.Period;
                    var specialtyEntity = dayPlan.Specialty;
                    foreach (var pp in periods.OrderBy(p => p.PeriodStart))
                    {
                        var currency = pp.Currency;
                        slots.Add(new DoctorCalendarPeriodSlotDto
                        {
                            PeriodPlanId = pp.Id,
                            PeriodStart = pp.PeriodStart,
                            PeriodStop = pp.PeriodStop,
                            PricePerPeriod = pp.PricePerPeriod,
                            CurrencyId = pp.CurrencyId,
                            CurrencyKey = currency?.Key ?? string.Empty,
                            PeriodId = dayPlan.PeriodId,
                            PeriodTimeMinutes = periodEntity?.PeriodTime ?? 0,
                            SpecialtyId = dayPlan.SpecialtyId,
                            IsBusy = pp.IsBusy,
                            IsOnlineService = pp.IsOnlineService,
                            IsOnSiteService = pp.IsOnSiteService
                        });
                    }
                }

                daysList.Add(new DoctorCalendarDayDto
                {
                    Date = date,
                    DayOfWeek = (int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek,
                    IsClosed = isClosed,
                    Periods = slots
                });
            }

            var response = new DoctorCalendarWeekResponseDto
            {
                DoctorId = doctorId,
                WeekStartDate = weekStart,
                WeekEndDate = weekEnd.AddDays(-1),
                Days = daysList
            };

            result.Success(response);
            _logger.LogInformation("GetWeeklyCalendarAsync completed. DoctorId: {DoctorId}, WeekStart: {WeekStart}, DaysWithPlans: {DaysWithPlans}",
                doctorId, weekStart, dayPlans.Select(dp => dp.BelongDate.Date).Distinct().Count());
            return result;
        }

        public async Task<Result> EditDayPlanAsync(EditDayPlanDto dto)
        {
            _logger.LogTrace("EditDayPlanAsync started. DayPlanId: {DayPlanId}, DoctorId: {DoctorId}, SpecialtyId: {SpecialtyId}, IsClosed: {IsClosed}",
                dto.DayPlanId, dto.DoctorId, dto.SpecialtyId, dto.IsClosed);

            var result = Result.Create();

            var validationResult = await _editDayPlanValidator.ValidateAsync(dto);
            if (validationResult == null)
            {
                _logger.LogError("Validation result is null for EditDayPlanDto.");
                result.AddMessage("ERR00100", "Unexpected error contact with admin", HttpStatusCode.InternalServerError);
                return result;
            }
            if (!validationResult.IsValid)
            {
                _logger.LogDebug("EditDayPlanDto validation failed. Errors: {ErrorCount}", validationResult.Errors.Count);
                result.SetFluentValidationAndBadRequest(validationResult);
                return result;
            }
            _logger.LogDebug("EditDayPlanDto validation succeeded.");

            var dayPlan = await _unitOfService.DayPlan.GetByIdAsync(dto.DayPlanId);
            if (dayPlan == null || dayPlan.IsDeleted)
            {
                _logger.LogInformation("Day plan not found or deleted. DayPlanId: {DayPlanId}", dto.DayPlanId);
                result.AddMessage("ERR00156", "Day plan not found.", HttpStatusCode.NotFound);
                return result;
            }
            if (dayPlan.DoctorId != dto.DoctorId)
            {
                _logger.LogInformation("Day plan does not belong to doctor. DayPlanId: {DayPlanId}, DayPlanDoctorId: {DayPlanDoctorId}, RequestedDoctorId: {DoctorId}",
                    dto.DayPlanId, dayPlan.DoctorId, dto.DoctorId);
                result.AddMessage("ERR00156", "Day plan not found.", HttpStatusCode.NotFound);
                return result;
            }
            _logger.LogDebug("Day plan found and belongs to doctor. DayPlanId: {DayPlanId}", dto.DayPlanId);

            var specialty = await _unitOfClassifier.Specialty.GetByIdAsync(dto.SpecialtyId);
            if (specialty == null || specialty.IsDeleted)
            {
                _logger.LogInformation("Specialty not found or deleted. SpecialtyId: {SpecialtyId}", dto.SpecialtyId);
                result.AddMessage("ERR00158", "Specialty not found.", HttpStatusCode.NotFound);
                return result;
            }
            _logger.LogDebug("Specialty found. SpecialtyId: {SpecialtyId}", dto.SpecialtyId);

            dayPlan.SpecialtyId = dto.SpecialtyId;
            dayPlan.IsClosed = dto.IsClosed;
            _unitOfService.DayPlan.Update(dayPlan);
            await _unitOfService.SaveChangesAsync();

            result.Success();
            _logger.LogInformation("EditDayPlanAsync completed. DayPlanId: {DayPlanId}, DoctorId: {DoctorId}",
                dto.DayPlanId, dto.DoctorId);
            return result;
        }
    }
}
