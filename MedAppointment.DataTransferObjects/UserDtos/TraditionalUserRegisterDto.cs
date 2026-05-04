using MedAppointment.DataTransferObjects.CredentialDtos;

namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record TraditionalUserRegisterDto : BaseRegisterDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public long GenderId { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        /// <summary>Same shape as login; required for model binding under <c>User</c> on doctor registration.</summary>
        public DeviceDto DeviceInfo { get; set; } = null!;
    }
}
