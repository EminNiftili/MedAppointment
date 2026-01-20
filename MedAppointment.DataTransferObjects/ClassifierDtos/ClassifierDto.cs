namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public abstract record ClassifierDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
