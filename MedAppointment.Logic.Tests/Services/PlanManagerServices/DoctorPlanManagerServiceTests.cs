namespace MedAppointment.Logic.Tests.Services.PlanManagerServices;

public class DoctorPlanManagerServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.PlanManagerServices.DoctorPlanManagerService";

    private readonly ILogger _logger;
    private readonly IDoctorService _doctorService;
    private readonly ITimeSlotService _timeSlotService;
    private readonly IUnitOfService _unitOfService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly IUnitOfDoctor _unitOfDoctor;
    private readonly IValidator<CreateDayPlansFromSchemaDto> _createDayPlansValidator;
    private readonly IValidator<CreateDayPlansFromWeeklySchemaByIdDto> _createDayPlansByIdValidator;
    private readonly IValidator<DoctorSchemaCreateDto> _doctorSchemaCreateValidator;
    private readonly IDayPlanRepository _dayPlanRepo;
    private readonly IPeriodPlanRepository _periodPlanRepo;
    private readonly IPeriodRepository _periodRepo;
    private readonly IPlanPaddingTypeRepository _planPaddingTypeRepo;
    private readonly ICurrencyRepository _currencyRepo;
    private readonly ISpecialtyRepository _specialtyRepo;
    private readonly IWeeklySchemaRepository _weeklySchemaRepo;
    private readonly IDoctorPlanManagerService _sut;

    public DoctorPlanManagerServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _doctorService = Substitute.For<IDoctorService>();
        _timeSlotService = Substitute.For<ITimeSlotService>();
        _unitOfService = Substitute.For<IUnitOfService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _unitOfDoctor = Substitute.For<IUnitOfDoctor>();
        _createDayPlansValidator = Substitute.For<IValidator<CreateDayPlansFromSchemaDto>>();
        _createDayPlansByIdValidator = Substitute.For<IValidator<CreateDayPlansFromWeeklySchemaByIdDto>>();
        _doctorSchemaCreateValidator = Substitute.For<IValidator<DoctorSchemaCreateDto>>();

        _dayPlanRepo = Substitute.For<IDayPlanRepository>();
        _periodPlanRepo = Substitute.For<IPeriodPlanRepository>();
        _unitOfService.DayPlan.Returns(_dayPlanRepo);
        _unitOfService.PeriodPlan.Returns(_periodPlanRepo);

        _periodRepo = Substitute.For<IPeriodRepository>();
        _planPaddingTypeRepo = Substitute.For<IPlanPaddingTypeRepository>();
        _currencyRepo = Substitute.For<ICurrencyRepository>();
        _specialtyRepo = Substitute.For<ISpecialtyRepository>();
        _unitOfClassifier.Period.Returns(_periodRepo);
        _unitOfClassifier.PlanPaddingType.Returns(_planPaddingTypeRepo);
        _unitOfClassifier.Currency.Returns(_currencyRepo);
        _unitOfClassifier.Specialty.Returns(_specialtyRepo);

        _weeklySchemaRepo = Substitute.For<IWeeklySchemaRepository>();
        _unitOfDoctor.WeeklySchema.Returns(_weeklySchemaRepo);

        _sut = ServiceReflectionHelper.CreateService<IDoctorPlanManagerService>(ServiceTypeName,
            _logger,
            _doctorService,
            _timeSlotService,
            _unitOfService,
            _unitOfClassifier,
            _unitOfDoctor,
            _createDayPlansValidator,
            _createDayPlansByIdValidator,
            _doctorSchemaCreateValidator);
    }

    [Fact]
    public async Task CreateDayPlansFromWeeklySchemaAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicCalendar.ValidCreateDayPlansFromSchemaDto;
        _createDayPlansValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("StartDate", "Invalid") }));

        var result = await _sut.CreateDayPlansFromWeeklySchemaAsync(dto);

        Assert.False(result.IsSuccess());
        await _dayPlanRepo.DidNotReceive().AddAsync(Arg.Any<DayPlanEntity>());
    }

    [Fact]
    public async Task CreateDayPlansFromWeeklySchemaAsync_WhenExistingDayPlanInWeek_ReturnsBadRequest()
    {
        var dto = MagicCalendar.ValidCreateDayPlansFromSchemaDto;
        _createDayPlansValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _dayPlanRepo.AnyAsync(Arg.Any<Expression<Func<DayPlanEntity, bool>>>()).Returns(true);

        var result = await _sut.CreateDayPlansFromWeeklySchemaAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00154");
    }

    [Fact]
    public async Task CreateDayPlansFromWeeklySchemaAsync_WhenDoctorNotVerified_MergesUnsuccessfulResult()
    {
        var dto = MagicCalendar.ValidCreateDayPlansFromSchemaDto;
        _createDayPlansValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _dayPlanRepo.AnyAsync(Arg.Any<Expression<Func<DayPlanEntity, bool>>>()).Returns(false);
        var failedResult = Result.Create();
        failedResult.AddMessage("ERR00152", "Doctor not verified.", HttpStatusCode.Forbidden);
        _doctorService.EnsureDoctorIsVerifiedAsync(MagicCalendar.DoctorIdOne).Returns(failedResult);

        var result = await _sut.CreateDayPlansFromWeeklySchemaAsync(dto);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task CreateDayPlansFromWeeklySchemaAsync_WhenStartDateNotMonday_ReturnsConflict()
    {
        var dto = MagicCalendar.ValidCreateDayPlansFromSchemaDto with { StartDate = new DateTime(2024, 6, 5, 0, 0, 0, DateTimeKind.Utc) }; // Wednesday
        _createDayPlansValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _dayPlanRepo.AnyAsync(Arg.Any<Expression<Func<DayPlanEntity, bool>>>()).Returns(false);
        _doctorService.EnsureDoctorIsVerifiedAsync(MagicCalendar.DoctorIdOne).Returns(Result.Create());
        _periodRepo.FindAsync(Arg.Any<Expression<Func<PeriodEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PeriodEntity>>(new List<PeriodEntity> { MagicPeriod.EntityOneWithLocalization }));
        _currencyRepo.GetByIdAsync(dto.CurrencyId, Arg.Any<bool>()).Returns(MagicCurrency.EntityOne);

        var result = await _sut.CreateDayPlansFromWeeklySchemaAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00133");
    }

    [Fact]
    public async Task CreateDayPlansFromWeeklySchemaByIdAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = new CreateDayPlansFromWeeklySchemaByIdDto
        {
            WeeklySchemaId = 1,
            StartDate = MagicCalendar.MondayStart,
            CurrencyId = MagicIds.CurrencyIdOne,
            PricePerPeriod = 50m
        };
        _createDayPlansByIdValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("WeeklySchemaId", "Invalid") }));

        var result = await _sut.CreateDayPlansFromWeeklySchemaByIdAsync(dto);

        Assert.False(result.IsSuccess());
        await _weeklySchemaRepo.DidNotReceive().GetByIdAsync(Arg.Any<long>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task CreateDayPlansFromWeeklySchemaByIdAsync_WhenWeeklySchemaNotFound_ReturnsNotFound()
    {
        var dto = new CreateDayPlansFromWeeklySchemaByIdDto
        {
            WeeklySchemaId = MagicIds.NonExistentId,
            StartDate = MagicCalendar.MondayStart,
            CurrencyId = MagicIds.CurrencyIdOne,
            PricePerPeriod = 50m
        };
        _createDayPlansByIdValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _weeklySchemaRepo.GetByIdAsync(dto.WeeklySchemaId, Arg.Any<bool>()).Returns((WeeklySchemaEntity?)null);

        var result = await _sut.CreateDayPlansFromWeeklySchemaByIdAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00126");
    }

    [Fact]
    public async Task AddDoctorSchemaAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicCalendar.ValidDoctorSchemaCreateDto;
        _doctorSchemaCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Required") }));

        var result = await _sut.AddDoctorSchemaAsync(dto);

        Assert.False(result.IsSuccess());
        _weeklySchemaRepo.DidNotReceive().Add(Arg.Any<WeeklySchemaEntity>());
    }

    [Fact]
    public async Task AddDoctorSchemaAsync_WhenDoctorNotVerified_MergesUnsuccessfulResult()
    {
        var dto = MagicCalendar.ValidDoctorSchemaCreateDto;
        _doctorSchemaCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        var failedResult = Result.Create();
        failedResult.AddMessage("ERR00152", "Doctor not verified.", HttpStatusCode.Forbidden);
        _doctorService.EnsureDoctorIsVerifiedAsync(dto.DoctorId).Returns(failedResult);

        var result = await _sut.AddDoctorSchemaAsync(dto);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task AddDoctorSchemaAsync_WhenSpecialtyNotFound_ReturnsNotFound()
    {
        var dto = MagicCalendar.ValidDoctorSchemaCreateDto;
        _doctorSchemaCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorService.EnsureDoctorIsVerifiedAsync(dto.DoctorId).Returns(Result.Create());
        _specialtyRepo.FindAsync(Arg.Any<Expression<Func<SpecialtyEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<SpecialtyEntity>>(new List<SpecialtyEntity>())); // empty - specialty not found

        var result = await _sut.AddDoctorSchemaAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task AddDoctorSchemaAsync_WhenValid_AddsSchemaAndReturnsCreated()
    {
        var dto = MagicCalendar.ValidDoctorSchemaCreateDto;
        _doctorSchemaCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorService.EnsureDoctorIsVerifiedAsync(dto.DoctorId).Returns(Result.Create());
        _specialtyRepo.FindAsync(Arg.Any<Expression<Func<SpecialtyEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<SpecialtyEntity>>(new List<SpecialtyEntity> { MagicSpecialty.EntityOneWithLocalization }));
        _periodRepo.FindAsync(Arg.Any<Expression<Func<PeriodEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PeriodEntity>>(new List<PeriodEntity> { MagicPeriod.EntityOneWithLocalization }));

        var result = await _sut.AddDoctorSchemaAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Created, result.HttpStatus);
        _weeklySchemaRepo.Received(1).Add(Arg.Any<WeeklySchemaEntity>());
        await _unitOfDoctor.Received(1).SaveChangesAsync();
    }
}
