namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record DoctorRegisterDto<TUserRegisterModel> where TUserRegisterModel : BaseRegisterDto, new()
    {
        public TUserRegisterModel User { get; set; } = null!;
        public List<long> Specialties { get; set; } = new List<long>();
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
