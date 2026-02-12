namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record AdminUserDetailsDto
    {
        public long Id { get; set; }
        public byte Provider { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string? ImagePath { get; set; }
        public IReadOnlyCollection<UserType> UserTypes { get; set; } = Array.Empty<UserType>();
    }
}
