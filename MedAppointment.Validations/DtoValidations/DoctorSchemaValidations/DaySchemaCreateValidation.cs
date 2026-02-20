namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class DaySchemaCreateValidation : BaseValidator<DaySchemaCreateDto>
    {
        public DaySchemaCreateValidation()
        {
            RuleFor(x => x.SpecialtyId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00111")
                .WithMessage("Day schema SpecialtyId must be greater than 0.");

            RuleFor(x => x.PeriodId)
                .GreaterThan(0L)
                .WithErrorCode("ERR00112")
                .WithMessage("Day schema PeriodId must be greater than 0.");

            RuleFor(x => x.DayOfWeek)
                .InclusiveBetween((byte)1, (byte)7)
                .WithErrorCode("ERR00113")
                .WithMessage("Day schema DayOfWeek must be between 1 (Monday) and 7 (Sunday).");

            RuleFor(x => x.PeriodCount)
                .InclusiveBetween((byte)0, (byte)255)
                .WithErrorCode("ERR00130")
                .WithMessage("Day schema PeriodCount must be between 0 and 255.");

            When(x => x.PlanPaddingTypeId.HasValue, () =>
            {
                RuleFor(x => x.PlanPaddingTypeId!.Value)
                    .GreaterThan(0L)
                    .WithErrorCode("ERR00119")
                    .WithMessage("Day schema PlanPaddingTypeId must be greater than 0 when provided.");
            });

            RuleForEach(x => x.DayBreaks)
                .SetValidator(new DayBreakCreateValidation())
                .When(x => x.DayBreaks != null);
        }
    }
}
