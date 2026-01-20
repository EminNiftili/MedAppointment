namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record SpecialtyDto : ClassifierDto
    {
        public long Id { get; set; }
    }

    public record SpecialtyCreateDto : ClassifierDto
    {
    }

    public record SpecialtyUpdateDto : ClassifierDto
    {
    }
}
