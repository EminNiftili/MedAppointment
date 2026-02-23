using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class PaymentTypeServiceTests
{
    private const string PaymentTypeServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.PaymentTypeService";

    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<PaymentTypeCreateDto> _createValidator;
    private readonly IValidator<PaymentTypeUpdateDto> _updateValidator;
    private readonly IValidator<ClassifierPaginationQueryDto> _paginationValidator;
    private readonly IClassifierFilterExpressionStrategy<PaymentTypeEntity, ClassifierPaginationQueryDto> _filterStrategy;
    private readonly ITranslationLookupService _translationLookup;
    private readonly IPaymentTypeRepository _paymentTypeRepo;
    private readonly IPaymentTypeService _sut;

    public PaymentTypeServiceTests()
    {
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(PaymentTypeServiceTypeName);
        _createValidator = Substitute.For<IValidator<PaymentTypeCreateDto>>();
        _updateValidator = Substitute.For<IValidator<PaymentTypeUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<ClassifierPaginationQueryDto>>();
        _filterStrategy = Substitute.For<IClassifierFilterExpressionStrategy<PaymentTypeEntity, ClassifierPaginationQueryDto>>();
        _translationLookup = Substitute.For<ITranslationLookupService>();
        _paymentTypeRepo = Substitute.For<IPaymentTypeRepository>();
        _unitOfClassifier.PaymentType.Returns(_paymentTypeRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<IPaymentTypeService>(
            PaymentTypeServiceTypeName,
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
    public async Task GetPaymentTypesAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicPaymentType.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetPaymentTypesAsync(query);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task GetPaymentTypesAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicPaymentType.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<PaymentTypeEntity> { MagicPaymentType.EntityOneWithLocalization };
        _paymentTypeRepo.FindAsync(Arg.Any<Expression<Func<PaymentTypeEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PaymentTypeEntity>>(entities));

        var result = await _sut.GetPaymentTypesAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model!.TotalCount);
    }

    [Fact]
    public async Task GetPaymentTypeByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _paymentTypeRepo.GetByIdAsync(MagicPaymentType.PaymentTypeIdOne + 9999, Arg.Any<bool>()).Returns((PaymentTypeEntity?)null);

        var result = await _sut.GetPaymentTypeByIdAsync(MagicPaymentType.PaymentTypeIdOne + 9999);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task GetPaymentTypeByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicPaymentType.EntityOneWithLocalization;
        _paymentTypeRepo.GetByIdAsync(MagicPaymentType.PaymentTypeIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetPaymentTypeByIdAsync(MagicPaymentType.PaymentTypeIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.Key, result.Model.Key);
    }

    [Fact]
    public async Task CreatePaymentTypeAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicPaymentType.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Key", "Required") }));

        var result = await _sut.CreatePaymentTypeAsync(dto);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task CreatePaymentTypeAsync_WhenKeyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicPaymentType.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _paymentTypeRepo.AnyAsync(Arg.Any<Expression<Func<PaymentTypeEntity, bool>>>()).Returns(true);

        var result = await _sut.CreatePaymentTypeAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
    }

    [Fact]
    public async Task CreatePaymentTypeAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicPaymentType.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _paymentTypeRepo.AnyAsync(Arg.Any<Expression<Func<PaymentTypeEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var result = await _sut.CreatePaymentTypeAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _paymentTypeRepo.Received(1).AddAsync(Arg.Is<PaymentTypeEntity>(e => e.Key == dto.Key));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdatePaymentTypeAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<PaymentTypeUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _paymentTypeRepo.GetByIdAsync(99999, Arg.Any<bool>()).Returns((PaymentTypeEntity?)null);

        var result = await _sut.UpdatePaymentTypeAsync(99999, MagicPaymentType.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task UpdatePaymentTypeAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicPaymentType.EntityOneWithLocalization;
        entity.Description = MagicClassifierHelper.ResourceWithTranslation("payment_desc", MagicIds.LanguageIdOne, "desc");
        _updateValidator.ValidateAsync(Arg.Any<PaymentTypeUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _paymentTypeRepo.GetByIdAsync(MagicPaymentType.PaymentTypeIdOne, Arg.Any<bool>()).Returns(entity);
        _paymentTypeRepo.AnyAsync(Arg.Any<Expression<Func<PaymentTypeEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var dto = MagicPaymentType.ValidUpdateDto;
        var result = await _sut.UpdatePaymentTypeAsync(MagicPaymentType.PaymentTypeIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _paymentTypeRepo.Received(1).Update(Arg.Any<PaymentTypeEntity>());
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
