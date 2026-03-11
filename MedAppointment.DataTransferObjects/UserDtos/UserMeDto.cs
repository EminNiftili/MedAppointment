namespace MedAppointment.DataTransferObjects.UserDtos
{
    /// <summary>
    /// Response for GET users/me for Admin and plain User roles.
    /// </summary>
    public record UserMeDto
    {
        public long Id { get; init; }
        public byte Provider { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Surname { get; init; } = string.Empty;
        public string FatherName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public DateTime BirthDate { get; init; }
        public string? ImagePath { get; init; }
        public IReadOnlyCollection<UserType> UserTypes { get; init; } = Array.Empty<UserType>();
    }
}
