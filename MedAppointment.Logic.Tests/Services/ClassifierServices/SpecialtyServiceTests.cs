using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class SpecialtyServiceTests
{
    private const string SpecialtyServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.SpecialtyService";

    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<SpecialtyCreateDto> _createValidator;
    private readonly IValidator<SpecialtyUpdateDto> _updateValidator;
    private readonly IValidator<ClassifierPaginationQueryDto> _paginationValidator;
    private readonly IClassifierFilterExpressionStrategy<SpecialtyEntity, ClassifierPaginationQueryDto> _filterStrategy;
    private readonly ITranslationLookupService _translationLookup;
    private readonly ISpecialtyRepository _specialtyRepo;
    private readonly ISpecialtyService _sut;

    public SpecialtyServiceTests()
    {
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(SpecialtyServiceTypeName);
        _createValidator = Substitute.For<IValidator<SpecialtyCreateDto>>();
        _updateValidator = Substitute.For<IValidator<SpecialtyUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<ClassifierPaginationQueryDto>>();
        _filterStrategy = Substitute.For<IClassifierFilterExpressionStrategy<SpecialtyEntity, ClassifierPaginationQueryDto>>();
        _translationLookup = Substitute.For<ITranslationLookupService>();
        _specialtyRepo = Substitute.For<ISpecialtyRepository>();
        _unitOfClassifier.Specialty.Returns(_specialtyRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<ISpecialtyService>(
            SpecialtyServiceTypeName,
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
    public async Task GetSpecialtiesAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicSpecialty.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetSpecialtiesAsync(query);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task GetSpecialtiesAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicSpecialty.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<SpecialtyEntity> { MagicSpecialty.EntityOneWithLocalization };
        _specialtyRepo.FindAsync(Arg.Any<Expression<Func<SpecialtyEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<SpecialtyEntity>>(entities));

        var result = await _sut.GetSpecialtiesAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model!.TotalCount);
    }

    [Fact]
    public async Task GetSpecialtyByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _specialtyRepo.GetByIdAsync(MagicIds.SpecialtyIdNonExistent, Arg.Any<bool>()).Returns((SpecialtyEntity?)null);

        var result = await _sut.GetSpecialtyByIdAsync(MagicIds.SpecialtyIdNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task GetSpecialtyByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicSpecialty.EntityOneWithLocalization;
        _specialtyRepo.GetByIdAsync(MagicIds.SpecialtyIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetSpecialtyByIdAsync(MagicIds.SpecialtyIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.Key, result.Model.Key);
    }

    [Fact]
    public async Task CreateSpecialtyAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicSpecialty.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Key", "Required") }));

        var result = await _sut.CreateSpecialtyAsync(dto);

        Assert.False(result.IsSuccess());
        await _specialtyRepo.DidNotReceive().AddAsync(Arg.Any<SpecialtyEntity>());
    }

    [Fact]
    public async Task CreateSpecialtyAsync_WhenKeyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicSpecialty.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _specialtyRepo.AnyAsync(Arg.Any<Expression<Func<SpecialtyEntity, bool>>>()).Returns(true);

        var result = await _sut.CreateSpecialtyAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00051");
    }

    [Fact]
    public async Task CreateSpecialtyAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicSpecialty.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _specialtyRepo.AnyAsync(Arg.Any<Expression<Func<SpecialtyEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var result = await _sut.CreateSpecialtyAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _specialtyRepo.Received(1).AddAsync(Arg.Is<SpecialtyEntity>(e => e.Key == dto.Key));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateSpecialtyAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<SpecialtyUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _specialtyRepo.GetByIdAsync(MagicIds.SpecialtyIdNonExistent, Arg.Any<bool>()).Returns((SpecialtyEntity?)null);

        var result = await _sut.UpdateSpecialtyAsync(MagicIds.SpecialtyIdNonExistent, MagicSpecialty.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task UpdateSpecialtyAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicSpecialty.EntityOneWithLocalization;
        entity.Description = MagicClassifierHelper.ResourceWithTranslation("specialty_desc", MagicIds.LanguageIdOne, "desc");
        _updateValidator.ValidateAsync(Arg.Any<SpecialtyUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _specialtyRepo.GetByIdAsync(MagicIds.SpecialtyIdOne, Arg.Any<bool>()).Returns(entity);
        _specialtyRepo.AnyAsync(Arg.Any<Expression<Func<SpecialtyEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<MedAppointment.DataTransferObjects.LocalizationDtos.CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var dto = MagicSpecialty.ValidUpdateDto;
        var result = await _sut.UpdateSpecialtyAsync(MagicIds.SpecialtyIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _specialtyRepo.Received(1).Update(Arg.Any<SpecialtyEntity>());
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
