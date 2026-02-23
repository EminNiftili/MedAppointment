namespace MedAppointment.Logic.Tests.Services.SecurityServices;

public class LoginServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.SecurityServices.LoginService";

    private readonly ILogger _logger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IUnitOfService _unitOfService;
    private readonly IUnitOfSecurity _unitOfSecurity;
    private readonly IHashService _hashService;
    private readonly IValidator<TraditionalUserLoginDto> _traditionalUserLoginValidator;
    private readonly IValidator<RefreshTokenRequestDto> _refreshTokenRequestValidator;
    private readonly ITokenService _tokenService;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IPersonRepository _personRepo;
    private readonly ISessionRepository _sessionRepo;
    private readonly ITokenRepository _tokenRepo;
    private readonly ILoginService _sut;

    public LoginServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _unitOfService = Substitute.For<IUnitOfService>();
        _unitOfSecurity = Substitute.For<IUnitOfSecurity>();
        _hashService = Substitute.For<IHashService>();
        _traditionalUserLoginValidator = Substitute.For<IValidator<TraditionalUserLoginDto>>();
        _refreshTokenRequestValidator = Substitute.For<IValidator<RefreshTokenRequestDto>>();
        _tokenService = Substitute.For<ITokenService>();
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _personRepo = Substitute.For<IPersonRepository>();
        _sessionRepo = Substitute.For<ISessionRepository>();
        _tokenRepo = Substitute.For<ITokenRepository>();

        _unitOfClient.Person.Returns(_personRepo);
        _unitOfSecurity.Session.Returns(_sessionRepo);
        _unitOfSecurity.Token.Returns(_tokenRepo);

        _sut = ServiceReflectionHelper.CreateService<ILoginService>(ServiceTypeName,
            _logger,
            _unitOfClient,
            _unitOfService,
            _hashService,
            _traditionalUserLoginValidator,
            _refreshTokenRequestValidator,
            _tokenService,
            _privateClientInfoService,
            _unitOfSecurity);
    }

    [Fact]
    public async Task TraditionalLoginAsync_WhenValidationFails_ReturnsBadRequest()
    {
        var dto = MagicSecurity.ValidTraditionalUserLogin;
        _traditionalUserLoginValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Username", "Required") }));

        var result = await _sut.TraditionalLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        await _personRepo.DidNotReceive().FindByUsernameAsync(Arg.Any<string>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task TraditionalLoginAsync_WhenValidationResultIsNull_ReturnsBadRequest()
    {
        var dto = MagicSecurity.ValidTraditionalUserLogin;
        _traditionalUserLoginValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns((FluentValidation.Results.ValidationResult?)null!);

        var result = await _sut.TraditionalLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00100");
    }

    [Fact]
    public async Task TraditionalLoginAsync_WhenUserNotFound_ReturnsNotFound()
    {
        var dto = MagicSecurity.ValidTraditionalUserLogin;
        _traditionalUserLoginValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Username!, true).Returns((PersonEntity?)null);

        var result = await _sut.TraditionalLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task TraditionalLoginAsync_WhenUserHasNoTraditionalUser_ReturnsConflict()
    {
        var dto = MagicSecurity.ValidTraditionalUserLogin;
        var personNoTraditional = new PersonEntity
        {
            Id = 1,
            Email = dto.Username,
            User = new UserEntity { Id = 1, TraditionalUser = null }
        };
        _traditionalUserLoginValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Username!, true).Returns(personNoTraditional);

        var result = await _sut.TraditionalLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00026");
    }

    [Fact]
    public async Task TraditionalLoginAsync_WhenPasswordIncorrect_ReturnsUnauthorized()
    {
        var dto = MagicSecurity.ValidTraditionalUserLogin;
        var person = MagicSecurity.PersonWithTraditionalUser;
        person.User!.TraditionalUser!.PasswordHash = "correct-hash";
        _traditionalUserLoginValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Username!, true).Returns(person);
        _hashService.HashText(dto.Password!, person.Email).Returns("wrong-hash");

        var result = await _sut.TraditionalLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00025");
    }

    [Fact]
    public async Task TraditionalLoginAsync_WhenValid_ReturnsTokensAndSavesSession()
    {
        var dto = MagicSecurity.ValidTraditionalUserLogin;
        var person = MagicSecurity.PersonWithTraditionalUser;
        var hashedPwd = "hashed-password";
        person.User!.TraditionalUser!.PasswordHash = hashedPwd;
        _traditionalUserLoginValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Username!, true).Returns(person);
        _hashService.HashText(dto.Password!, person.Email).Returns(hashedPwd);
        _privateClientInfoService.GetUserTypesAsync(person.User.Id).Returns(new[] { UserType.User });
        _tokenService.GetToken(out Arg.Any<DateTime>(), Arg.Any<IDictionary<string, object>>())
            .Returns(callInfo =>
            {
                callInfo[0] = DateTime.UtcNow.AddMinutes(60);
                return "access-token";
            });
        _tokenService.GenerateRefreshToken().Returns("refresh-token");

        var result = await _sut.TraditionalLoginAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal("access-token", result.Model.AccessToken);
        Assert.Equal("refresh-token", result.Model.RefreshToken);
        _sessionRepo.Received(1).Add(Arg.Any<SessionEntity>());
        await _unitOfSecurity.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenValidationFails_ReturnsBadRequest()
    {
        var dto = MagicSecurity.ValidRefreshTokenRequest;
        _refreshTokenRequestValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("RefreshToken", "Required") }));

        var result = await _sut.RefreshTokenAsync(dto);

        Assert.False(result.IsSuccess());
        await _tokenRepo.DidNotReceive().FindFirstAsync(Arg.Any<Expression<Func<TokenEntity, bool>>>(), Arg.Any<bool>());
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenNotFound_ReturnsUnauthorized()
    {
        var dto = MagicSecurity.ValidRefreshTokenRequest;
        _refreshTokenRequestValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _tokenRepo.FindFirstAsync(Arg.Any<Expression<Func<TokenEntity, bool>>>(), true).Returns((TokenEntity?)null);

        var result = await _sut.RefreshTokenAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00054");
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenExpired_ReturnsUnauthorized()
    {
        var dto = MagicSecurity.ValidRefreshTokenRequest;
        var oldToken = new TokenEntity { Id = 1, RefreshToken = dto.RefreshToken, IsExpired = true, Session = new SessionEntity { UserId = MagicClient.UserIdOne } };
        _refreshTokenRequestValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _tokenRepo.FindFirstAsync(Arg.Any<Expression<Func<TokenEntity, bool>>>(), true).Returns(oldToken);

        var result = await _sut.RefreshTokenAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Unauthorized, result.HttpStatus);
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenValid_ReturnsNewTokensAndUpdatesSession()
    {
        var dto = MagicSecurity.ValidRefreshTokenRequest;
        var oldToken = new TokenEntity
        {
            Id = 1,
            RefreshToken = dto.RefreshToken,
            IsExpired = false,
            SessionId = 10,
            Session = new SessionEntity { Id = 10, UserId = MagicClient.UserIdOne }
        };
        _refreshTokenRequestValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _tokenRepo.FindFirstAsync(Arg.Any<Expression<Func<TokenEntity, bool>>>(), true).Returns(oldToken);
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(new[] { UserType.User });
        _tokenService.GetToken(out Arg.Any<DateTime>(), Arg.Any<IDictionary<string, object>>())
            .Returns(callInfo =>
            {
                callInfo[0] = DateTime.UtcNow.AddMinutes(60);
                return "new-access-token";
            });
        _tokenService.GenerateRefreshToken().Returns("new-refresh-token");

        var result = await _sut.RefreshTokenAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.Equal("new-access-token", result.Model!.AccessToken);
        Assert.Equal("new-refresh-token", result.Model.RefreshToken);
        Assert.True(oldToken.IsExpired);
        _tokenRepo.Received(1).Update(oldToken);
        _tokenRepo.Received(1).Add(Arg.Any<TokenEntity>());
        await _unitOfSecurity.Received(1).SaveChangesAsync();
    }
}
