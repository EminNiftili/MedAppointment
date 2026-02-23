namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Magic data for Security and Schedule unit tests.
/// </summary>
public static class MagicSecurity
{
    /// <summary>
    /// Valid device info for login DTOs.
    /// </summary>
    public static DeviceDto ValidDeviceDto => new()
    {
        Name = "TestDevice",
        DeviceType = DeviceType.Windows,
        AppType = ApplicationType.Web,
        OSName = "Windows",
        OSVersion = "10",
        UUID = "test-uuid-123"
    };

    /// <summary>
    /// Valid traditional login DTO (email as username). Matches PersonWithTraditionalUser.Email.
    /// </summary>
    public static TraditionalUserLoginDto ValidTraditionalUserLogin => new()
    {
        Username = "test@example.com",
        Password = "password123",
        DeviceInfo = ValidDeviceDto
    };

    /// <summary>
    /// Valid refresh token request.
    /// </summary>
    public static RefreshTokenRequestDto ValidRefreshTokenRequest => new()
    {
        RefreshToken = "valid-refresh-token-base64"
    };

    /// <summary>
    /// Person with traditional user (for login success). Email matches ValidTraditionalUserLogin.Username.
    /// </summary>
    public static PersonEntity PersonWithTraditionalUser => new()
    {
        Id = MagicClient.PersonIdOne,
        Name = "Test",
        Surname = "User",
        Email = "test@example.com",
        PhoneNumber = "+1234567890",
        BirthDate = default,
        User = new UserEntity
        {
            Id = MagicClient.UserIdOne,
            PersonId = MagicClient.PersonIdOne,
            TraditionalUser = new TraditionalUserEntity
            {
                PasswordHash = "will-be-set-by-test"
            }
        }
    };
}
