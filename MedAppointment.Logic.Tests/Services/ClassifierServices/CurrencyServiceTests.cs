using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class CurrencyServiceTests
{
    private const string CurrencyServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.CurrencyService";

    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<CurrencyCreateDto> _createValidator;
    private readonly IValidator<CurrencyUpdateDto> _updateValidator;
    private readonly IValidator<CurrencyPaginationQueryDto> _paginationValidator;
    private readonly IClassifierFilterExpressionStrategy<CurrencyEntity, CurrencyPaginationQueryDto> _filterStrategy;
    private readonly ITranslationLookupService _translationLookupService;
    private readonly ICurrencyRepository _currencyRepo;
    private readonly ICurrencyService _sut;

    public CurrencyServiceTests()
    {
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(CurrencyServiceTypeName);
        _createValidator = Substitute.For<IValidator<CurrencyCreateDto>>();
        _updateValidator = Substitute.For<IValidator<CurrencyUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<CurrencyPaginationQueryDto>>();
        _filterStrategy = Substitute.For<IClassifierFilterExpressionStrategy<CurrencyEntity, CurrencyPaginationQueryDto>>();
        _translationLookupService = Substitute.For<ITranslationLookupService>();
        _currencyRepo = Substitute.For<ICurrencyRepository>();

        _unitOfClassifier.Currency.Returns(_currencyRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<ICurrencyService>(
            CurrencyServiceTypeName,
            _localizerService,
            _unitOfClassifier,
            _logger,
            _createValidator,
            _updateValidator,
            _paginationValidator,
            _filterStrategy,
            _translationLookupService);
    }

    [Fact]
    public async Task GetCurrenciesAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicCurrency.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetCurrenciesAsync(query);

        Assert.False(result.IsSuccess());
        await _currencyRepo.DidNotReceive().FindAsync(Arg.Any<Expression<Func<CurrencyEntity, bool>>>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task GetCurrenciesAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicCurrency.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<CurrencyEntity> { MagicCurrency.EntityOneWithLocalization };
        _currencyRepo.FindAsync(Arg.Any<Expression<Func<CurrencyEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<CurrencyEntity>>(entities));

        var result = await _sut.GetCurrenciesAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model!.TotalCount);
        Assert.Equal(query.PageNumber, result.Model.PageNumber);
        Assert.Equal(query.PageSize, result.Model.PageSize);
    }

    [Fact]
    public async Task GetCurrencyByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _currencyRepo.GetByIdAsync(MagicIds.CurrencyIdNonExistent, Arg.Any<bool>()).Returns((CurrencyEntity?)null);

        var result = await _sut.GetCurrencyByIdAsync(MagicIds.CurrencyIdNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task GetCurrencyByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicCurrency.EntityOneWithLocalization;
        _currencyRepo.GetByIdAsync(MagicIds.CurrencyIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetCurrencyByIdAsync(MagicIds.CurrencyIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.Key, result.Model.Key);
        Assert.Equal(entity.Coefficent, result.Model.Coefficent);
    }

    [Fact]
    public async Task CreateCurrencyAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicCurrency.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Key", "Required") }));

        var result = await _sut.CreateCurrencyAsync(dto);

        Assert.False(result.IsSuccess());
        await _currencyRepo.DidNotReceive().AddAsync(Arg.Any<CurrencyEntity>());
    }

    [Fact]
    public async Task CreateCurrencyAsync_WhenKeyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicCurrency.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _currencyRepo.AnyAsync(Arg.Any<Expression<Func<CurrencyEntity, bool>>>()).Returns(true);

        var result = await _sut.CreateCurrencyAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00051");
    }

    [Fact]
    public async Task CreateCurrencyAsync_WhenLocalizerFails_ReturnsMergedErrors()
    {
        var dto = MagicCurrency.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _currencyRepo.AnyAsync(Arg.Any<Expression<Func<CurrencyEntity, bool>>>()).Returns(false);
        var failedResult = Result<long>.Create();
        failedResult.AddMessage("ERR00100", "Localization failed", HttpStatusCode.BadRequest);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(failedResult);

        var result = await _sut.CreateCurrencyAsync(dto);

        Assert.False(result.IsSuccess());
        await _currencyRepo.DidNotReceive().AddAsync(Arg.Any<CurrencyEntity>());
    }

    [Fact]
    public async Task CreateCurrencyAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicCurrency.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _currencyRepo.AnyAsync(Arg.Any<Expression<Func<CurrencyEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var result = await _sut.CreateCurrencyAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _currencyRepo.Received(1).AddAsync(Arg.Is<CurrencyEntity>(e => e.Key == dto.Key && e.Coefficent == dto.Coefficent));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateCurrencyAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<CurrencyUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _currencyRepo.GetByIdAsync(MagicIds.CurrencyIdNonExistent, Arg.Any<bool>()).Returns((CurrencyEntity?)null);

        var result = await _sut.UpdateCurrencyAsync(MagicIds.CurrencyIdNonExistent, MagicCurrency.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task UpdateCurrencyAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicCurrency.EntityOneWithLocalization;
        _updateValidator.ValidateAsync(Arg.Any<CurrencyUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _currencyRepo.GetByIdAsync(MagicIds.CurrencyIdOne, Arg.Any<bool>()).Returns(entity);
        _currencyRepo.AnyAsync(Arg.Any<Expression<Func<CurrencyEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var dto = MagicCurrency.ValidUpdateDto;
        var result = await _sut.UpdateCurrencyAsync(MagicIds.CurrencyIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _currencyRepo.Received(1).Update(Arg.Any<CurrencyEntity>());
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
