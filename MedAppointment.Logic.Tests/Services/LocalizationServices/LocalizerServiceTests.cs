namespace MedAppointment.Logic.Tests.Services.LocalizationServices;

public class LocalizerServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.LocalizationServices.LocalizerService";

    private readonly ILogger _logger;
    private readonly IUnitOfLocalization _unitOfLocalization;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly IResourceRepository _resourceRepo;
    private readonly ILanguageRepository _languageRepo;
    private readonly ILocalizerService _sut;

    public LocalizerServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _unitOfLocalization = Substitute.For<IUnitOfLocalization>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _resourceRepo = Substitute.For<IResourceRepository>();
        _languageRepo = Substitute.For<ILanguageRepository>();

        _unitOfLocalization.Resource.Returns(_resourceRepo);
        _unitOfClassifier.Language.Returns(_languageRepo);

        _sut = ServiceReflectionHelper.CreateService<ILocalizerService>(ServiceTypeName,
            _logger,
            _unitOfLocalization,
            _unitOfClassifier);
    }

    [Fact]
    public async Task AddResourceAsync_WhenDuplicateLanguageId_DoesNotCallResourceAddOrUpdate()
    {
        var localizations = new List<CreateLocalizationDto>
        {
            new() { LanguageId = MagicIds.LanguageIdOne, Text = "A" },
            new() { LanguageId = MagicIds.LanguageIdOne, Text = "B" }
        };

        var result = await _sut.AddResourceAsync("test_key", localizations);

        await _resourceRepo.DidNotReceive().AddAsync(Arg.Any<ResourceEntity>());
        _resourceRepo.DidNotReceive().Update(Arg.Any<ResourceEntity>());
        await _unitOfLocalization.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task AddResourceAsync_WhenValidNewResource_CallsAddAndSaveChanges()
    {
        var localizations = new List<CreateLocalizationDto>
        {
            new() { LanguageId = MagicIds.LanguageIdOne, Text = "Hello" }
        };
        _resourceRepo.GetByKeyAsync(Arg.Any<string>()).Returns((ResourceEntity?)null);
        _languageRepo.AnyAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>()).Returns(true);

        var result = await _sut.AddResourceAsync("new_key", localizations);

        await _resourceRepo.Received(1).AddAsync(Arg.Is<ResourceEntity>(r => r.Key.StartsWith("new_key")));
        await _unitOfLocalization.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task AddResourceAsync_WhenLanguageNotFound_DoesNotSave()
    {
        var localizations = new List<CreateLocalizationDto>
        {
            new() { LanguageId = MagicIds.LanguageIdNonExistent, Text = "Hello" }
        };
        _resourceRepo.GetByKeyAsync(Arg.Any<string>()).Returns((ResourceEntity?)null);
        _languageRepo.AnyAsync(Arg.Any<Expression<Func<LanguageEntity, bool>>>()).Returns(false);

        var result = await _sut.AddResourceAsync("key", localizations);

        await _unitOfLocalization.DidNotReceive().SaveChangesAsync();
    }
}
