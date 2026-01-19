namespace MedAppointment.Logics.Services.SecurityServices
{
    public interface ILoginService
    {
        Task<Result<TokenDto>> TraditionalLoginAsync(TraditionalUserLoginDto traditionalUserLogin);
    }
}
