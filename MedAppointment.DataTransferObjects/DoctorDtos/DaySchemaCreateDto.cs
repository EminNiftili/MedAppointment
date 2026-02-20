namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DaySchemaCreateDto
    {
        public long SpecialtyId { get; set; }
        public long PeriodId { get; set; }
        public long? PlanPaddingTypeId { get; set; }
        /// <summary>
        /// 1 = Monday ... 7 = Sunday.
        /// </summary>
        public byte DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public byte PeriodCount { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOnlineService { get; set; }
        public bool IsOnSiteService { get; set; }
        public IEnumerable<DayBreakCreateDto> DayBreaks { get; set; } = new List<DayBreakCreateDto>();
    }
}
