namespace MedAppointment.Logic.Tests.Services.LocalizationServices;

public class TranslationLookupServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.LocalizationServices.TranslationLookupService";

    private readonly IUnitOfLocalization _unitOfLocalization;
    private readonly ILogger _logger;
    private readonly ITranslationRepository _translationRepo;
    private readonly ITranslationLookupService _sut;

    public TranslationLookupServiceTests()
    {
        _unitOfLocalization = Substitute.For<IUnitOfLocalization>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _translationRepo = Substitute.For<ITranslationRepository>();

        _unitOfLocalization.Translation.Returns(_translationRepo);

        _sut = ServiceReflectionHelper.CreateService<ITranslationLookupService>(ServiceTypeName,
            _unitOfLocalization,
            _logger);
    }

    [Fact]
    public async Task GetFilterIdsAsync_WhenBothFiltersNull_ReturnsEmptyLists()
    {
        var (nameIds, descIds) = await _sut.GetFilterIdsAsync(null, null);

        Assert.NotNull(nameIds);
        Assert.NotNull(descIds);
        Assert.Empty(nameIds);
        Assert.Empty(descIds);
        await _translationRepo.DidNotReceive().FindAsync(Arg.Any<Expression<Func<TranslationEntity, bool>>>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task GetFilterIdsAsync_WhenNameFilterSet_ReturnsNameResourceIds()
    {
        var translations = new List<TranslationEntity>
        {
            new() { Id = 1, ResourceId = 10, LanguageId = 1, Text = "Cardiology" },
            new() { Id = 2, ResourceId = 11, LanguageId = 2, Text = "Cardiology" }
        };
        _translationRepo.FindAsync(Arg.Any<Expression<Func<TranslationEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<TranslationEntity>>(translations));

        var (nameIds, descIds) = await _sut.GetFilterIdsAsync("Cardio", null);

        Assert.NotNull(nameIds);
        Assert.Equal(2, nameIds.Count);
        Assert.Contains(10, nameIds);
        Assert.Contains(11, nameIds);
        Assert.NotNull(descIds);
        Assert.Empty(descIds);
    }

    [Fact]
    public async Task GetFilterIdsAsync_WhenDescriptionFilterSet_ReturnsDescriptionResourceIds()
    {
        var translations = new List<TranslationEntity>
        {
            new() { Id = 1, ResourceId = 20, LanguageId = 1, Text = "Heart specialist" }
        };
        _translationRepo.FindAsync(Arg.Any<Expression<Func<TranslationEntity, bool>>>(), Arg.Any<bool>())
            .Returns(Task.FromResult<IEnumerable<TranslationEntity>>(translations));

        var (nameIds, descIds) = await _sut.GetFilterIdsAsync(null, "heart");

        Assert.NotNull(nameIds);
        Assert.Empty(nameIds);
        Assert.NotNull(descIds);
        Assert.Single(descIds);
        Assert.Equal(20, descIds[0]);
    }
}
