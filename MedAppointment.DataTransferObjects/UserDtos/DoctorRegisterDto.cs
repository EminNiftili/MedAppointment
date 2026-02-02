namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record DoctorRegisterDto<TUserRegisterModel> where TUserRegisterModel : BaseRegisterDto, new()
    {
        public TUserRegisterModel User { get; set; } = null!;
        public List<long> Specialties { get; set; } = new List<long>();
        public List<LocalizationDto> Title { get; set; } = new List<LocalizationDto>();
        public List<LocalizationDto> Description { get; set; } = new List<LocalizationDto>();
    }
}
