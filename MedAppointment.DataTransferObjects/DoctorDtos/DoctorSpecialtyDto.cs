namespace MedAppointment.DataTransferObjects.DoctorDtos
{
    public record DoctorSpecialtyDto : SpecialtyDto
    {
        public bool IsConfirm { get; set; }
    }
}
