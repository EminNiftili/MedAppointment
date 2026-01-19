namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record TraditionalUserLoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DeviceDto DeviceInfo { get; set; } = null!;
    }
}
