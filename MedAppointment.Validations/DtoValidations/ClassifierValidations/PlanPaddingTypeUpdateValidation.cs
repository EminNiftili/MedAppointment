namespace MedAppointment.Validations.DtoValidations.ClassifierValidations
{
    public class PlanPaddingTypeUpdateValidation : BaseClassifierValidation<PlanPaddingTypeUpdateDto>
    {
        public PlanPaddingTypeUpdateValidation() : base()
        {
            RuleFor(x => x.PaddingPosition)
                .IsInEnum()
                .WithErrorCode("ERR00129")
                .WithMessage("Invalid padding position. Must be a valid PlanPaddingPosition enum value (0-4).");

            RuleFor(x => x.PaddingTime)
                .InclusiveBetween((byte)1, byte.MaxValue)
                .WithErrorCode("ERR00142")
                .WithMessage("Padding time must be between 1 and 255 minutes.");
        }
    }
}
