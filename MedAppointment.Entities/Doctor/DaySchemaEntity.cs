namespace MedAppointment.Entities.Doctor
{
    public class DaySchemaEntity : BaseEntity
    {
        public long WeeklySchemaId { get; set; }
        public long SpecialtyId { get; set; }
        public long PeriodId { get; set; }
        public long? PlanPaddingTypeId { get; set; }
        /// <summary>
        /// 1=Monday ... 7=Sunday
        /// </summary>
        public byte DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        /// <summary>
        /// Number of periods (slots) for this day. Used when generating day slots; 0 when IsClosed.
        /// </summary>
        public byte PeriodCount { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOnlineService { get; set; }
        public bool IsOnSiteService { get; set; }

        public WeeklySchemaEntity? WeeklySchema { get; set; }
        public SpecialtyEntity? Specialty { get; set; }
        public PeriodEntity? Period { get; set; }
        public PlanPaddingTypeEntity? PlanPaddingType { get; set; }
        public ICollection<DayBreakEntity> DayBreaks { get; set; } = new List<DayBreakEntity>();
    }
}
