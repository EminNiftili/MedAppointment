using MedAppointment.DataTransferObjects.DoctorDtos;

namespace MedAppointment.DataTransferObjects.UserDtos
{
    /// <summary>
    /// Response for GET users/me for Doctor role. Includes user profile and doctor-specific details.
    /// </summary>
    public record DoctorUserMeDto
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

        public long DoctorId { get; init; }
        public bool IsConfirm { get; init; }
        public long ProfessionId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? PresentationVideoUrl { get; init; }
        public IReadOnlyCollection<DoctorSpecialtyDto> Specialties { get; init; } = Array.Empty<DoctorSpecialtyDto>();
    }
}
