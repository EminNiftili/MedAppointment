namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    /// <summary>
    /// Base DTO for day schema write operations (create/update).
    /// </summary>
    public abstract record BaseDaySchemaDto
    {
        public long SpecialtyId { get; set; }
        public long PeriodId { get; set; }
        public long? PlanPaddingTypeId { get; set; }
        /// <summary>
        /// 1 = Monday ... 7 = Sunday.
        /// </summary>
        public byte DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public bool IsClosed { get; set; }
        public IReadOnlyCollection<DayBreakCreateDto> DayBreaks { get; set; } = new List<DayBreakCreateDto>();
    }

    /// <summary>
    /// Response DTO for day schema (read).
    /// </summary>
    public record DaySchemaDto : BaseDaySchemaDto
    {
        public long Id { get; set; }
        public long WeeklySchemaId { get; set; }
    }

    /// <summary>
    /// DTO for creating a day schema (optionally with breaks).
    /// </summary>
    public record DaySchemaCreateDto : BaseDaySchemaDto
    {
        public long WeeklySchemaId { get; set; }
    }

    /// <summary>
    /// DTO for updating a day schema.
    /// </summary>
    public record DaySchemaUpdateDto : BaseDaySchemaDto;
}
