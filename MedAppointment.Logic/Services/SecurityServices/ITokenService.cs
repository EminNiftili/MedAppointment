namespace MedAppointment.Logics.Services.SecurityServices
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        string GetToken(out DateTime expiredDate, IDictionary<string, object>? claims = null);
    }
}
