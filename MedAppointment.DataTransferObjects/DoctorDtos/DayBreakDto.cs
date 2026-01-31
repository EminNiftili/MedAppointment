namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    /// <summary>
    /// Base DTO for day break write operations (create/update).
    /// </summary>
    public abstract record BaseDayBreakDto
    {
        public string? Name { get; set; }
        public bool IsVisible { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    /// <summary>
    /// Response DTO for day break (read).
    /// </summary>
    public record DayBreakDto : BaseDayBreakDto
    {
        public long Id { get; set; }
        public long DaySchemaId { get; set; }
    }

    /// <summary>
    /// DTO for creating a day break (DaySchemaId from route when adding to a schema).
    /// </summary>
    public record DayBreakCreateDto : BaseDayBreakDto;

    /// <summary>
    /// DTO for updating a day break.
    /// </summary>
    public record DayBreakUpdateDto : BaseDayBreakDto;
}
