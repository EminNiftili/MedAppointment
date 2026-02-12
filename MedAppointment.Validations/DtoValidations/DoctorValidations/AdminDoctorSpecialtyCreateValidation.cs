namespace MedAppointment.Validations.DtoValidations.DoctorValidations
{
    public class AdminDoctorSpecialtyCreateValidation : BaseValidator<AdminDoctorSpecialtyCreateDto>
    {
        public AdminDoctorSpecialtyCreateValidation()
        {
            RuleFor(x => x.SpecialtyId)
                .GreaterThan(0)
                .WithErrorCode("ERR00102")
                .WithMessage("Specialty id must be greater than 0.");
        }
    }
}
