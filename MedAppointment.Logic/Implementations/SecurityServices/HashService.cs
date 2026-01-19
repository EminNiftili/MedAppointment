namespace MedAppointment.Logics.Implementations.SecurityServices
{
    internal class HashService : IHashService
    {
        protected readonly ILogger<HashService> Logger;

        public HashService(ILogger<HashService> logger)
        {
            Logger = logger;
        }

        public string HashText(string key, string salt)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("Salt cannot be null or empty.", nameof(salt));

            using var sha256 = SHA256.Create();

            var combined = $"{key}{salt}";
            var bytes = Encoding.UTF8.GetBytes(combined);
            var hashBytes = sha256.ComputeHash(bytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
