namespace MedAppointment.Entities.Doctor
{
    public class WeeklySchemaEntity : BaseEntity
    {
        public long DoctorId { get; set; }
        public string ColorHex { get; set; } = null!;
        public string Name { get; set; } = null!;

        public DoctorEntity? Doctor { get; set; }
        public ICollection<DaySchemaEntity> DayPlans { get; set; } = new List<DaySchemaEntity>();

    }
}
