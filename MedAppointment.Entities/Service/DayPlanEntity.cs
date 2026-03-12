namespace MedAppointment.Entities.Service
{
    public class DayPlanEntity : BaseEntity
    {
        public long DoctorId { get; set; }
        public long PeriodId { get; set; }
        public DateTime BelongDate { get; set; }
        /// <summary>
        /// 1=Monday ... 7=Sunday
        /// </summary>
        public int DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }   
        public TimeSpan CloseTime { get; set; }  
        public bool IsClosed { get; set; }

        public DoctorEntity? Doctor { get; set; }
        public PeriodEntity? Period { get; set; }
        public ICollection<DayPlanSpecialtyEntity> DayPlanSpecialties { get; set; } = new List<DayPlanSpecialtyEntity>();
    }
}
