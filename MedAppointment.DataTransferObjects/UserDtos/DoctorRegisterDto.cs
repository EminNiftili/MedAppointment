namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record DoctorRegisterDto<TUserRegisterModel> where TUserRegisterModel : BaseRegisterDto, new()
    {
        public TUserRegisterModel User { get; set; } = null!;
        public List<long> Specialties { get; set; } = new List<long>();
        public List<CreateLocalizationDto> Title { get; set; } = new List<CreateLocalizationDto>();
        public List<CreateLocalizationDto> Description { get; set; } = new List<CreateLocalizationDto>();
    }
}
