namespace MedAppointment.Logic.Tests.Services.SecurityServices;

public class JwtBearerTokenServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.SecurityServices.JwtBearerTokenService";

    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _sut;

    public JwtBearerTokenServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _configuration = Substitute.For<IConfiguration>();
        SetValidJwtConfig(_configuration);
        _sut = ServiceReflectionHelper.CreateService<ITokenService>(ServiceTypeName, _logger, _configuration);
    }

    private static void SetValidJwtConfig(IConfiguration config)
    {
        config["Jwt:Issuer"].Returns("TestIssuer");
        config["Jwt:Audience"].Returns("TestAudience");
        config["Jwt:SigningKey"].Returns("ThisIsAValidSigningKeyWithAtLeast32Characters!");
        config["Jwt:AccessTokenMinutes"].Returns("60");
        config["Jwt:RefreshTokenBytes"].Returns("32");
    }

    [Fact]
    public void GetToken_WithValidConfig_ReturnsNonEmptyTokenAndSetsExpiredDate()
    {
        var token = _sut.GetToken(out var expiredDate, null);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.True(expiredDate > DateTime.UtcNow);
        Assert.True(expiredDate <= DateTime.UtcNow.AddMinutes(61));
    }

    [Fact]
    public void GetToken_WithClaims_ReturnsToken()
    {
        var claims = new Dictionary<string, object> { { ClaimTypes.Role, new[] { "User", "Doctor" } } };

        var token = _sut.GetToken(out var expiredDate, claims);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.True(expiredDate > DateTime.UtcNow);
    }

    [Fact]
    public void GetToken_WhenIssuerMissing_ThrowsInvalidOperationException()
    {
        _configuration["Jwt:Issuer"].Returns((string?)null);

        var ex = Assert.Throws<InvalidOperationException>(() => _sut.GetToken(out _, null));
        Assert.Contains("Issuer", ex.Message);
    }

    [Fact]
    public void GetToken_WhenAccessTokenMinutesInvalid_ThrowsArgumentException()
    {
        _configuration["Jwt:AccessTokenMinutes"].Returns("invalid");

        var ex = Assert.Throws<ArgumentException>(() => _sut.GetToken(out _, null));
        Assert.Contains("AccessTokenMinutes", ex.Message);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsNonEmptyBase64String()
    {
        var token = _sut.GenerateRefreshToken();

        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.True(Convert.FromBase64String(token).Length >= 32);
    }

    [Fact]
    public void GenerateRefreshToken_EachCallReturnsDifferentValue()
    {
        var token1 = _sut.GenerateRefreshToken();
        var token2 = _sut.GenerateRefreshToken();

        Assert.NotEqual(token1, token2);
    }
}
