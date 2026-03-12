namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class DoctorUserMeStrategyTests
{
    private const string StrategyTypeName = "MedAppointment.Logics.Implementations.ClientServices.CurrentUserMe.DoctorUserMeStrategy";

    private readonly ILogger _logger;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IUnitOfDoctor _unitOfDoctor;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IUserRepository _userRepo;
    private readonly IDoctorRepository _doctorRepo;
    private readonly IUserMeResponseStrategy _sut;

    public DoctorUserMeStrategyTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(StrategyTypeName);
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _unitOfDoctor = Substitute.For<IUnitOfDoctor>();
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _userRepo = Substitute.For<IUserRepository>();
        _doctorRepo = Substitute.For<IDoctorRepository>();

        _unitOfClient.User.Returns(_userRepo);
        _unitOfDoctor.Doctor.Returns(_doctorRepo);

        _sut = ServiceReflectionHelper.CreateService<IUserMeResponseStrategy>(StrategyTypeName,
            _logger,
            _unitOfClient,
            _unitOfDoctor,
            _privateClientInfoService);
    }

    [Fact]
    public void CanHandle_WhenContainsDoctor_ReturnsTrue()
    {
        var userTypes = new[] { UserType.Doctor };

        var result = _sut.CanHandle(userTypes);

        Assert.True(result);
    }

    [Fact]
    public void CanHandle_WhenMultipleTypesIncludingDoctor_ReturnsTrue()
    {
        var userTypes = new[] { UserType.User, UserType.Doctor };

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
    public void CanHandle_WhenOnlySystemAdmin_ReturnsFalse()
    {
        var userTypes = new[] { UserType.SystemAdmin };

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
            Doctor = MagicClient.DoctorOne
        };
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns(userWithoutPerson);

        var result = await _sut.BuildAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorIsNull_ReturnsNotFound()
    {
        var userWithoutDoctor = new UserEntity
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
                BirthDate = default
            },
            Doctor = null
        };
        _userRepo.GetByIdAsync(MagicClient.UserIdOne, Arg.Any<bool>()).Returns(userWithoutDoctor);

        var result = await _sut.BuildAsync(MagicClient.UserIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorEntityNotFound_ReturnsNotFound()
    {
        var user = MagicClient.UserWithDoctor;
        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(user.Doctor!.Id, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.BuildAsync(user.Id);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorFound_ReturnsDoctorUserMeDto()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithDetails();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.OK, result.HttpStatus);
        Assert.NotNull(result.Model);

        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(user.Id, dto.Id);
        Assert.Equal(user.Provider, dto.Provider);
        Assert.Equal(user.Person!.Name, dto.Name);
        Assert.Equal(user.Person.Surname, dto.Surname);
        Assert.Equal(user.Person.Email, dto.Email);
        Assert.Equal(user.Person.PhoneNumber, dto.PhoneNumber);
        Assert.Equal(doctor.Id, dto.DoctorId);
        Assert.Equal(doctor.IsConfirm, dto.IsConfirm);
        Assert.Equal(doctor.ProfessionId, dto.ProfessionId);
        Assert.Equal(userTypes, dto.UserTypes);
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorHasTitle_ReturnsTitleText()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithDetails();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal("Doctor Title", dto.Title);
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorHasDescription_ReturnsDescriptionText()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithDetails();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal("Doctor Description", dto.Description);
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorHasNoTitle_ReturnsEmptyString()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithoutTitle();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Title);
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorHasPresentationVideo_ReturnsVideoUrl()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithDetails();
        doctor.PresentationVideoUrl = "https://example.com/video.mp4";
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal("https://example.com/video.mp4", dto.PresentationVideoUrl);
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorHasSpecialties_ReturnsSpecialtiesList()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithSpecialties();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.NotEmpty(dto.Specialties);
        Assert.Single(dto.Specialties);
        
        var specialty = dto.Specialties.First();
        Assert.Equal(MagicIds.SpecialtyIdOne, specialty.Id);
        Assert.True(specialty.IsConfirm);
    }

    [Fact]
    public async Task BuildAsync_WhenDoctorHasNoSpecialties_ReturnsEmptyList()
    {
        var user = MagicClient.UserWithDoctor;
        var doctor = CreateDoctorWithoutSpecialties();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Empty(dto.Specialties);
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
            Doctor = MagicClient.DoctorOne
        };
        var doctor = CreateDoctorWithoutTitle();
        var userTypes = new[] { UserType.Doctor };

        _userRepo.GetByIdAsync(user.Id, Arg.Any<bool>()).Returns(user);
        _doctorRepo.GetByIdAsync(doctor.Id, Arg.Any<bool>()).Returns(doctor);
        _privateClientInfoService.GetUserTypesAsync(user.Id).Returns(userTypes);

        var result = await _sut.BuildAsync(user.Id);

        Assert.True(result.IsSuccess());
        var dto = result.Model as DoctorUserMeDto;
        Assert.NotNull(dto);
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.Surname);
        Assert.Equal(string.Empty, dto.FatherName);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.PhoneNumber);
    }

    private static DoctorEntity CreateDoctorWithDetails()
    {
        return new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne,
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
            Specialties = new List<DoctorSpecialtyEntity>()
        };
    }

    private static DoctorEntity CreateDoctorWithoutTitle()
    {
        return new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne,
            IsConfirm = true,
            ProfessionId = 1,
            TitleTextId = 0,
            DescriptionTextId = 0,
            Title = null,
            Description = null,
            Specialties = new List<DoctorSpecialtyEntity>()
        };
    }

    private static DoctorEntity CreateDoctorWithSpecialties()
    {
        return new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne,
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

    private static DoctorEntity CreateDoctorWithoutSpecialties()
    {
        return new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne,
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
            Specialties = null
        };
    }
}
