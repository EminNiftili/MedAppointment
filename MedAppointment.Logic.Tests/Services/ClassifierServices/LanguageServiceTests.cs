using MedAppointment.Logic.Tests.TestHelpers;

namespace MedAppointment.Logic.Tests.Services.ClassifierServices;

public class LanguageServiceTests
{
    private const string LanguageServiceTypeName = "MedAppointment.Logics.Implementations.ClassifierServices.LanguageService";

    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly ILogger _logger;
    private readonly IValidator<LanguageCreateDto> _createValidator;
    private readonly IValidator<LanguageUpdateDto> _updateValidator;
    private readonly IValidator<LanguagePaginationQueryDto> _paginationValidator;
    private readonly ILanguageRepository _languageRepo;
    private readonly ILanguageService _sut;

    public LanguageServiceTests()
    {
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(LanguageServiceTypeName);
        _createValidator = Substitute.For<IValidator<LanguageCreateDto>>();
        _updateValidator = Substitute.For<IValidator<LanguageUpdateDto>>();
        _paginationValidator = Substitute.For<IValidator<LanguagePaginationQueryDto>>();
        _languageRepo = Substitute.For<ILanguageRepository>();

        _unitOfClassifier.Language.Returns(_languageRepo);

        _sut = ServiceReflectionHelper.CreateClassifierService<ILanguageService>(
            LanguageServiceTypeName,
            _unitOfClassifier,
            _logger,
            _createValidator,
            _updateValidator,
            _paginationValidator);
    }

    [Fact]
    public async Task GetLanguagesAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicLanguage.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetLanguagesAsync(query);

        Assert.False(result.IsSuccess());
        await _languageRepo.DidNotReceive().FindAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task GetLanguagesAsync_WhenValidationSucceeds_ReturnsPagedResult()
    {
        var query = MagicLanguage.ValidPaginationQuery;
        _paginationValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        var entities = new List<LanguageEntity> { MagicLanguage.EntityOne, MagicLanguage.EntityTwo };
        _languageRepo.FindAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<LanguageEntity>>(entities));

        var result = await _sut.GetLanguagesAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(2, result.Model!.TotalCount);
        Assert.Equal(2, result.Model.Items.Count);
    }

    [Fact]
    public async Task GetLanguageByIdAsync_WhenNotFound_ReturnsNotFound()
    {
        _languageRepo.GetByIdAsync(MagicIds.LanguageIdNonExistent, Arg.Any<bool>()).Returns((LanguageEntity?)null);

        var result = await _sut.GetLanguageByIdAsync(MagicIds.LanguageIdNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task GetLanguageByIdAsync_WhenFound_ReturnsMappedDto()
    {
        var entity = MagicLanguage.EntityOne;
        _languageRepo.GetByIdAsync(MagicIds.LanguageIdOne, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.GetLanguageByIdAsync(MagicIds.LanguageIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(entity.Id, result.Model!.Id);
        Assert.Equal(entity.Name, result.Model.Name);
        Assert.Equal(entity.IsDefault, result.Model.IsDefault);
    }

    [Fact]
    public async Task CreateLanguageAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicLanguage.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Required") }));

        var result = await _sut.CreateLanguageAsync(dto);

        Assert.False(result.IsSuccess());
        await _languageRepo.DidNotReceive().AddAsync(Arg.Any<LanguageEntity>());
    }

    [Fact]
    public async Task CreateLanguageAsync_WhenNameAlreadyExists_ReturnsConflict()
    {
        var dto = MagicLanguage.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _languageRepo.AnyAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>()).Returns(true);

        var result = await _sut.CreateLanguageAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00051");
    }

    [Fact]
    public async Task CreateLanguageAsync_WhenIsDefaultAndDefaultAlreadyExists_ReturnsConflict()
    {
        var dto = MagicLanguage.ValidCreateDto with { IsDefault = true };
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _languageRepo.AnyAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>()).Returns(false, true);

        var result = await _sut.CreateLanguageAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00146");
    }

    [Fact]
    public async Task CreateLanguageAsync_WhenValid_CreatesAndReturnsNoContent()
    {
        var dto = MagicLanguage.ValidCreateDto;
        _createValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _languageRepo.AnyAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>()).Returns(false);

        var result = await _sut.CreateLanguageAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _languageRepo.Received(1).AddAsync(Arg.Is<LanguageEntity>(e => e.Name == dto.Name && e.IsDefault == dto.IsDefault));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateLanguageAsync_WhenEntityNotFound_ReturnsNotFound()
    {
        _updateValidator.ValidateAsync(Arg.Any<LanguageUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _languageRepo.GetByIdAsync(MagicIds.LanguageIdNonExistent, Arg.Any<bool>()).Returns((LanguageEntity?)null);

        var result = await _sut.UpdateLanguageAsync(MagicIds.LanguageIdNonExistent, MagicLanguage.ValidUpdateDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task UpdateLanguageAsync_WhenValid_UpdatesAndReturnsNoContent()
    {
        var entity = MagicLanguage.EntityTwo;
        _updateValidator.ValidateAsync(Arg.Any<LanguageUpdateDto>(), Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _languageRepo.GetByIdAsync(entity.Id, Arg.Any<bool>()).Returns(entity);
        _languageRepo.AnyAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>()).Returns(false);

        var dto = MagicLanguage.ValidUpdateDto;
        var result = await _sut.UpdateLanguageAsync(entity.Id, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _languageRepo.Received(1).Update(Arg.Is<LanguageEntity>(e => e.Id == entity.Id && e.Name == dto.Name && e.IsDefault == dto.IsDefault));
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteLanguageAsync_WhenNotFound_ReturnsNotFound()
    {
        _languageRepo.GetByIdAsync(MagicIds.LanguageIdNonExistent, Arg.Any<bool>()).Returns((LanguageEntity?)null);

        var result = await _sut.DeleteLanguageAsync(MagicIds.LanguageIdNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task DeleteLanguageAsync_WhenIsDefault_ReturnsConflict()
    {
        var entity = MagicLanguage.EntityOne;
        _languageRepo.GetByIdAsync(entity.Id, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.DeleteLanguageAsync(entity.Id);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00144");
    }

    [Fact]
    public async Task DeleteLanguageAsync_WhenValid_RemovesAndReturnsNoContent()
    {
        var entity = MagicLanguage.EntityTwo;
        _languageRepo.GetByIdAsync(entity.Id, Arg.Any<bool>()).Returns(entity);

        var result = await _sut.DeleteLanguageAsync(entity.Id);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _languageRepo.Received(1).RemoveAsync(entity.Id);
        await _unitOfClassifier.Received(1).SaveChangesAsync();
    }
}
