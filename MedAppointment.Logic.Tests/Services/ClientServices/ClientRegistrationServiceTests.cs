namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class ClientRegistrationServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.ClientRegistrationService";

    private readonly IUnitOfClient _unitOfClient;
    private readonly IValidator<TraditionalUserRegisterDto> _traditionalUserRegisterValidator;
    private readonly ILogger _logger;
    private readonly IHashService _hasher;
    private readonly ITokenService _tokenService;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IUnitOfSecurity _unitOfSecurity;
    private readonly IPersonRepository _personRepo;
    private readonly ISessionRepository _sessionRepo;
    private readonly IClientRegistrationService _sut;

    public ClientRegistrationServiceTests()
    {
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _traditionalUserRegisterValidator = Substitute.For<IValidator<TraditionalUserRegisterDto>>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _hasher = Substitute.For<IHashService>();
        _tokenService = Substitute.For<ITokenService>();
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _unitOfSecurity = Substitute.For<IUnitOfSecurity>();
        _personRepo = Substitute.For<IPersonRepository>();
        _sessionRepo = Substitute.For<ISessionRepository>();

        _unitOfClient.Person.Returns(_personRepo);
        _unitOfSecurity.Session.Returns(_sessionRepo);

        _sut = ServiceReflectionHelper.CreateService<IClientRegistrationService>(ServiceTypeName,
            _unitOfClient,
            _logger,
            _traditionalUserRegisterValidator,
            _hasher,
            _tokenService,
            _privateClientInfoService,
            _unitOfSecurity);
    }

    #region RegisterUserAsync

    [Fact]
    public async Task RegisterUserAsync_WhenInputIsNull_ReturnsBadRequest()
    {
        var result = await _sut.RegisterUserAsync(null!);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00101");
    }

    [Fact]
    public async Task RegisterUserAsync_WhenInvalidRegistrationType_ReturnsConflict()
    {
        var unknownDto = new TestUnknownRegisterDto();
        var result = await _sut.RegisterUserAsync(unknownDto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00101" && m.Text?.Contains("unknown") == true);
    }

    [Fact]
    public async Task RegisterUserAsync_WhenValidationFails_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Invalid") }));

        var result = await _sut.RegisterUserAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        await _personRepo.DidNotReceive().AddAsync(Arg.Any<PersonEntity>());
    }

    [Fact]
    public async Task RegisterUserAsync_WhenValidationResultIsNull_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns((FluentValidation.Results.ValidationResult?)null!);

        var result = await _sut.RegisterUserAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00100");
    }

    [Fact]
    public async Task RegisterUserAsync_WhenEmailAlreadyExists_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Email, Arg.Any<bool>()).Returns(new PersonEntity { Id = 1, Email = dto.Email });

        var result = await _sut.RegisterUserAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00022");
        await _personRepo.DidNotReceive().AddAsync(Arg.Any<PersonEntity>());
    }

    [Fact]
    public async Task RegisterUserAsync_WhenPhoneAlreadyExists_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Email, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _personRepo.FindByUsernameAsync(dto.PhoneNumber, Arg.Any<bool>()).Returns(new PersonEntity { Id = 1, PhoneNumber = dto.PhoneNumber });

        var result = await _sut.RegisterUserAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00023");
        await _personRepo.DidNotReceive().AddAsync(Arg.Any<PersonEntity>());
    }

    [Fact]
    public async Task RegisterUserAsync_WhenValid_AddsPersonAndReturnsUserId()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Email, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _personRepo.FindByUsernameAsync(dto.PhoneNumber, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _hasher.HashText(dto.Password, dto.Email).Returns("hashed");

        var result = await _sut.RegisterUserAsync(dto);

        Assert.True(result.IsSuccess());
        await _personRepo.Received(1).AddAsync(Arg.Is<PersonEntity>(p =>
            p.Name == dto.Name && p.Surname == dto.Surname && p.Email == dto.Email &&
            p.PhoneNumber == dto.PhoneNumber && p.User != null && p.User.TraditionalUser != null &&
            p.User.TraditionalUser.PasswordHash == "hashed"));
        await _unitOfClient.Received(1).SaveChangesAsync();
    }

    #endregion

    #region RegisterAndLoginAsync

    [Fact]
    public async Task RegisterAndLoginAsync_WhenInputIsNull_ReturnsBadRequest()
    {
        var result = await _sut.RegisterAndLoginAsync(null!);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00101");
    }

    [Fact]
    public async Task RegisterAndLoginAsync_WhenValidationFails_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Email", "Invalid") }));

        var result = await _sut.RegisterAndLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        _sessionRepo.DidNotReceive().Add(Arg.Any<SessionEntity>());
    }

    [Fact]
    public async Task RegisterAndLoginAsync_WhenEmailAlreadyExists_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Email, Arg.Any<bool>()).Returns(new PersonEntity { Id = 1, Email = dto.Email });

        var result = await _sut.RegisterAndLoginAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00022");
        _sessionRepo.DidNotReceive().Add(Arg.Any<SessionEntity>());
    }

    [Fact]
    public async Task RegisterAndLoginAsync_WhenValid_ReturnsTokenAndCreatesSession()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Email, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _personRepo.FindByUsernameAsync(dto.PhoneNumber, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _hasher.HashText(dto.Password, dto.Email).Returns("hashed");
        _privateClientInfoService.GetUserTypesAsync(Arg.Any<long>()).Returns(new[] { UserType.User });
        _tokenService.GetToken(out Arg.Any<DateTime>(), Arg.Any<IDictionary<string, object>>())
            .Returns(callInfo =>
            {
                callInfo[0] = DateTime.UtcNow.AddMinutes(15);
                return "access-token";
            });
        _tokenService.GenerateRefreshToken().Returns("refresh-token");

        var result = await _sut.RegisterAndLoginAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal("access-token", result.Model!.AccessToken);
        Assert.Equal("refresh-token", result.Model.RefreshToken);
        _sessionRepo.Received(1).Add(Arg.Any<SessionEntity>());
        await _unitOfSecurity.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RegisterAndLoginAsync_WhenValid_SessionDeviceMatchesRegistrationDeviceInfo()
    {
        var dto = MagicClient.ValidTraditionalUserRegister;
        _traditionalUserRegisterValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _personRepo.FindByUsernameAsync(dto.Email, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _personRepo.FindByUsernameAsync(dto.PhoneNumber, Arg.Any<bool>()).Returns((PersonEntity?)null);
        _hasher.HashText(dto.Password, dto.Email).Returns("hashed");
        _privateClientInfoService.GetUserTypesAsync(Arg.Any<long>()).Returns(new[] { UserType.User });
        _tokenService.GetToken(out Arg.Any<DateTime>(), Arg.Any<IDictionary<string, object>>())
            .Returns(callInfo => { callInfo[0] = DateTime.UtcNow.AddMinutes(15); return "access-token"; });
        _tokenService.GenerateRefreshToken().Returns("refresh-token");

        await _sut.RegisterAndLoginAsync(dto);

        _sessionRepo.Received(1).Add(Arg.Is<SessionEntity>(s =>
            s.Device != null &&
            s.Device.Name == dto.DeviceInfo.Name &&
            s.Device.UUID == dto.DeviceInfo.UUID));
    }

    #endregion

    /// <summary>
    /// DTO type not handled by ClientRegistrationService (simulates unknown registration type).
    /// </summary>
    private sealed record TestUnknownRegisterDto : BaseRegisterDto;
}
