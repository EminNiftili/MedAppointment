namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    /// <summary>
    /// Base DTO for weekly schema write operations (create/update).
    /// </summary>
    public abstract record BaseWeeklySchemaDto
    {
        public string Name { get; set; } = null!;
        /// <summary>
        /// Color in RGBA hex format (#RRGGBBAA).
        /// </summary>
        public string ColorHex { get; set; } = null!;
    }

    /// <summary>
    /// Response DTO for weekly schema (read).
    /// </summary>
    public record WeeklySchemaDto : BaseWeeklySchemaDto
    {
        public long Id { get; set; }
        public long DoctorId { get; set; }
        public IReadOnlyCollection<DaySchemaDto> DaySchemas { get; set; } = Array.Empty<DaySchemaDto>();
    }

    /// <summary>
    /// DTO for creating a weekly schema.
    /// </summary>
    public record WeeklySchemaCreateDto : BaseWeeklySchemaDto
    {
        public long DoctorId { get; set; }
    }

    /// <summary>
    /// DTO for updating a weekly schema.
    /// </summary>
    public record WeeklySchemaUpdateDto : BaseWeeklySchemaDto;
}
