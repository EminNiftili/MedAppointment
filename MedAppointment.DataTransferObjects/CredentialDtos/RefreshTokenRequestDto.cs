namespace MedAppointment.DataTransferObjects.CredentialDtos
{
    public record RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
