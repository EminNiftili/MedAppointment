namespace MedAppointment.Logic.Tests.Services.ClientServices;

public class DoctorServiceTests
{
    private const string ServiceTypeName = "MedAppointment.Logics.Implementations.ClientServices.DoctorService";

    private readonly ILogger _logger;
    private readonly ILocalizerService _localizerService;
    private readonly IUnitOfDoctor _unitOfDoctor;
    private readonly IUnitOfClient _unitOfClient;
    private readonly IUnitOfClassifier _unitOfClassifier;
    private readonly IClientRegistrationService _clientRegistration;
    private readonly IValidator<PaginationQueryDto> _paginationQueryValidator;
    private readonly IValidator<AdminDoctorSpecialtyCreateDto> _adminDoctorSpecialtyCreateValidator;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IPrivateClientInfoService _privateClientInfoService;
    private readonly IUnitOfSecurity _unitOfSecurity;
    private readonly IDoctorRepository _doctorRepo;
    private readonly ISpecialtyRepository _specialtyRepo;
    private readonly IUserRepository _userRepo;
    private readonly ISessionRepository _sessionRepo;
    private readonly IDoctorService _sut;

    public DoctorServiceTests()
    {
        _logger = ServiceReflectionHelper.CreateLoggerFor(ServiceTypeName);
        _localizerService = Substitute.For<ILocalizerService>();
        _unitOfDoctor = Substitute.For<IUnitOfDoctor>();
        _unitOfClient = Substitute.For<IUnitOfClient>();
        _unitOfClassifier = Substitute.For<IUnitOfClassifier>();
        _clientRegistration = Substitute.For<IClientRegistrationService>();
        _paginationQueryValidator = Substitute.For<IValidator<PaginationQueryDto>>();
        _adminDoctorSpecialtyCreateValidator = Substitute.For<IValidator<AdminDoctorSpecialtyCreateDto>>();
        _mapper = Substitute.For<IMapper>();
        _tokenService = Substitute.For<ITokenService>();
        _privateClientInfoService = Substitute.For<IPrivateClientInfoService>();
        _unitOfSecurity = Substitute.For<IUnitOfSecurity>();
        _doctorRepo = Substitute.For<IDoctorRepository>();
        _specialtyRepo = Substitute.For<ISpecialtyRepository>();
        _userRepo = Substitute.For<IUserRepository>();
        _sessionRepo = Substitute.For<ISessionRepository>();

        _unitOfDoctor.Doctor.Returns(_doctorRepo);
        _unitOfClassifier.Specialty.Returns(_specialtyRepo);
        _unitOfClient.User.Returns(_userRepo);
        _unitOfSecurity.Session.Returns(_sessionRepo);

        _sut = ServiceReflectionHelper.CreateService<IDoctorService>(ServiceTypeName,
            _localizerService,
            _logger,
            _unitOfDoctor,
            _unitOfClient,
            _unitOfClassifier,
            _clientRegistration,
            _paginationQueryValidator,
            _adminDoctorSpecialtyCreateValidator,
            _mapper,
            _tokenService,
            _privateClientInfoService,
            _unitOfSecurity);
    }

    [Fact]
    public async Task GetDoctorsAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var query = MagicClient.ValidDoctorPaginationQuery;
        _paginationQueryValidator.ValidateAsync(query, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("PageSize", "Invalid") }));

        var result = await _sut.GetDoctorsAsync(query, true);

        Assert.False(result.IsSuccess());
        await _doctorRepo.DidNotReceive().GetAllAsync(Arg.Any<bool>());
    }

    [Fact]
    public async Task GetDoctorsAsync_WhenValid_ReturnsPagedDoctors()
    {
        var query = MagicClient.ValidDoctorPaginationQuery;
        _paginationQueryValidator.ValidateAsync(query, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        var doctors = new List<DoctorEntity> { MagicClient.DoctorOne };
        _doctorRepo.GetAllAsync(Arg.Any<bool>()).Returns(doctors);
        var dto = new DoctorDto { Id = MagicClient.DoctorOne.Id, Name = "Doc", Surname = "Tor", Title = "Dr", Description = "Desc", IsConfirm = true, Specialties = Array.Empty<DoctorSpecialtyDto>() };
        _mapper.Map<DoctorDto>(Arg.Any<object>(), Arg.Any<Action<IMappingOperationOptions>>()).Returns(dto);

        var result = await _sut.GetDoctorsAsync(query, true);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Single(result.Model!.Items);
        Assert.Equal(1, result.Model.TotalCount);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorNotFound_ReturnsNotFound()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.GetDoctorByIdAsync(MagicCalendar.DoctorIdOne, true);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenDoctorNotConfirmedAndIncludeUnconfirmedFalse_ReturnsNotFound()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOneUnconfirmed);

        var result = await _sut.GetDoctorByIdAsync(MagicCalendar.DoctorIdOne, false);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
    }

    [Fact]
    public async Task GetDoctorByIdAsync_WhenFound_ReturnsDoctorDto()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOne);
        var dto = new DoctorDto { Id = MagicCalendar.DoctorIdOne, Name = "Doc", Surname = "Tor", Title = "Dr", Description = "Desc", IsConfirm = true, Specialties = Array.Empty<DoctorSpecialtyDto>() };
        _mapper.Map<DoctorDto>(Arg.Any<object>(), Arg.Any<Action<IMappingOperationOptions>>()).Returns(dto);

        var result = await _sut.GetDoctorByIdAsync(MagicCalendar.DoctorIdOne, true);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal(MagicCalendar.DoctorIdOne, result.Model!.Id);
    }

    [Fact]
    public async Task EnsureDoctorIsVerifiedAsync_WhenDoctorNotFound_ReturnsNotFound()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.EnsureDoctorIsVerifiedAsync(MagicCalendar.DoctorIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task EnsureDoctorIsVerifiedAsync_WhenDoctorNotConfirmed_ReturnsForbidden()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOneUnconfirmed);

        var result = await _sut.EnsureDoctorIsVerifiedAsync(MagicCalendar.DoctorIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Forbidden, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00125");
    }

    [Fact]
    public async Task EnsureDoctorIsVerifiedAsync_WhenDoctorConfirmed_ReturnsSuccess()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOne);

        var result = await _sut.EnsureDoctorIsVerifiedAsync(MagicCalendar.DoctorIdOne);

        Assert.True(result.IsSuccess());
    }

    [Fact]
    public async Task AddDoctorSpecialtyAsync_WhenValidationFails_ReturnsUnsuccessfulResult()
    {
        var dto = MagicClient.ValidAdminDoctorSpecialtyCreate;
        _adminDoctorSpecialtyCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("SpecialtyId", "Invalid") }));

        var result = await _sut.AddDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, dto);

        Assert.False(result.IsSuccess());
        await _specialtyRepo.DidNotReceive().GetByIdAsync(Arg.Any<long>());
    }

    [Fact]
    public async Task AddDoctorSpecialtyAsync_WhenDoctorNotFound_ReturnsNotFound()
    {
        var dto = MagicClient.ValidAdminDoctorSpecialtyCreate;
        _adminDoctorSpecialtyCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.AddDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, dto);

        Assert.False(result.IsSuccess());
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task AddDoctorSpecialtyAsync_WhenSpecialtyNotFound_ReturnsNotFound()
    {
        var dto = MagicClient.ValidAdminDoctorSpecialtyCreate;
        _adminDoctorSpecialtyCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOne);
        _specialtyRepo.GetByIdAsync(dto.SpecialtyId).Returns((SpecialtyEntity?)null);

        var result = await _sut.AddDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00050");
    }

    [Fact]
    public async Task AddDoctorSpecialtyAsync_WhenSpecialtyAlreadyExists_ReturnsConflict()
    {
        var dto = MagicClient.ValidAdminDoctorSpecialtyCreate;
        _adminDoctorSpecialtyCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOne);
        _specialtyRepo.GetByIdAsync(dto.SpecialtyId).Returns(MagicSpecialty.EntityOneWithLocalization);

        var result = await _sut.AddDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00051");
    }

    [Fact]
    public async Task AddDoctorSpecialtyAsync_WhenValid_AddsSpecialtyAndReturnsNoContent()
    {
        var doctorWithOneSpecialty = new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            UserId = MagicClient.UserIdOne,
            IsConfirm = true,
            Specialties = new List<DoctorSpecialtyEntity> { new() { SpecialtyId = 999, IsDeleted = false } }
        };
        var dto = new AdminDoctorSpecialtyCreateDto { SpecialtyId = MagicIds.SpecialtyIdOne, IsConfirmed = true };
        _adminDoctorSpecialtyCreateValidator.ValidateAsync(dto, Arg.Any<CancellationToken>()).Returns(new FluentValidation.Results.ValidationResult());
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(doctorWithOneSpecialty);
        _specialtyRepo.GetByIdAsync(dto.SpecialtyId).Returns(MagicSpecialty.EntityOneWithLocalization);

        var result = await _sut.AddDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, dto);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        _doctorRepo.Received(1).Update(Arg.Any<DoctorEntity>());
        await _unitOfDoctor.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RemoveDoctorSpecialtyAsync_WhenDoctorNotFound_ReturnsNotFound()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.RemoveDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, MagicIds.SpecialtyIdOne);

        Assert.False(result.IsSuccess());
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task RemoveDoctorSpecialtyAsync_WhenSpecialtyNotFound_ReturnsNotFound()
    {
        var doctorWithOtherSpecialty = new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            Specialties = new List<DoctorSpecialtyEntity> { new() { SpecialtyId = 999, IsDeleted = false } }
        };
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(doctorWithOtherSpecialty);

        var result = await _sut.RemoveDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, MagicIds.SpecialtyIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00057");
    }

    [Fact]
    public async Task RemoveDoctorSpecialtyAsync_WhenValid_SoftDeletesAndReturnsNoContent()
    {
        var specialtyToRemove = new DoctorSpecialtyEntity { Id = 1, DoctorId = MagicCalendar.DoctorIdOne, SpecialtyId = MagicIds.SpecialtyIdOne, IsDeleted = false };
        var doctor = new DoctorEntity
        {
            Id = MagicCalendar.DoctorIdOne,
            Specialties = new List<DoctorSpecialtyEntity> { specialtyToRemove }
        };
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(doctor);

        var result = await _sut.RemoveDoctorSpecialtyAsync(MagicCalendar.DoctorIdOne, MagicIds.SpecialtyIdOne);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        Assert.True(specialtyToRemove.IsDeleted);
        await _unitOfDoctor.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ConfirmDoctorAsync_WhenDoctorNotFound_ReturnsNotFound()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns((DoctorEntity?)null);

        var result = await _sut.ConfirmDoctorAsync(MagicCalendar.DoctorIdOne, true);

        Assert.False(result.IsSuccess());
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00056");
    }

    [Fact]
    public async Task ConfirmDoctorAsync_WhenValid_SetsConfirmAndReturnsNoContent()
    {
        var doctor = new DoctorEntity { Id = MagicCalendar.DoctorIdOne, IsConfirm = false, Specialties = new List<DoctorSpecialtyEntity> { new() { IsConfirm = false } } };
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(doctor);

        var result = await _sut.ConfirmDoctorAsync(MagicCalendar.DoctorIdOne, true);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        Assert.True(doctor.IsConfirm);
        await _unitOfDoctor.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task ConfirmDoctorSpecialtiesAsync_WhenDoctorNotConfirmed_ReturnsConflict()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOneUnconfirmed);

        var result = await _sut.ConfirmDoctorSpecialtiesAsync(MagicCalendar.DoctorIdOne, MagicIds.SpecialtyIdOne);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00058");
    }

    [Fact]
    public async Task ConfirmDoctorSpecialtiesAsync_WhenSpecialtyNotFound_ReturnsNotFound()
    {
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(MagicClient.DoctorOne);

        var result = await _sut.ConfirmDoctorSpecialtiesAsync(MagicCalendar.DoctorIdOne, MagicIds.SpecialtyIdNonExistent);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NotFound, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00057");
    }

    [Fact]
    public async Task ConfirmDoctorSpecialtiesAsync_WhenValid_SetsSpecialtyConfirmAndReturnsNoContent()
    {
        var specialty = new DoctorSpecialtyEntity { Id = 1, DoctorId = MagicCalendar.DoctorIdOne, SpecialtyId = MagicIds.SpecialtyIdOne, IsConfirm = false };
        var doctor = new DoctorEntity { Id = MagicCalendar.DoctorIdOne, IsConfirm = true, Specialties = new List<DoctorSpecialtyEntity> { specialty } };
        _doctorRepo.GetByIdAsync(MagicCalendar.DoctorIdOne, Arg.Any<bool>()).Returns(doctor);

        var result = await _sut.ConfirmDoctorSpecialtiesAsync(MagicCalendar.DoctorIdOne, MagicIds.SpecialtyIdOne);

        Assert.True(result.IsSuccess());
        Assert.Equal(HttpStatusCode.NoContent, result.HttpStatus);
        Assert.True(specialty.IsConfirm);
        await _unitOfDoctor.Received(1).SaveChangesAsync();
    }

    #region RegisterAsync

    [Fact]
    public async Task RegisterAsync_WhenUserRegistrationFails_ReturnsBadRequest()
    {
        var dto = MagicClient.ValidDoctorRegister;
        var failedResult = Result<long>.Create();
        failedResult.AddMessage("ERR00022", "Email already registered!", HttpStatusCode.BadRequest);
        _clientRegistration.RegisterUserAsync(dto.User).Returns(failedResult);

        var result = await _sut.RegisterAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00022");
        await _unitOfDoctor.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task RegisterAsync_WhenUserEntityNotFoundAfterRegistration_ReturnsConflict()
    {
        var dto = MagicClient.ValidDoctorRegister;
        var userRegisterResult = Result<long>.Create();
        userRegisterResult.Success(MagicClient.UserIdOne);
        _clientRegistration.RegisterUserAsync(dto.User).Returns(userRegisterResult);
        _userRepo.FindFirstAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<bool>()).Returns((UserEntity?)null);

        var result = await _sut.RegisterAsync(dto);

        Assert.False(result.IsSuccess());
        Assert.Equal(HttpStatusCode.Conflict, result.HttpStatus);
        Assert.Contains(result.Messages, m => m.TextCode == "ERR00024");
    }

    [Fact]
    public async Task RegisterAsync_WhenLocalizerFails_ReturnsFailed()
    {
        var dto = MagicClient.ValidDoctorRegister;
        var userRegisterResult = Result<long>.Create();
        userRegisterResult.Success(MagicClient.UserIdOne);
        _clientRegistration.RegisterUserAsync(dto.User).Returns(userRegisterResult);
        _userRepo.FindFirstAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<bool>())
            .Returns(new UserEntity { Id = MagicClient.UserIdOne });

        var failedLocalize = Result<long>.Create();
        failedLocalize.AddMessage("ERR00200", "Localization failed", HttpStatusCode.InternalServerError);
        _localizerService.AddResourceAsync(Arg.Any<string>(), Arg.Any<IEnumerable<CreateLocalizationDto>>())
            .Returns(failedLocalize);

        var result = await _sut.RegisterAsync(dto);

        Assert.False(result.IsSuccess());
        await _unitOfDoctor.DidNotReceive().SaveChangesAsync();
        _sessionRepo.DidNotReceive().Add(Arg.Any<SessionEntity>());
    }

    [Fact]
    public async Task RegisterAsync_WhenValid_RegistersDoctorAndReturnsToken()
    {
        var dto = MagicClient.ValidDoctorRegister;
        var userRegisterResult = Result<long>.Create();
        userRegisterResult.Success(MagicClient.UserIdOne);
        _clientRegistration.RegisterUserAsync(dto.User).Returns(userRegisterResult);

        var userEntity = new UserEntity { Id = MagicClient.UserIdOne };
        _userRepo.FindFirstAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<bool>())
            .Returns(userEntity);

        var titleResult = Result<long>.Create();
        titleResult.Success(MagicIds.NameTextId);
        var descResult = Result<long>.Create();
        descResult.Success(MagicIds.DescriptionTextId);
        _localizerService.AddResourceAsync(Arg.Is<string>(s => s.Contains("title")), Arg.Any<IEnumerable<CreateLocalizationDto>>())
            .Returns(titleResult);
        _localizerService.AddResourceAsync(Arg.Is<string>(s => s.Contains("desc")), Arg.Any<IEnumerable<CreateLocalizationDto>>())
            .Returns(descResult);

        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(new[] { UserType.Doctor });
        _tokenService.GetToken(out Arg.Any<DateTime>(), Arg.Any<IDictionary<string, object>>())
            .Returns(callInfo =>
            {
                callInfo[0] = DateTime.UtcNow.AddMinutes(15);
                return "access-token";
            });
        _tokenService.GenerateRefreshToken().Returns("refresh-token");

        var result = await _sut.RegisterAsync(dto);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Model);
        Assert.Equal("access-token", result.Model!.AccessToken);
        Assert.Equal("refresh-token", result.Model.RefreshToken);
        _userRepo.Received(1).Update(Arg.Any<UserEntity>());
        await _unitOfDoctor.Received(1).SaveChangesAsync();
        _sessionRepo.Received(1).Add(Arg.Is<SessionEntity>(s =>
            s.UserId == MagicClient.UserIdOne &&
            s.Device != null &&
            s.Device.UUID == dto.User.DeviceInfo.UUID));
        await _unitOfSecurity.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task RegisterAsync_WhenValid_TokenClaimsContainDoctorRole()
    {
        var dto = MagicClient.ValidDoctorRegister;
        var userRegisterResult = Result<long>.Create();
        userRegisterResult.Success(MagicClient.UserIdOne);
        _clientRegistration.RegisterUserAsync(dto.User).Returns(userRegisterResult);
        _userRepo.FindFirstAsync(Arg.Any<Expression<Func<UserEntity, bool>>>(), Arg.Any<bool>())
            .Returns(new UserEntity { Id = MagicClient.UserIdOne });

        var titleResult = Result<long>.Create(); titleResult.Success(MagicIds.NameTextId);
        var descResult = Result<long>.Create(); descResult.Success(MagicIds.DescriptionTextId);
        _localizerService.AddResourceAsync(Arg.Is<string>(s => s.Contains("title")), Arg.Any<IEnumerable<CreateLocalizationDto>>()).Returns(titleResult);
        _localizerService.AddResourceAsync(Arg.Is<string>(s => s.Contains("desc")), Arg.Any<IEnumerable<CreateLocalizationDto>>()).Returns(descResult);

        _privateClientInfoService.GetUserTypesAsync(MagicClient.UserIdOne).Returns(new[] { UserType.Doctor });
        IDictionary<string, object>? capturedClaims = null;
        _tokenService.GetToken(out Arg.Any<DateTime>(), Arg.Any<IDictionary<string, object>>())
            .Returns(callInfo =>
            {
                capturedClaims = (IDictionary<string, object>?)callInfo[1];
                callInfo[0] = DateTime.UtcNow.AddMinutes(15);
                return "access-token";
            });
        _tokenService.GenerateRefreshToken().Returns("refresh-token");

        await _sut.RegisterAsync(dto);

        Assert.NotNull(capturedClaims);
        var roles = capturedClaims![ClaimTypes.Role] as string[];
        Assert.NotNull(roles);
        Assert.Contains(UserType.Doctor.ToString(), roles!);
    }

    #endregion
}
