namespace MedAppointment.Logic.Tests.Services.SecurityServices;

public class HashServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.SecurityServices.HashService";

    private readonly ILogger _logger;
    private readonly IHashService _sut;

    public HashServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _sut = ServiceReflectionHelper.CreateService<IHashService>(ServiceTypeName, _logger);
    }

    [Fact]
    public void HashText_WhenKeyIsNull_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.HashText(null!, "salt"));
        Assert.Equal("key", ex.ParamName?.ToLowerInvariant());
    }

    [Fact]
    public void HashText_WhenKeyIsEmpty_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.HashText("", "salt"));
        Assert.Equal("key", ex.ParamName?.ToLowerInvariant());
    }

    [Fact]
    public void HashText_WhenSaltIsNull_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.HashText("key", null!));
        Assert.Equal("salt", ex.ParamName?.ToLowerInvariant());
    }

    [Fact]
    public void HashText_WhenSaltIsEmpty_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _sut.HashText("key", ""));
        Assert.Equal("salt", ex.ParamName?.ToLowerInvariant());
    }

    [Fact]
    public void HashText_WhenValid_ReturnsBase64String()
    {
        var hash = _sut.HashText("password", "email@test.com");

        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.True(Convert.FromBase64String(hash).Length > 0);
    }

    [Fact]
    public void HashText_WhenSameInput_ReturnsSameHash()
    {
        var hash1 = _sut.HashText("secret", "salt");
        var hash2 = _sut.HashText("secret", "salt");

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void HashText_WhenDifferentSalt_ReturnsDifferentHash()
    {
        var hash1 = _sut.HashText("secret", "salt1");
        var hash2 = _sut.HashText("secret", "salt2");

        Assert.NotEqual(hash1, hash2);
    }
}
