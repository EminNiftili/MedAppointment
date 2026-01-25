namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record TraditionalUserLoginDto : BaseLoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
