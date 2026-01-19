namespace MedAppointment.Logics.Services.SecurityServices
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        string GetToken(IDictionary<string, object>? claims = null);
    }
}
