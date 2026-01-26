namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record DoctorDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string? ImagePath { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsConfirm { get; set; }
        public IReadOnlyCollection<SpecialtyDto> Specialties { get; set; } = Array.Empty<SpecialtyDto>();
    }

    public record SpecialtyDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsConfirm { get; set; }
    }
}
