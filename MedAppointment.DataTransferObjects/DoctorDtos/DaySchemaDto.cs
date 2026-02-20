namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DaySchemaDto
    {
        public long Id { get; set; }
        public long WeeklySchemaId { get; set; }
        public long SpecialtyId { get; set; }
        public long PeriodId { get; set; }
        /// <summary>
        /// Period duration in minutes (for slot generation without loading Period from DB).
        /// </summary>
        public byte PeriodTimeMinutes { get; set; }
        public long? PlanPaddingTypeId { get; set; }
        /// <summary>
        /// Padding time in minutes (for slot generation without loading PlanPaddingType from DB).
        /// </summary>
        public byte? PaddingTimeMinutes { get; set; }
        /// <summary>
        /// Padding position (for slot generation without loading PlanPaddingType from DB).
        /// </summary>
        public PlanPaddingPosition? PaddingPosition { get; set; }
        /// <summary>
        /// 1 = Monday ... 7 = Sunday.
        /// </summary>
        public byte DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        /// <summary>
        /// Number of periods (slots) for this day. 0 when IsClosed.
        /// </summary>
        public byte PeriodCount { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOnlineService { get; set; }
        public bool IsOnSiteService { get; set; }
        public IEnumerable<DayBreakDto> DayBreaks { get; set; } = new List<DayBreakDto>();
    }
}
