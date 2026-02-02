namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public abstract record ClassifierDto
    {
        public string Key { get; set; } = null!;
        public List<LocalizationDto> Name { get; set; } = new List<LocalizationDto>();
        public List<LocalizationDto> Description { get; set; } = new List<LocalizationDto>();
    }
}
