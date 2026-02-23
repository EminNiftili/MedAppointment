namespace MedAppointment.Logic.Tests.Services.CalendarServices;

public class DoctorCalendarServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.CalendarServices.DoctorCalendarService";

    private readonly ILogger _logger;
    private readonly IUnitOfService _unitOfService;
    private readonly IUnitOfDoctor _unitOfDoctor;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly IValidator<DoctorCalendarWeekQueryDto> _queryValidator;
    private readonly IValidator<EditDayPlanDto> _editDayPlanValidator;
    private readonly IValidator<EditPeriodPlanDto> _editPeriodPlanValidator;
    private readonly IDayPlanRepository _dayPlanRepo;
    private readonly IPeriodPlanRepository _periodPlanRepo;
    private readonly IDoctorRepository _doctorRepo;
    private readonly ISpecialtyRepository _specialtyRepo;
    private readonly ICurrencyRepository _currencyRepo;
    private readonly IDoctorCalendarService _sut;

    public DoctorCalendarServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _unitOfService = Substitute.For<IUnitOfService>();
        _unitOfDoctor = Substitute.For<IUnitOfDoctor>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _queryValidator = Substitute.For<IValidator<DoctorCalendarWeekQueryDto>>();
        _editDayPlanValidator = Substitute.For<IValidator<EditDayPlanDto>>();
        _editPeriodPlanValidator = Substitute.For<IValidator<EditPeriodPlanDto>>();

        _dayPlanRepo = Substitute.For<IDayPlanRepository>();
        _periodPlanRepo = Substitute.For<IPeriodPlanRepository>();
        _doctorRepo = Substitute.For<IDoctorRepository>();

        _unitOfService.DayPlan.Returns(_dayPlanRepo);
        _unitOfService.PeriodPlan.Returns(_periodPlanRepo);
        _unitOfDoctor.Doctor.Returns(_doctorRepo);
        _specialtyRepo = Substitute.For<ISpecialtyRepository>();
        _currencyRepo = Substitute.For<ICurrencyRepository>();
        _unitOfClassifier.Specialty.Returns(_specialtyRepo);
        _unitOfClassifier.Currency.Returns(_currencyRepo);

        _sut = ServiceReflectionHelper.CreateService<IDoctorCalendarService>(ServiceTypeName,
            _logger,
            _unitOfService,
            _unitOfDoctor,
            _unitOfClassifier,
            _queryValidator,
            _editDayPlanValidator,
            _editPeriodPlanValidator);
    }

    [Fact]
    public async Task GetWeeklyCalendarAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicCalendar.ValidWeekQuery;
        _queryValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("DoctorId", "Invalid") }));

        var result = await _sut.GetWeeklyCalendarAsync(query);

        Assert.False(result.IsSuccess());
        await _doctorRepo.DidNotReceive().GetByIdAsync(Arg.Any<long>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task GetWeeklyCalendarAsync_WhenDoctorNotFound_ReturnsNotFound()
    {
        var query = MagicCalendar.ValidWeekQuery;
        _queryValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.GetWeeklyCalendarAsync(query);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00153");
    }

    [Fact]
    public async Task GetWeeklyCalendarAsync_WhenDoctorFound_ReturnsWeekResponseWithSevenDays()
    {
        var query = MagicCalendar.ValidWeekQuery;
        _queryValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicCalendar.DoctorOne);
        _dayPlanRepo.FindAsync(Arg.Any<Expression<Func<DayPlanEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<DayPlanEntity>>(new List<DayPlanEntity>()));
        _periodPlanRepo.FindAsync(Arg.Any<Expression<Func<PeriodPlanEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PeriodPlanEntity>>(new List<PeriodPlanEntity>()));

        var result = await _sut.GetWeeklyCalendarAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(MagicCalendar.DoctorIdOne, result.Model!.DoctorId);
        Assert.Equal(7, result.Model.Days.Count);
    }

    [Fact]
    public async Task EditDayPlanAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicCalendar.ValidEditDayPlanDto;
        _editDayPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("DayPlanId", "Invalid") }));

        var result = await _sut.EditDayPlanAsync(dto);

        Assert.False(result.IsSuccess());
        await _dayPlanRepo.DidNotReceive().GetByIdAsync(Arg.Any<long>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task EditDayPlanAsync_WhenDayPlanNotFound_ReturnsNotFound()
    {
        var dto = MagicCalendar.ValidEditDayPlanDto;
        _editDayPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _dayPlanRepo.GetByIdAsync(dto.DayPlanId, Arg.Any<bool>()).Returns((DayPlanEntity?)null);

        var result = await _sut.EditDayPlanAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00156");
    }

    [Fact]
    public async Task EditDayPlanAsync_WhenSpecialtyNotFound_ReturnsNotFound()
    {
        var dto = MagicCalendar.ValidEditDayPlanDto;
        var dayPlan = MagicCalendar.DayPlanOne;
        _editDayPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _dayPlanRepo.GetByIdAsync(dto.DayPlanId, Arg.Any<bool>()).Returns(dayPlan);
        _specialtyRepo.GetByIdAsync(dto.SpecialtyId, Arg.Any<bool>()).Returns((SpecialtyEntity?)null);

        var result = await _sut.EditDayPlanAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00158");
    }

    [Fact]
    public async Task EditDayPlanAsync_WhenValid_UpdatesAndReturnsSuccess()
    {
        var dto = MagicCalendar.ValidEditDayPlanDto;
        var dayPlan = MagicCalendar.DayPlanOne;
        var specialty = MagicSpecialty.EntityOneWithLocalization;
        _editDayPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _dayPlanRepo.GetByIdAsync(dto.DayPlanId, Arg.Any<bool>()).Returns(dayPlan);
        _specialtyRepo.GetByIdAsync(dto.SpecialtyId, Arg.Any<bool>()).Returns(specialty);

        var result = await _sut.EditDayPlanAsync(dto);

        Assert.True(result.IsSuccess());
        _dayPlanRepo.Received(1).Update(Arg.Any<DayPlanEntity>());
        await _unitOfService.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task EditPeriodPlanAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicCalendar.ValidEditPeriodPlanDto;
        _editPeriodPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PeriodPlanId", "Invalid") }));

        var result = await _sut.EditPeriodPlanAsync(dto);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task EditPeriodPlanAsync_WhenPeriodPlanNotFound_ReturnsNotFound()
    {
        var dto = MagicCalendar.ValidEditPeriodPlanDto;
        _editPeriodPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodPlanRepo.GetByIdAsync(dto.PeriodPlanId, Arg.Any<bool>()).Returns((PeriodPlanEntity?)null);

        var result = await _sut.EditPeriodPlanAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00160");
    }

    [Fact]
    public async Task EditPeriodPlanAsync_WhenCurrencyNotFound_ReturnsNotFound()
    {
        var dto = MagicCalendar.ValidEditPeriodPlanDto;
        var periodPlan = MagicCalendar.PeriodPlanOne;
        periodPlan.DayPlan = MagicCalendar.DayPlanOne;
        _editPeriodPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodPlanRepo.GetByIdAsync(dto.PeriodPlanId, Arg.Any<bool>()).Returns(periodPlan);
        _currencyRepo.GetByIdAsync(dto.CurrencyId, Arg.Any<bool>()).Returns((CurrencyEntity?)null);

        var result = await _sut.EditPeriodPlanAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task EditPeriodPlanAsync_WhenValid_UpdatesAndReturnsSuccess()
    {
        var dto = MagicCalendar.ValidEditPeriodPlanDto;
        var periodPlan = MagicCalendar.PeriodPlanOne;
        periodPlan.DayPlan = MagicCalendar.DayPlanOne;
        var currency = MagicCurrency.EntityOne;
        _editPeriodPlanValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodPlanRepo.GetByIdAsync(dto.PeriodPlanId, Arg.Any<bool>()).Returns(periodPlan);
        _currencyRepo.GetByIdAsync(dto.CurrencyId, Arg.Any<bool>()).Returns(currency);
        _periodPlanRepo.FindAsync(Arg.Any<Expression<Func<PeriodPlanEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PeriodPlanEntity>>(new List<PeriodPlanEntity>()));

        var result = await _sut.EditPeriodPlanAsync(dto);

        Assert.True(result.IsSuccess());
        _periodPlanRepo.Received(1).Update(Arg.Any<PeriodPlanEntity>());
        await _unitOfService.Received(1).SaveChangesAsync();
    }
}
