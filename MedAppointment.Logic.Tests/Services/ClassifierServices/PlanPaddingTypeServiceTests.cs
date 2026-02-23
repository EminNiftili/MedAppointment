using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class PlanPaddingTypeServiceTests
{
    private const string PlanPaddingTypeServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.PlanPaddingTypeService";

    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<PlanPaddingTypeCreateDto> _createValidator;
    private readonly IValidator<PlanPaddingTypeUpdateDto> _updateValidator;
    private readonly IValidator<PlanPaddingTypePaginationQueryDto> _paginationValidator;
    private readonly IClassifierFilterExpressionStrategy<PlanPaddingTypeEntity, PlanPaddingTypePaginationQueryDto> _filterStrategy;
    private readonly ITranslationLookupService _translationLookup;
    private readonly IPlanPaddingTypeRepository _planPaddingTypeRepo;
    private readonly IPlanPaddingTypeService _sut;

    public PlanPaddingTypeServiceTests()
    {
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(PlanPaddingTypeServiceTypeName);
        _createValidator = Substitute.For<IValidator<PlanPaddingTypeCreateDto>>();
        _updateValidator = Substitute.For<IValidator<PlanPaddingTypeUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<PlanPaddingTypePaginationQueryDto>>();
        _filterStrategy = Substitute.For<IClassifierFilterExpressionStrategy<PlanPaddingTypeEntity, PlanPaddingTypePaginationQueryDto>>();
        _translationLookup = Substitute.For<ITranslationLookupService>();
        _planPaddingTypeRepo = Substitute.For<IPlanPaddingTypeRepository>();
        _unitOfClassifier.PlanPaddingType.Returns(_planPaddingTypeRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<IPlanPaddingTypeService>(
            PlanPaddingTypeServiceTypeName,
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
    public async Task GetPlanPaddingTypesAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicPlanPaddingType.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetPlanPaddingTypesAsync(query);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task GetPlanPaddingTypesAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicPlanPaddingType.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<PlanPaddingTypeEntity> { MagicPlanPaddingType.EntityOneWithLocalization };
        _planPaddingTypeRepo.FindAsync(Arg.Any<Expression<Func<PlanPaddingTypeEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<PlanPaddingTypeEntity>>(entities));

        var result = await _sut.GetPlanPaddingTypesAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model!.TotalCount);
    }

    [Fact]
    public async Task GetPlanPaddingTypeByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _planPaddingTypeRepo.GetByIdAsync(99999, Arg.Any<bool>()).Returns((PlanPaddingTypeEntity?)null);

        var result = await _sut.GetPlanPaddingTypeByIdAsync(99999);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task GetPlanPaddingTypeByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicPlanPaddingType.EntityOneWithLocalization;
        _planPaddingTypeRepo.GetByIdAsync(MagicPlanPaddingType.PlanPaddingTypeIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetPlanPaddingTypeByIdAsync(MagicPlanPaddingType.PlanPaddingTypeIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.PaddingTime, result.Model.PaddingTime);
    }

    [Fact]
    public async Task CreatePlanPaddingTypeAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicPlanPaddingType.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Key", "Required") }));

        var result = await _sut.CreatePlanPaddingTypeAsync(dto);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task CreatePlanPaddingTypeAsync_WhenKeyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicPlanPaddingType.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _planPaddingTypeRepo.AnyAsync(Arg.Any<Expression<Func<PlanPaddingTypeEntity, bool>>>()).Returns(true);

        var result = await _sut.CreatePlanPaddingTypeAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
    }

    [Fact]
    public async Task CreatePlanPaddingTypeAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicPlanPaddingType.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _planPaddingTypeRepo.AnyAsync(Arg.Any<Expression<Func<PlanPaddingTypeEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var result = await _sut.CreatePlanPaddingTypeAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _planPaddingTypeRepo.Received(1).AddAsync(Arg.Is<PlanPaddingTypeEntity>(e => e.Key == dto.Key && e.PaddingTime == dto.PaddingTime));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdatePlanPaddingTypeAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<PlanPaddingTypeUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _planPaddingTypeRepo.GetByIdAsync(99999, Arg.Any<bool>()).Returns((PlanPaddingTypeEntity?)null);

        var result = await _sut.UpdatePlanPaddingTypeAsync(99999, MagicPlanPaddingType.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task UpdatePlanPaddingTypeAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicPlanPaddingType.EntityOneWithLocalization;
        entity.Description = MagicClassifierHelper.ResourceWithTranslation("padding_desc", MagicIds.LanguageIdOne, "desc");
        _updateValidator.ValidateAsync(Arg.Any<PlanPaddingTypeUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _planPaddingTypeRepo.GetByIdAsync(MagicPlanPaddingType.PlanPaddingTypeIdOne, Arg.Any<bool>()).Returns(entity);
        _planPaddingTypeRepo.AnyAsync(Arg.Any<Expression<Func<PlanPaddingTypeEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var dto = MagicPlanPaddingType.ValidUpdateDto;
        var result = await _sut.UpdatePlanPaddingTypeAsync(MagicPlanPaddingType.PlanPaddingTypeIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _planPaddingTypeRepo.Received(1).Update(Arg.Any<PlanPaddingTypeEntity>());
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
