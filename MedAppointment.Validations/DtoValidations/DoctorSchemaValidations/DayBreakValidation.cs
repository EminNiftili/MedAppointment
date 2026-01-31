namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class DayBreakValidation : BaseValidator<DayBreakDto>
    {
        public DayBreakValidation()
        {
            RuleFor(x => x.DaySchemaId)
                .GreaterThan(0L)
                    .WithErrorCode("ERR00120")
                    .WithMessage("Day break DaySchemaId must be greater than 0.");

            RuleFor(x => x)
                .Must(x => x.EndTime > x.StartTime)
                    .WithErrorCode("ERR00117")
                    .WithMessage("Day break EndTime must be after StartTime.");

            When(x => x.Name != null, () =>
            {
                RuleFor(x => x.Name)
                    .MaximumLength(150)
                        .WithErrorCode("ERR00118")
                        .WithMessage("Day break name must not exceed 150 characters.");
            });
        }
    }
}
