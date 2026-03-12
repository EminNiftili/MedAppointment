namespace MedAppointment.Entities.Composition
{
    public class WeeklySchemaSpecialtyEntity : BaseEntity
    {
        public long WeeklySchemaId { get; set; }
        public long SpecialtyId { get; set; }

        public WeeklySchemaEntity? WeeklySchema { get; set; }
        public SpecialtyEntity? Specialty { get; set; }
    }
}
