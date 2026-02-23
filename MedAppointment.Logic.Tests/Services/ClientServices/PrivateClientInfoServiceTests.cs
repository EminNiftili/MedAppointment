namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class PrivateClientInfoServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.PrivateClientInfoService";

    private readonly ILogger _logger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IUserRepository _userRepo;
    private readonly IOrganizationUserRepository _orgUserRepo;
    private readonly IPrivateClientInfoService _sut;

    public PrivateClientInfoServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _userRepo = Substitute.For<IUserRepository>();
        _orgUserRepo = Substitute.For<IOrganizationUserRepository>();

        _unitOfClient.User.Returns(_userRepo);
        _unitOfClient.OrganizationUser.Returns(_orgUserRepo);

        _sut = ServiceReflectionHelper.CreateService<IPrivateClientInfoService>(ServiceTypeName,
            _logger,
            _unitOfClient);
    }

    [Fact]
    public async Task GetUserTypesAsync_WhenUserNotFound_ReturnsEmptyArray()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns((UserEntity?)null);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>(), Arg.Any<bool>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetUserTypesAsync(MagicClient.UserIdOne);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserTypesAsync_WhenUserHasAdmin_ReturnsSystemAdmin()
    {
        _userRepo.GetByIdAsync(MagicClient.UserWithAdmin.Id, Arg.Any<bool>()).Returns(MagicClient.UserWithAdmin);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetUserTypesAsync(MagicClient.UserWithAdmin.Id);

        Assert.NotNull(result);
        Assert.Contains(UserType.SystemAdmin, result);
    }

    [Fact]
    public async Task GetUserTypesAsync_WhenUserHasDoctor_ReturnsDoctor()
    {
        _userRepo.GetByIdAsync(MagicClient.UserWithDoctor.Id, Arg.Any<bool>()).Returns(MagicClient.UserWithDoctor);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetUserTypesAsync(MagicClient.UserWithDoctor.Id);

        Assert.NotNull(result);
        Assert.Contains(UserType.Doctor, result);
    }

    [Fact]
    public async Task GetUserTypesAsync_WhenUserIsOrgAdmin_ReturnsOrganizationAdmin()
    {
        var userWithOrgAdmin = new UserEntity
        {
            Id = MagicClient.OrgAdminOne.UserId,
            PersonId = 1,
            Person = new PersonEntity { Id = 1, Name = "Org", Surname = "Admin", Email = "org@ex.com", PhoneNumber = "+1", BirthDate = default },
            Admin = null,
            Doctor = null
        };
        _userRepo.GetByIdAsync(userWithOrgAdmin.Id, Arg.Any<bool>()).Returns(userWithOrgAdmin);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns(MagicClient.OrgAdminOne);

        var result = await _sut.GetUserTypesAsync(userWithOrgAdmin.Id);

        Assert.NotNull(result);
        Assert.Contains(UserType.OrganizationAdmin, result);
    }

    [Fact]
    public async Task GetUserTypesAsync_WhenUserHasNoSpecialRole_ReturnsUser()
    {
        _userRepo.GetByIdAsync(MagicClient.UserOneWithPerson.Id, Arg.Any<bool>()).Returns(MagicClient.UserOneWithPerson);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetUserTypesAsync(MagicClient.UserOneWithPerson.Id);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(UserType.User, result[0]);
    }
}
