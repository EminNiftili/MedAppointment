namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record WeeklySchemaDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        /// <summary>
        /// Color in RGBA hex format (#RRGGBBAA).
        /// </summary>
        public string ColorHex { get; set; } = null!;

        public IEnumerable<DaySchemaDto> DaySchemas { get; set; } = new List<DaySchemaDto>();
    }

}
