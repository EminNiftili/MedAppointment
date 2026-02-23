namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class ClientRegistrationServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.ClientRegistrationService";

    private readonly IUnitOfClient _unitOfClient;
    private readonly IValidator<TraditionalUserRegisterDto> _traditionalUserRegisterValidator;
    private readonly ILogger _logger;
    private readonly IHashService _hasher;
    private readonly IPersonRepository _personRepo;
    private readonly IClientRegistrationService _sut;

    public ClientRegistrationServiceTests()
    {
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _traditionalUserRegisterValidator = Substitute.For<IValidator<TraditionalUserRegisterDto>>();
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _hasher = Substitute.For<IHashService>();
        _personRepo = Substitute.For<IPersonRepository>();

        _unitOfClient.Person.Returns(_personRepo);

        _sut = ServiceReflectionHelper.CreateService<IClientRegistrationService>(ServiceTypeName,
            _unitOfClient,
            _logger,
            _traditionalUserRegisterValidator,
            _hasher);
    }

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

    /// <summary>
    /// DTO type not handled by ClientRegistrationService (simulates unknown registration type).
    /// </summary>
    private sealed record TestUnknownRegisterDto : BaseRegisterDto;
}
