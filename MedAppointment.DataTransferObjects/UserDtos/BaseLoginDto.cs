namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record BaseLoginDto
    {
        public DeviceDto DeviceInfo { get; set; } = null!;
    }
}
