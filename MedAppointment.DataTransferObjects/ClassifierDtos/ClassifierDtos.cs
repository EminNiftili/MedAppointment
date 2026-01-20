namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class ClassifierDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class ClassifierCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class ClassifierUpdateDto : ClassifierCreateDto
    {
    }
}
