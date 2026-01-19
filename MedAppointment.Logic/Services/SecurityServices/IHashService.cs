namespace MedAppointment.Logics.Services.SecurityServices
{
    public interface IHashService
    {
        public string HashText(string key, string salt);
    }
}
