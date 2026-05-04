using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class ProfessionServiceTests
{
    private const string ProfessionServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.ProfessionService";

    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<ProfessionCreateDto> _createValidator;
    private readonly IValidator<ProfessionUpdateDto> _updateValidator;
    private readonly IValidator<ClassifierPaginationQueryDto> _paginationValidator;
    private readonly IClassifierFilterExpressionStrategy<ProfessionEntity, ClassifierPaginationQueryDto> _filterStrategy;
    private readonly ITranslationLookupService _translationLookup;
    private readonly IProfessionRepository _professionRepo;
    private readonly IProfessionService _sut;

    public ProfessionServiceTests()
    {
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(ProfessionServiceTypeName);
        _createValidator = Substitute.For<IValidator<ProfessionCreateDto>>();
        _updateValidator = Substitute.For<IValidator<ProfessionUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<ClassifierPaginationQueryDto>>();
        _filterStrategy = Substitute.For<IClassifierFilterExpressionStrategy<ProfessionEntity, ClassifierPaginationQueryDto>>();
        _translationLookup = Substitute.For<ITranslationLookupService>();
        _professionRepo = Substitute.For<IProfessionRepository>();
        _unitOfClassifier.Profession.Returns(_professionRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<IProfessionService>(
            ProfessionServiceTypeName,
            _localizerService,
            _unitOfClassifier,
            _logger,
            _createValidator,
            _updateValidator,
            _paginationValidator,
            _filterStrategy,
            _translationLookup);
    }

    // --- GetProfessionsAsync ---

    [Fact]
    public async Task GetProfessionsAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicProfession.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("PageSize", "Invalid")
            }));

        var result = await _sut.GetProfessionsAsync(query);

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public async Task GetProfessionsAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicProfession.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<ProfessionEntity> { MagicProfession.EntityOneWithLocalization };
        _professionRepo.FindAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<ProfessionEntity>>(entities));

        var result = await _sut.GetProfessionsAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(1, result.Model!.TotalCount);
        Assert.Single(result.Model.Items);
    }

    [Fact]
    public async Task GetProfessionsAsync_WhenNameFilterProvided_CallsTranslationLookup()
    {
        var query = new ClassifierPaginationQueryDto
        {
            PageNumber = 1,
            PageSize = 10,
            NameFilter = "doctor",
            DescriptionFilter = null
        };
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _translationLookup.GetFilterIdsAsync(query.NameFilter, query.DescriptionFilter)
            .Returns(((IReadOnlyList<long>?)new List<long> { 1L }, (IReadOnlyList<long>?)new List<long>()));
        _filterStrategy.Build(query, Arg.Any<IReadOnlyList<long>?>(), Arg.Any<IReadOnlyList<long>?>())
            .Returns(_ => true);
        _professionRepo.FindAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<ProfessionEntity>>(new List<ProfessionEntity>()));

        var result = await _sut.GetProfessionsAsync(query);

        Assert.True(result.IsSuccess());
        await _translationLookup.Received(1).GetFilterIdsAsync(query.NameFilter, query.DescriptionFilter);
    }

    [Fact]
    public async Task GetProfessionsAsync_WhenMultiplePagesExist_ReturnsPaginatedItems()
    {
        var query = new ClassifierPaginationQueryDto { PageNumber = 2, PageSize = 1 };
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _filterStrategy.Build(query).Returns(_ => true);
        var entities = new List<ProfessionEntity>
        {
            MagicProfession.EntityOneWithLocalization,
            new ProfessionEntity
            {
                Id = MagicIds.ProfessionIdOne + 1,
                Key = "NURSE",
                NameTextId = MagicIds.NameTextId,
                DescriptionTextId = MagicIds.DescriptionTextId,
                Name = MagicClassifierHelper.ResourceWithTranslation("profession_name", MagicIds.LanguageIdOne, "Nurse"),
                Description = MagicClassifierHelper.ResourceWithTranslation("profession_desc", MagicIds.LanguageIdOne, "Nursing specialist")
            }
        };
        _professionRepo.FindAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<ProfessionEntity>>(entities));

        var result = await _sut.GetProfessionsAsync(query);

        Assert.True(result.IsSuccess());
        Assert.Equal(2, result.Model!.TotalCount);
        Assert.Equal(2, result.Model.TotalPages);
        Assert.Single(result.Model.Items);
    }

    // --- GetProfessionByIdAsync ---

    [Fact]
    public async Task GetProfessionByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _professionRepo.GetByIdAsync(MagicIds.ProfessionIdNonExistent, Arg.Any<bool>())
            .Returns((ProfessionEntity?)null);

        var result = await _sut.GetProfessionByIdAsync(MagicIds.ProfessionIdNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task GetProfessionByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicProfession.EntityOneWithLocalization;
        _professionRepo.GetByIdAsync(MagicIds.ProfessionIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetProfessionByIdAsync(MagicIds.ProfessionIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.Key, result.Model.Key);
        Assert.NotEmpty(result.Model.Name);
        Assert.NotEmpty(result.Model.Description);
    }

    // --- CreateProfessionAsync ---

    [Fact]
    public async Task CreateProfessionAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicProfession.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("Key", "Required")
            }));

        var result = await _sut.CreateProfessionAsync(dto);

        Assert.False(result.IsSuccess());
        await _professionRepo.DidNotReceive().AddAsync(Arg.Any<ProfessionEntity>());
    }

    [Fact]
    public async Task CreateProfessionAsync_WhenKeyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicProfession.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _professionRepo.AnyAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>()).Returns(true);

        var result = await _sut.CreateProfessionAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00051");
        await _professionRepo.DidNotReceive().AddAsync(Arg.Any<ProfessionEntity>());
    }

    [Fact]
    public async Task CreateProfessionAsync_WhenLocalizerFails_ReturnsFailure()
    {
        var dto = MagicProfession.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _professionRepo.AnyAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>()).Returns(false);

        var failedResult = Result<long>.Create();
        failedResult.AddMessage("ERR00100", "Localizer failed.", HttpStatusCode.BadRequest);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<CreateLocalizationDto>>())
            .Returns(failedResult);

        var result = await _sut.CreateProfessionAsync(dto);

        Assert.False(result.IsSuccess());
        await _professionRepo.DidNotReceive().AddAsync(Arg.Any<ProfessionEntity>());
    }

    [Fact]
    public async Task CreateProfessionAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicProfession.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _professionRepo.AnyAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var result = await _sut.CreateProfessionAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _professionRepo.Received(1).AddAsync(Arg.Is<ProfessionEntity>(e => e.Key == dto.Key));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    // --- UpdateProfessionAsync ---

    [Fact]
    public async Task UpdateProfessionAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicProfession.ValidUpdateDto;
        _updateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[]
            {
                new FluentValidation.Results.ValidationFailure("Key", "Required")
            }));

        var result = await _sut.UpdateProfessionAsync(MagicIds.ProfessionIdOne, dto);

        Assert.False(result.IsSuccess());
        await _professionRepo.DidNotReceive().GetByIdAsync(Arg.Any<long>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task UpdateProfessionAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<ProfessionUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _professionRepo.GetByIdAsync(MagicIds.ProfessionIdNonExistent, Arg.Any<bool>())
            .Returns((ProfessionEntity?)null);

        var result = await _sut.UpdateProfessionAsync(MagicIds.ProfessionIdNonExistent, MagicProfession.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task UpdateProfessionAsync_WhenKeyConflictsWithAnother_ReturnsConflict()
    {
        var entity = MagicProfession.EntityOneWithLocalization;
        entity.Description = MagicClassifierHelper.ResourceWithTranslation("profession_desc", MagicIds.LanguageIdOne, "desc");
        _updateValidator.ValidateAsync(Arg.Any<ProfessionUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _professionRepo.GetByIdAsync(MagicIds.ProfessionIdOne, Arg.Any<bool>()).Returns(entity);
        _professionRepo.AnyAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>()).Returns(true);

        var result = await _sut.UpdateProfessionAsync(MagicIds.ProfessionIdOne, MagicProfession.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00051");
        _professionRepo.DidNotReceive().Update(Arg.Any<ProfessionEntity>());
    }

    [Fact]
    public async Task UpdateProfessionAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicProfession.EntityOneWithLocalization;
        entity.Description = MagicClassifierHelper.ResourceWithTranslation("profession_desc", MagicIds.LanguageIdOne, "desc");
        _updateValidator.ValidateAsync(Arg.Any<ProfessionUpdateDto>(), Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        _professionRepo.GetByIdAsync(MagicIds.ProfessionIdOne, Arg.Any<bool>()).Returns(entity);
        _professionRepo.AnyAsync(Arg.Any<Expression<Func<ProfessionEntity, bool>>>()).Returns(false);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<CreateLocalizationDto>>())
            .Returns(Result<long>.Create(MagicIds.NameTextId));

        var dto = MagicProfession.ValidUpdateDto;
        var result = await _sut.UpdateProfessionAsync(MagicIds.ProfessionIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _professionRepo.Received(1).Update(Arg.Any<ProfessionEntity>());
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
