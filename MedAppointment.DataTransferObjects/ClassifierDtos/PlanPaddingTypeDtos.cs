namespace MedAppointment.DataTransferObjects.ClassifierDtos
{
    public record PlanPaddingTypeDto : ClassifierDto
    {
        public long Id { get; set; }
        public PlanPaddingPosition PaddingPosition { get; set; }
        public byte PaddingTime { get; set; }
    }

    public record PlanPaddingTypeCreateDto : ClassifierDto
    {
        public PlanPaddingPosition PaddingPosition { get; set; }
        public byte PaddingTime { get; set; }
    }

    public record PlanPaddingTypeUpdateDto : ClassifierDto
    {
        public PlanPaddingPosition PaddingPosition { get; set; }
        public byte PaddingTime { get; set; }
    }
}
