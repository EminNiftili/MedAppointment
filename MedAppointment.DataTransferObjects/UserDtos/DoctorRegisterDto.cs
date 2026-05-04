using MedAppointment.DataTransferObjects.CredentialDtos;

namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record DoctorRegisterDto<TUserRegisterModel> where TUserRegisterModel : BaseRegisterDto, new()
    {
        public TUserRegisterModel User { get; set; } = null!;
        /// <summary>Optional device fingerprint for the session created after registration (same as login).</summary>
        public DeviceDto? DeviceInfo { get; set; }
        public long ProfessionId { get; set; }
        public string? PresentationVideoUrl { get; set; }
        public List<long> Specialties { get; set; } = new List<long>();
        public List<long> ServiceGenderIds { get; set; } = new List<long>();
        public List<long> ServiceLanguageIds { get; set; } = new List<long>();
        public List<CreateLocalizationDto> Title { get; set; } = new List<CreateLocalizationDto>();
        public List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }
}
