using MedAppointment.DataTransferObjects.CredentialDtos;

namespace MedAppointment.DataTransferObjects.UserDtos
{
    public abstract record BaseRegisterDto
    {
        public DeviceDto DeviceInfo { get; set; } = null!;
    }
}
