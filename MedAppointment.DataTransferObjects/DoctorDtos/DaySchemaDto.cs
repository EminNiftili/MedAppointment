namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public abstract record DaySchemaDto
    {
        public long Id { get; set; }
        public long WeeklySchemaId { get; set; }
        public long SpecialtyId { get; set; }
        public long PeriodId { get; set; }
        public long? PlanPaddingTypeId { get; set; }
        /// <summary>
        /// 1 = Monday ... 7 = Sunday.
        /// </summary>
        public byte DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public bool IsClosed { get; set; }
        public IEnumerable<DayBreakDto> DayBreaks { get; set; } = new List<DayBreakDto>();
    }
}
