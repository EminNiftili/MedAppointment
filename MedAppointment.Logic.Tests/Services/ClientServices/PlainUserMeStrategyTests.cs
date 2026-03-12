namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class PlainUserMeStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe.PlainUserMeStrategy";

    private readonly ILogger _logger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IUserRepository _userRepo;
    private readonly IUserMeResponseStrategy _sut;

    public PlainUserMeStrategyTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(StrategyTypeName);
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _userRepo = Substitute.For<IUserRepository>();

        _unitOfClient.User.Returns(_userRepo);

        _sut = ServiceReflectionHelper.CreateService<IUserMeResponseStrategy>(StrategyTypeName,
            _logger,
            _unitOfClient,
            _privateClientInfoService);
    }

    [Fact]
    public void CanHandle_WhenSingleUserType_ReturnsTrue()
    {
        var userTypes = new[] { UserType.User };

        var result = _sut.CanHandle(userTypes);

        Assert.True(result);
    }

    [Fact]
    public void CanHandle_WhenMultipleUserTypes_ReturnsFalse()
    {
        var userTypes = new[] { UserType.User, UserType.Doctor };

        var result = _sut.CanHandle(userTypes);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_WhenSystemAdmin_ReturnsFalse()
    {
        var userTypes = new[] { UserType.SystemAdmin };

        var result = _sut.CanHandle(userTypes);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_WhenDoctor_ReturnsFalse()
    {
        var userTypes = new[] { UserType.Doctor };

        var result = _sut.CanHandle(userTypes);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_WhenEmptyArray_ReturnsFalse()
    {
        var userTypes = Array.Empty<UserType>();

        var result = _sut.CanHandle(userTypes);

        Assert.False(result);
    }

    [Fact]
    public async Task BuildAsync_WhenUserNotFound_ReturnsNotFound()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns((UserEntity?)null);

        var result = await _sut.BuildAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task BuildAsync_WhenPersonIsNull_ReturnsNotFound()
    {
        var userWithoutPerson = new UserEntity
        {
            Id = MagicClient.UserIdOne,
            PersonId = MagicClient.PersonIdOne,
            Provider = 0,
            Person = null
        };
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns(userWithoutPerson);

        var result = await _sut.BuildAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task BuildAsync_WhenUserFound_ReturnsUserMeDto()
    {
        var user = MagicClient.UserOneWithPerson;
        var userTypes = new[] { UserType.User };
        
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.Model);
        
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Provider, dto.Provider);
        Assert.Equal(user.Person!.Name, dto.Name);
        Assert.Equal(user.Person.Surname, dto.Surname);
        Assert.Equal(user.Person.FatherName, dto.FatherName);
        Assert.Equal(user.Person.Email, dto.Email);
        Assert.Equal(user.Person.PhoneNumber, dto.PhoneNumber);
        Assert.Equal(user.Person.BirthDate, dto.BirthDate);
        Assert.Equal(userTypes, dto.UserTypes);
    }

    [Fact]
    public async Task BuildAsync_WhenUserHasImage_ReturnsImagePath()
    {
        var user = new UserEntity
        {
            Id = MagicClient.UserIdOne,
            PersonId = MagicClient.PersonIdOne,
            Provider = 0,
            Person = new PersonEntity
            {
                Id = MagicClient.PersonIdOne,
                Name = "Test",
                Surname = "User",
                Email = "test@example.com",
                PhoneNumber = "+1234567890",
                BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Image = new ImageEntity { Id = 1, FilePath = "/images/user.jpg" }
            }
        };
        var userTypes = new[] { UserType.User };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Equal("/images/user.jpg", dto.ImagePath);
    }

    [Fact]
    public async Task BuildAsync_WhenUserHasNoImage_ReturnsNullImagePath()
    {
        var user = MagicClient.UserOneWithPerson;
        var userTypes = new[] { UserType.User };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Null(dto.ImagePath);
    }

    [Fact]
    public async Task BuildAsync_WhenPersonFieldsAreNull_ReturnsEmptyStrings()
    {
        var user = new UserEntity
        {
            Id = MagicClient.UserIdOne,
            PersonId = MagicClient.PersonIdOne,
            Provider = 0,
            Person = new PersonEntity
            {
                Id = MagicClient.PersonIdOne,
                Name = null,
                Surname = null,
                FatherName = null,
                Email = null,
                PhoneNumber = null,
                BirthDate = default
            }
        };
        var userTypes = new[] { UserType.User };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Surname);
        Assert.Equal(string.Empty, dto.FatherName);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.PhoneNumber);
    }
}
