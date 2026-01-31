namespace MedAppointment.Validations.DtoValidations.DoctorSchemaValidations
{
    public class DaySchemaCreateValidation : BaseDaySchemaWriteValidation<DaySchemaCreateDto>
    {
        public DaySchemaCreateValidation()
        {
            RuleFor(x => x.WeeklySchemaId)
                .GreaterThan(0L)
                    .WithErrorCode("ERR00110")
                    .WithMessage("Day schema WeeklySchemaId must be greater than 0.");
        }
    }
}
