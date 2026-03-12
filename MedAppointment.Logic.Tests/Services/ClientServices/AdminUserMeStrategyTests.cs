namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class AdminUserMeStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe.AdminUserMeStrategy";

    private readonly ILogger _logger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IUserRepository _userRepo;
    private readonly IUserMeResponseStrategy _sut;

    public AdminUserMeStrategyTests()
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
    public void CanHandle_WhenContainsSystemAdmin_ReturnsTrue()
    {
        var userTypes = new[] { UserType.SystemAdmin };

        var result = _sut.CanHandle(userTypes);

        Assert.True(result);
    }

    [Fact]
    public void CanHandle_WhenMultipleTypesIncludingSystemAdmin_ReturnsTrue()
    {
        var userTypes = new[] { UserType.User, UserType.SystemAdmin };

        var result = _sut.CanHandle(userTypes);

        Assert.True(result);
    }

    [Fact]
    public void CanHandle_WhenOnlyUser_ReturnsFalse()
    {
        var userTypes = new[] { UserType.User };

        var result = _sut.CanHandle(userTypes);

        Assert.False(result);
    }

    [Fact]
    public void CanHandle_WhenOnlyDoctor_ReturnsFalse()
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
            Person = null,
            Admin = new AdminEntity { Id = 1, UserId = MagicClient.UserIdOne }
        };
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns(userWithoutPerson);

        var result = await _sut.BuildAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task BuildAsync_WhenAdminUserFound_ReturnsUserMeDto()
    {
        var user = MagicClient.UserWithAdmin;
        var userTypes = new[] { UserType.SystemAdmin };

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
        Assert.Equal(user.Person.Email, dto.Email);
        Assert.Equal(user.Person.PhoneNumber, dto.PhoneNumber);
        Assert.Equal(userTypes, dto.UserTypes);
    }

    [Fact]
    public async Task BuildAsync_WhenAdminHasImage_ReturnsImagePath()
    {
        var user = new UserEntity
        {
            Id = MagicClient.UserIdOne,
            PersonId = MagicClient.PersonIdOne,
            Provider = 0,
            Person = new PersonEntity
            {
                Id = MagicClient.PersonIdOne,
                Name = "Admin",
                Surname = "User",
                Email = "admin@example.com",
                PhoneNumber = "+1111111111",
                BirthDate = default,
                Image = new ImageEntity { Id = 1, FilePath = "/images/admin.jpg" }
            },
            Admin = new AdminEntity { Id = 1, UserId = MagicClient.UserIdOne }
        };
        var userTypes = new[] { UserType.SystemAdmin };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Equal("/images/admin.jpg", dto.ImagePath);
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
            },
            Admin = new AdminEntity { Id = 1, UserId = MagicClient.UserIdOne }
        };
        var userTypes = new[] { UserType.SystemAdmin };

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
