namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DoctorSchemaCreateDto
    {
        public long DoctorId { get; set; }
        public string Name { get; set; } = null!;
        /// <summary>
        /// Color in RGBA hex format (#RRGGBBAA).
        /// </summary>
        public string ColorHex { get; set; } = null!;
        public IEnumerable<DaySchemaCreateDto> DaySchemas { get; set; } = new List<DaySchemaCreateDto>();
    }
}
