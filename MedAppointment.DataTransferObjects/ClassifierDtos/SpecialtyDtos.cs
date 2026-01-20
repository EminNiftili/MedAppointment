namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public class SpecialtyDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class SpecialtyCreateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class SpecialtyUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
