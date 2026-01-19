namespace MedAppointment.DataTransferObjects.CredentialDtos
{
    public record DeviceDto
    {
        public string Name { get; set; } = null!;
        public DeviceType DeviceType { get; set; }
        public ApplicationType AppType { get; set; }
        public string? OSName { get; set; }
        public string? OSVersion { get; set; }
        public string UUID { get; set; } = null!;
    }
}
