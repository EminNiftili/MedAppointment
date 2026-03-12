namespace MedAppointment.Logic.Tests.Services.ClientServices;

/// <summary>
/// Integration tests for CurrentUserMe service and strategies working together end-to-end.
/// </summary>
public class CurrentUserMeIntegrationTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMeService";
    private const string PlainStrategyTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe.PlainUserMeStrategy";
    private const string AdminStrategyTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe.AdminUserMeStrategy";
    private const string DoctorStrategyTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe.DoctorUserMeStrategy";
    private const string PrivateClientInfoServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.PrivateClientInfoService";

    private readonly ILogger _serviceLogger;
    private readonly ILogger _privateClientInfoLogger;
    private readonly ILogger _plainStrategyLogger;
    private readonly ILogger _adminStrategyLogger;
    private readonly ILogger _doctorStrategyLogger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IUnitOfDoctor _unitOfDoctor;
    private readonly IUserRepository _userRepo;
    private readonly IDoctorRepository _doctorRepo;
    private readonly IOrganizationUserRepository _orgUserRepo;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly ICurrentUserMeService _sut;

    public CurrentUserMeIntegrationTests()
    {
        _serviceLogger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _privateClientInfoLogger = ServiceReflectionHelper.CreateLoggerFor(PrivateClientInfoServiceTypeName);
        _plainStrategyLogger = ServiceReflectionHelper.CreateLoggerFor(PlainStrategyTypeName);
        _adminStrategyLogger = ServiceReflectionHelper.CreateLoggerFor(AdminStrategyTypeName);
        _doctorStrategyLogger = ServiceReflectionHelper.CreateLoggerFor(DoctorStrategyTypeName);
        
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _unitOfDoctor = Substitute.For<IUnitOfDoctor>();
        _userRepo = Substitute.For<IUserRepository>();
        _doctorRepo = Substitute.For<IDoctorRepository>();
        _orgUserRepo = Substitute.For<IOrganizationUserRepository>();

        _unitOfClient.User.Returns(_userRepo);
        _unitOfClient.OrganizationUser.Returns(_orgUserRepo);
        _unitOfDoctor.Doctor.Returns(_doctorRepo);

        _privateClientInfoService = ServiceReflectionHelper.CreateService<IPrivateClientInfoService>(
            PrivateClientInfoServiceTypeName,
            _privateClientInfoLogger,
            _unitOfClient);

        var plainStrategy = ServiceReflectionHelper.CreateService<IUserMeResponseStrategy>(
            PlainStrategyTypeName,
            _plainStrategyLogger,
            _unitOfClient,
            _privateClientInfoService);

        var adminStrategy = ServiceReflectionHelper.CreateService<IUserMeResponseStrategy>(
            AdminStrategyTypeName,
            _adminStrategyLogger,
            _unitOfClient,
            _privateClientInfoService);

        var doctorStrategy = ServiceReflectionHelper.CreateService<IUserMeResponseStrategy>(
            DoctorStrategyTypeName,
            _doctorStrategyLogger,
            _unitOfClient,
            _unitOfDoctor,
            _privateClientInfoService);

        var strategies = new[] { plainStrategy, adminStrategy, doctorStrategy };

        _sut = ServiceReflectionHelper.CreateService<ICurrentUserMeService>(
            ServiceTypeName,
            _serviceLogger,
            _privateClientInfoService,
            strategies);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_PlainUser_ReturnsUserMeDto()
    {
        var user = MagicClient.UserOneWithPerson;
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Person!.Name, dto.Name);
        Assert.Equal(user.Person.Surname, dto.Surname);
        Assert.Single(dto.UserTypes);
        Assert.Contains(UserType.User, dto.UserTypes);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_SystemAdmin_ReturnsUserMeDto()
    {
        var user = MagicClient.UserWithAdmin;
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Person!.Name, dto.Name);
        Assert.Contains(UserType.SystemAdmin, dto.UserTypes);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_Doctor_ReturnsDoctorUserMeDto()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithFullDetails();
        
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Person!.Name, dto.Name);
        Assert.Equal(doctor.Id, dto.DoctorId);
        Assert.Equal(doctor.IsConfirm, dto.IsConfirm);
        Assert.Contains(UserType.Doctor, dto.UserTypes);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_OrganizationAdmin_ReturnsNotFound()
    {
        var user = new UserEntity
        {
            Id = MagicClient.OrgAdminOne.UserId,
            PersonId = 1,
            Person = new PersonEntity
            {
                Id = 1,
                Name = "Org",
                Surname = "Admin",
                Email = "org@example.com",
                PhoneNumber = "+1111111111",
                BirthDate = default
            },
            Admin = null,
            Doctor = null
        };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns(MagicClient.OrgAdminOne);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_UserNotFound_ReturnsNotFound()
    {
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns((UserEntity?)null);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_DoctorWithoutDoctorEntity_ReturnsNotFound()
    {
        var user = MagicClient.UserWithDoctor;
        
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(user.Doctor!.Id, Arg.Any<bool>()).Returns((DoctorEntity?)null);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_AdminTakesPrecedenceOverUser_ReturnsAdminStrategy()
    {
        var user = MagicClient.UserWithAdmin;
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as UserMeDto;
        Assert.NotNull(dto);
        Assert.Contains(UserType.SystemAdmin, dto.UserTypes);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_DoctorWithSpecialties_ReturnsSpecialtiesList()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithSpecialties();
        
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.NotEmpty(dto.Specialties);
        Assert.Single(dto.Specialties);
    }

    [Fact]
    public async Task GetCurrentUserMeAsync_MultipleCallsForSameUser_ReturnsConsistentResults()
    {
        var user = MagicClient.UserOneWithPerson;
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _orgUserRepo.FindFirstAsync(Arg.Any<Expression<Func<OrganizationUserEntity, bool>>>()).Returns((OrganizationUserEntity?)null);

        var result1 = await _sut.GetCurrentUserMeAsync(user.Id);
        var result2 = await _sut.GetCurrentUserMeAsync(user.Id);

        Assert.True(result1.IsSuccess());
        Assert.True(result2.IsSuccess());
        
        var dto1 = result1.Model as UserMeDto;
        var dto2 = result2.Model as UserMeDto;
        
        Assert.NotNull(dto1);
        Assert.NotNull(dto2);
        Assert.Equal(dto1.Id, dto2.Id);
        Assert.Equal(dto1.Name, dto2.Name);
        Assert.Equal(dto1.Email, dto2.Email);
    }

    private static DoctorEntity CreateDoctorWithFullDetails()
    {
        return new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne + 2,
            IsConfirm = true,
            ProfessionId = 1,
            TitleTextId = MagicIds.NameTextId,
            DescriptionTextId = MagicIds.DescriptionTextId,
            Title = new ResourceEntity
            {
                Id = MagicIds.NameTextId,
                Key = "doctor.title",
                Translations = new List<TranslationEntity>
                {
                    new() { Id = 1, ResourceId = MagicIds.NameTextId, LanguageId = MagicIds.LanguageIdOne, Text = "Cardiologist" }
                }
            },
            Description = new ResourceEntity
            {
                Id = MagicIds.DescriptionTextId,
                Key = "doctor.description",
                Translations = new List<TranslationEntity>
                {
                    new() { Id = 2, ResourceId = MagicIds.DescriptionTextId, LanguageId = MagicIds.LanguageIdOne, Text = "Heart specialist with 10 years experience" }
                }
            },
            Specialties = new List<DoctorSpecialtyEntity>()
        };
    }

    private static DoctorEntity CreateDoctorWithSpecialties()
    {
        return new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne + 2,
            IsConfirm = true,
            ProfessionId = 1,
            TitleTextId = MagicIds.NameTextId,
            DescriptionTextId = MagicIds.DescriptionTextId,
            Title = new ResourceEntity
            {
                Id = MagicIds.NameTextId,
                Key = "doctor.title",
                Translations = new List<TranslationEntity>
                {
                    new() { Id = 1, ResourceId = MagicIds.NameTextId, LanguageId = MagicIds.LanguageIdOne, Text = "Doctor Title" }
                }
            },
            Description = new ResourceEntity
            {
                Id = MagicIds.DescriptionTextId,
                Key = "doctor.description",
                Translations = new List<TranslationEntity>
                {
                    new() { Id = 2, ResourceId = MagicIds.DescriptionTextId, LanguageId = MagicIds.LanguageIdOne, Text = "Doctor Description" }
                }
            },
            Specialties = new List<DoctorSpecialtyEntity>
            {
                new()
                {
                    Id = 1,
                    DoctorId = MagicCalendar.DoctorIdOne,
                    SpecialtyId = MagicIds.SpecialtyIdOne,
                    IsConfirm = true,
                    Specialty = new SpecialtyEntity
                    {
                        Id = MagicIds.SpecialtyIdOne,
                        NameTextId = MagicIds.NameTextId,
                        DescriptionTextId = MagicIds.DescriptionTextId,
                        Name = new ResourceEntity
                        {
                            Id = MagicIds.NameTextId,
                            Key = "specialty.name",
                            Translations = new List<TranslationEntity>
                            {
                                new() { Id = 3, ResourceId = MagicIds.NameTextId, LanguageId = MagicIds.LanguageIdOne, Text = "Cardiology" }
                            }
                        },
                        Description = new ResourceEntity
                        {
                            Id = MagicIds.DescriptionTextId,
                            Key = "specialty.description",
                            Translations = new List<TranslationEntity>
                            {
                                new() { Id = 4, ResourceId = MagicIds.DescriptionTextId, LanguageId = MagicIds.LanguageIdOne, Text = "Heart specialist" }
                            }
                        }
                    }
                }
            }
        };
    }
}
