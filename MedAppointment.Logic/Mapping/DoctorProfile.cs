namespace MedAppointment.Logics.Mapping
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<DoctorEntity, DoctorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User!.Person!.Name ?? string.Empty))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User!.Person!.Surname ?? string.Empty))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.User!.Person!.Image == null ? null : src.User!.Person!.Image.FilePath))
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom((src, _, _, context) =>
                {
                    var includeUnconfirmed = context.Items.TryGetValue("IncludeUnconfirmed", out var value)
                        && value is bool flag
                        && flag;
                    return includeUnconfirmed
                        ? src.Specialties
                        : src.Specialties.Where(s => s.IsConfirm);
                }));

            CreateMap<DoctorSpecialtyEntity, DataTransferObjects.DoctorDtos.DoctorSpecialtyDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SpecialtyId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Specialty!.Name ?? string.Empty))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Specialty!.Description ?? string.Empty))
                .ForMember(dest => dest.IsConfirm, opt => opt.MapFrom(src => src.IsConfirm));
        }
    }
}
