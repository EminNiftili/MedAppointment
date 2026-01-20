namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class PeriodUpdateValidation : ClassifierBaseValidator<PeriodUpdateDto>
    {
        public PeriodUpdateValidation()
        {
            RuleFor(x => x.PeriodTime)
                .InclusiveBetween((byte)1, byte.MaxValue)
                    .WithErrorCode("ERR00048")
                    .WithMessage("Period time must be between 1 and 255 minutes.");
        }
    }
}
