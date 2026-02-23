using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class PeriodServiceTests
{
    private const string PeriodServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.PeriodService";

    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<PeriodCreateDto> _createValidator;
    private readonly IValidator<PeriodUpdateDto> _updateValidator;
    private readonly IValidator<PeriodPaginationQueryDto> _paginationValidator;
    private readonly IClassifierFilterExpressionStrategy<PeriodEntity, PeriodPaginationQueryDto> _filterStrategy;
    private readonly ITranslationLookupService _translationLookup;
    private readonly IPeriodRepository _periodRepo;
    private readonly IPeriodService _sut;

    public PeriodServiceTests()
    {
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(PeriodServiceTypeName);
        _createValidator = Substitute.For<IValidator<PeriodCreateDto>>();
        _updateValidator = Substitute.For<IValidator<PeriodUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<PeriodPaginationQueryDto>>();
        _filterStrategy = Substitute.For<IClassifierFilterExpressionStrategy<PeriodEntity, PeriodPaginationQueryDto>>();
        _translationLookup = Substitute.For<ITranslationLookupService>();
        _periodRepo = Substitute.For<IPeriodRepository>();
        _unitOfClassifier.Period.Returns(_periodRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<IPeriodService>(
            PeriodServiceTypeName,
            _localizerService,
            _unitOfClassifier,
            _logger,
            _createValidator,
            _updateValidator,
            _paginationValidator,
            _filterStrategy,
            _translationLookup);
    }

    [Fact]
    public async Task GetPeriodsAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicPeriod.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetPeriodsAsync(query);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task GetPeriodsAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicPeriod.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<PeriodEntity> { MagicPeriod.EntityOneWithLocalization };
        _periodRepo.FindAsync(Arg.Any<Expression<Func<PeriodEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PeriodEntity>>(entities));

        var result = await _sut.GetPeriodsAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model!.TotalCount);
        Assert.Equal(30, result.Model.Items.First().PeriodTime);
    }

    [Fact]
    public async Task GetPeriodByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _periodRepo.GetByIdAsync(99999, Arg.Any<bool>()).Returns((PeriodEntity?)null);

        var result = await _sut.GetPeriodByIdAsync(99999);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task GetPeriodByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicPeriod.EntityOneWithLocalization;
        _periodRepo.GetByIdAsync(MagicPeriod.PeriodIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetPeriodByIdAsync(MagicPeriod.PeriodIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.PeriodTime, result.Model.PeriodTime);
    }

    [Fact]
    public async Task CreatePeriodAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicPeriod.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Key", "Required") }));

        var result = await _sut.CreatePeriodAsync(dto);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task CreatePeriodAsync_WhenKeyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicPeriod.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodRepo.AnyAsync(Arg.Any<Expression<Func<PeriodEntity, bool>>>()).Returns(true);

        var result = await _sut.CreatePeriodAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
    }

    [Fact]
    public async Task CreatePeriodAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicPeriod.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodRepo.AnyAsync(Arg.Any<Expression<Func<PeriodEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var result = await _sut.CreatePeriodAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _periodRepo.Received(1).AddAsync(Arg.Is<PeriodEntity>(e => e.Key == dto.Key && e.PeriodTime == dto.PeriodTime));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdatePeriodAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<PeriodUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodRepo.GetByIdAsync(99999, Arg.Any<bool>()).Returns((PeriodEntity?)null);

        var result = await _sut.UpdatePeriodAsync(99999, MagicPeriod.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task UpdatePeriodAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicPeriod.EntityOneWithLocalization;
        entity.Description = MagicClassifierHelper.ResourceWithTranslation("period_desc", MagicIds.LanguageIdOne, "desc");
        _updateValidator.ValidateAsync(Arg.Any<PeriodUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _periodRepo.GetByIdAsync(MagicPeriod.PeriodIdOne, Arg.Any<bool>()).Returns(entity);
        _periodRepo.AnyAsync(Arg.Any<Expression<Func<PeriodEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var dto = MagicPeriod.ValidUpdateDto;
        var result = await _sut.UpdatePeriodAsync(MagicPeriod.PeriodIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _periodRepo.Received(1).Update(Arg.Any<PeriodEntity>());
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
