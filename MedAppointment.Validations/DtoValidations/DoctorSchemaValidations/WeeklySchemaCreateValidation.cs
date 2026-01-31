namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class WeeklySchemaCreateValidation : BaseWeeklySchemaWriteValidation<WeeklySchemaCreateDto>
    {
        public WeeklySchemaCreateValidation()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0L)
                    .WithErrorCode("ERR00109")
                    .WithMessage("DoctorId must be greater than 0.");
        }
    }
}
