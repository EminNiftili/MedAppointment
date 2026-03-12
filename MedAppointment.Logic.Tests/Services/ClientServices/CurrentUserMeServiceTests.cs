namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class CurrentUserMeServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMeService";

    private readonly ILogger _logger;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IUserMeResponseStrategy _plainUserStrategy;
    private readonly IUserMeResponseStrategy _adminUserStrategy;
    private readonly IUserMeResponseStrategy _doctorUserStrategy;
    private readonly ICurrentUserMeService _sut;

    public CurrentUserMeServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _plainUserStrategy = Substitute.For<IUserMeResponseStrategy>();
        _adminUserStrategy = Substitute.For<IUserMeResponseStrategy>();
        _doctorUserStrategy = Substitute.For<IUserMeResponseStrategy>();

        var strategies = new[] { _plainUserStrategy, _adminUserStrategy, _doctorUserStrategy };

        _sut = ServiceReflectionHelper.CreateService<ICurrentUserMeService>(ServiceTypeName,
            _logger,
            _privateClientInfoService,
            strategies);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_WhenNoStrategyFound_ReturnsNotFound()
    {
        var userTypes = new[] { UserType.User };
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(userTypes);
        _plainUserStrategy.CanHandle(userTypes).Returns(false);
        _adminUserStrategy.CanHandle(userTypes).Returns(false);
        _doctorUserStrategy.CanHandle(userTypes).Returns(false);

        var result = await _sut.GetCurrentUserMeAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_WhenStrategyFoundButReturnsError_ReturnsMergedError()
    {
        var userTypes = new[] { UserType.User };
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(userTypes);
        _plainUserStrategy.CanHandle(userTypes).Returns(true);

        var strategyResult = Result<object>.Create();
        strategyResult.AddMessage("ERR00024", "User cannot found", HttpStatusCode.NotFound);
        _plainUserStrategy.BuildAsync(MagicClient.UserIdOne).Returns(strategyResult);

        var result = await _sut.GetCurrentUserMeAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_WhenStrategySucceeds_ReturnsSuccessWithData()
    {
        var userTypes = new[] { UserType.User };
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(userTypes);
        _plainUserStrategy.CanHandle(userTypes).Returns(true);

        var dto = new UserMeDto
        {
            Id = MagicClient.UserIdOne,
            Provider = 0,
            Name = "Test",
            Surname = "User",
            Email = "test@example.com",
            PhoneNumber = "+1234567890",
            UserTypes = userTypes
        };

        var strategyResult = Result<object>.Create();
        strategyResult.Success(dto, HttpStatusCode.OK);
        _plainUserStrategy.BuildAsync(MagicClient.UserIdOne).Returns(strategyResult);

        var result = await _sut.GetCurrentUserMeAsync(MagicClient.UserIdOne);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.Model);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_WhenExceptionThrown_ReturnsCriticalError()
    {
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne)
            .Returns<UserType[]>(x => throw new InvalidOperationException("Test exception"));

        var result = await _sut.GetCurrentUserMeAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00100");
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_WhenMultipleStrategies_SelectsFirstMatching()
    {
        var userTypes = new[] { UserType.SystemAdmin };
        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(userTypes);
        
        _plainUserStrategy.CanHandle(userTypes).Returns(false);
        _adminUserStrategy.CanHandle(userTypes).Returns(true);
        _doctorUserStrategy.CanHandle(userTypes).Returns(true);

        var dto = new UserMeDto
        {
            Id = MagicClient.UserIdOne,
            Provider = 0,
            Name = "Admin",
            Surname = "User",
            Email = "admin@example.com",
            PhoneNumber = "+1111111111",
            UserTypes = userTypes
        };

        var adminResult = Result<object>.Create();
        adminResult.Success(dto, HttpStatusCode.OK);
        _adminUserStrategy.BuildAsync(MagicClient.UserIdOne).Returns(adminResult);

        var result = await _sut.GetCurrentUserMeAsync(MagicClient.UserIdOne);

        Assert.True(result.IsSuccess());
        await _adminUserStrategy.Received(1).BuildAsync(MagicClient.UserIdOne);
        await _doctorUserStrategy.DidNotReceive().BuildAsync(Arg.Any<long>());
    }
}
