namespace MedAppointment.Entities.Doctor
{
    public class DayBreakEntity : BaseEntity
    {
        public string? Name { get; set; }
        public bool IsVisible { get; set; }
        public long DaySchemaId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DaySchemaEntity? DaySchema { get; set; }
    }
}
