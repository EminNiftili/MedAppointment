namespace MedAppointment.DataTransferObjects.UserDtos
{
    /// <summary>
    /// Short user info for list views: name, contact, role.
    /// </summary>
    public record UserListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IReadOnlyCollection<UserType> UserTypes { get; set; } = Array.Empty<UserType>();
    }
}
