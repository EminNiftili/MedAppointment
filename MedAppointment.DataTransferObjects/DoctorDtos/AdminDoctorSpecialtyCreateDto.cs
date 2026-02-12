namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record AdminDoctorSpecialtyCreateDto
    {
        public long SpecialtyId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
