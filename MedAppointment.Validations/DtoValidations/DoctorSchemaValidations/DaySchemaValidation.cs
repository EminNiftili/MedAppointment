namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class DaySchemaValidation : BaseValidator<DaySchemaDto>
    {
        protected DaySchemaValidation()
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

            When(x => x.PlanPaddingTypeId.HasValue, () =>
            {
                RuleFor(x => x.PlanPaddingTypeId!.Value)
                    .GreaterThan(0L)
                        .WithErrorCode("ERR00119")
                        .WithMessage("Day schema PlanPaddingTypeId must be greater than 0 when provided.");
            });

            RuleForEach(x => x.DayBreaks)
                .SetValidator(new DayBreakValidation())
                .When(x => x.DayBreaks != null);
        }
    }
}
