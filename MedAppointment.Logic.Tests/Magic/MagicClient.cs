namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Magic data for Client services unit tests (Doctor, AdminUser, PrivateClientInfo, ClientRegistration).
/// </summary>
public static class MagicClient
{
    public const long UserIdOne = 7001;
    public const long PersonIdOne = 7002;

    /// <summary>
    /// User with Person only (no Admin, no Doctor, not org admin) -> UserType.User.
    /// </summary>
    public static UserEntity UserOneWithPerson => new()
    {
        Id = UserIdOne,
        PersonId = PersonIdOne,
        Provider = 0,
        Person = new PersonEntity
        {
            Id = PersonIdOne,
            Name = "Test",
            Surname = "User",
            FatherName = "Father",
            Email = "test@example.com",
            PhoneNumber = "+1234567890",
            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        Admin = null,
        Doctor = null
    };

    /// <summary>
    /// User with Admin -> UserType.SystemAdmin.
    /// </summary>
    public static UserEntity UserWithAdmin => new()
    {
        Id = UserIdOne + 1,
        PersonId = PersonIdOne + 1,
        Provider = 0,
        Person = new PersonEntity
        {
            Id = PersonIdOne + 1,
            Name = "Admin",
            Surname = "User",
            FatherName = "",
            Email = "admin@example.com",
            PhoneNumber = "+111",
            BirthDate = default
        },
        Admin = new AdminEntity { Id = 1, UserId = UserIdOne + 1 },
        Doctor = null
    };

    /// <summary>
    /// User with Doctor -> UserType.Doctor.
    /// </summary>
    public static UserEntity UserWithDoctor => new()
    {
        Id = UserIdOne + 2,
        PersonId = PersonIdOne + 2,
        Provider = 0,
        Person = new PersonEntity
        {
            Id = PersonIdOne + 2,
            Name = "Doc",
            Surname = "Tor",
            Email = "doc@example.com",
            PhoneNumber = "+222",
            BirthDate = default
        },
        Admin = null,
        Doctor = DoctorOne
    };

    /// <summary>
    /// Doctor entity (verified) with one specialty.
    /// </summary>
    public static DoctorEntity DoctorOne => new()
    {
        Id = MagicCalendar.DoctorIdOne,
        UserId = UserIdOne + 2,
        IsConfirm = true,
        TitleTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        Specialties = new List<DoctorSpecialtyEntity>
        {
            new() { Id = 1, DoctorId = MagicCalendar.DoctorIdOne, SpecialtyId = MagicIds.SpecialtyIdOne, IsConfirm = true, IsDeleted = false }
        }
    };

    /// <summary>
    /// Doctor entity not confirmed (for EnsureDoctorIsVerifiedAsync failure).
    /// </summary>
    public static DoctorEntity DoctorOneUnconfirmed => new()
    {
        Id = MagicCalendar.DoctorIdOne,
        UserId = UserIdOne + 2,
        IsConfirm = false,
        TitleTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        Specialties = new List<DoctorSpecialtyEntity>()
    };

    /// <summary>
    /// OrganizationUser for org admin (UserId, IsAdmin = true).
    /// </summary>
    public static OrganizationUserEntity OrgAdminOne => new()
    {
        Id = 1,
        UserId = UserIdOne + 3,
        OrganizationId = 1,
        IsAdmin = true
    };

    /// <summary>
    /// Valid pagination query for doctor list.
    /// </summary>
    public static PaginationQueryDto ValidDoctorPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10
    };

    /// <summary>
    /// Valid user pagination query.
    /// </summary>
    public static UserPaginationQueryDto ValidUserPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        SurnameFilter = null,
        EmailFilter = null,
        PhoneFilter = null,
        UserTypeFilter = null
    };

    /// <summary>
    /// Valid AdminDoctorSpecialtyCreateDto.
    /// </summary>
    public static AdminDoctorSpecialtyCreateDto ValidAdminDoctorSpecialtyCreate => new()
    {
        SpecialtyId = MagicIds.SpecialtyIdOne,
        IsConfirmed = false
    };

    /// <summary>
    /// Valid TraditionalUserRegisterDto for registration tests.
    /// </summary>
    public static TraditionalUserRegisterDto ValidTraditionalUserRegister => new()
    {
        Name = "New",
        Surname = "User",
        FatherName = "Father",
        BirthDate = new DateTime(1995, 5, 15, 0, 0, 0, DateTimeKind.Utc),
        Email = "newuser@example.com",
        PhoneNumber = "+9999999999",
        Password = "SecurePwd123!"
    };
}
