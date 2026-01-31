namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public abstract record DayBreakDto
    {
        public long Id { get; set; }
        public long DaySchemaId { get; set; }
        public string? Name { get; set; }
        public bool IsVisible { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
