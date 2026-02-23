namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class AdminUserServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.AdminUserService";

    private readonly ILogger _logger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IValidator<UserPaginationQueryDto> _userPaginationQueryValidator;
    private readonly IUserRepository _userRepo;
    private readonly IOrganizationUserRepository _orgUserRepo;
    private readonly IAdminUserService _sut;

    public AdminUserServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _userPaginationQueryValidator = Substitute.For<IValidator<UserPaginationQueryDto>>();
        _userRepo = Substitute.For<IUserRepository>();
        _orgUserRepo = Substitute.For<IOrganizationUserRepository>();

        _unitOfClient.User.Returns(_userRepo);
        _unitOfClient.OrganizationUser.Returns(_orgUserRepo);

        _sut = ServiceReflectionHelper.CreateService<IAdminUserService>(ServiceTypeName,
            _logger,
            _unitOfClient,
            _privateClientInfoService,
            _userPaginationQueryValidator);
    }

    [Fact]
    public async Task GetUsersAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicClient.ValidUserPaginationQuery;
        _userPaginationQueryValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetUsersAsync(query);

        Assert.False(result.IsSuccess());
        await _userRepo.DidNotReceive().FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>());
    }

    [Fact]
    public async Task GetUsersAsync_WhenValidationResultIsNull_ReturnsBadRequest()
    {
        var query = MagicClient.ValidUserPaginationQuery;
        _userPaginationQueryValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns((FluentValidation.Results.ValidationResult?)null!);

        var result = await _sut.GetUsersAsync(query);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00100");
    }

    [Fact]
    public async Task GetUsersAsync_WhenValid_ReturnsPagedUsers()
    {
        var query = MagicClient.ValidUserPaginationQuery;
        _userPaginationQueryValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        var users = new List<UserEntity> { MagicClient.UserOneWithPerson };
        _userRepo.FindAsync(Arg.Any<Expression<Func<UserEntity, bool>>>()).Returns(users);
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(new[] { UserType.User });

        var result = await _sut.GetUsersAsync(query);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Single(result.Model!.Items);
        Assert.Equal(1, result.Model.TotalCount);
        Assert.Equal("Test", result.Model.Items.First().Name);
        Assert.Equal("User", result.Model.Items.First().Surname);
    }

    [Fact]
    public async Task GetUserDetailsByIdAsync_WhenUserNotFound_ReturnsNotFound()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns((UserEntity?)null);

        var result = await _sut.GetUserDetailsByIdAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task GetUserDetailsByIdAsync_WhenPersonIsNull_ReturnsNotFound()
    {
        var userWithoutPerson = new UserEntity { Id = MagicClient.UserIdOne, PersonId = 0, Person = null };
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns(userWithoutPerson);

        var result = await _sut.GetUserDetailsByIdAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task GetUserDetailsByIdAsync_WhenValid_ReturnsAdminUserDetails()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns(MagicClient.UserOneWithPerson);
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(new[] { UserType.User });

        var result = await _sut.GetUserDetailsByIdAsync(MagicClient.UserIdOne);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(MagicClient.UserIdOne, result.Model!.Id);
        Assert.Equal("Test", result.Model.Name);
        Assert.Equal("User", result.Model.Surname);
        Assert.Single(result.Model.UserTypes);
        Assert.Equal(UserType.User, result.Model.UserTypes.First());
    }

    [Fact]
    public async Task RemoveUserAsync_WhenUserNotFound_ReturnsNotFound()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, false).Returns((UserEntity?)null);

        var result = await _sut.RemoveUserAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
        await _userRepo.DidNotReceive().RemoveAsync(Arg.Any<long>());
    }

    [Fact]
    public async Task RemoveUserAsync_WhenValid_RemovesUserAndReturnsNoContent()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, false).Returns(MagicClient.UserOneWithPerson);

        var result = await _sut.RemoveUserAsync(MagicClient.UserIdOne);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        await _userRepo.Received(1).RemoveAsync(MagicClient.UserIdOne);
        await _unitOfClient.Received(1).SaveChangesAsync();
    }
}
