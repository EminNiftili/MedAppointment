namespace MedAppointment.Validations.DtoValidations.FileValidations
{
    public class DocumentUploadMetaValidator : BaseValidator<DocumentUploadMetaDto>
    {
        public DocumentUploadMetaValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                    .WithErrorCode("ERR00210")
                    .WithMessage("DoctorId must be greater than 0.");

            RuleFor(x => x.SpecialtyId)
                .GreaterThan(0)
                    .WithErrorCode("ERR00211")
                    .WithMessage("SpecialtyId must be greater than 0.")
                .When(x => x.SpecialtyId.HasValue);

            RuleFor(x => x)
                .Must(HaveExactlyOneTypeFlag)
                    .WithErrorCode("ERR00212")
                    .WithMessage("Exactly one document type must be set: SpecialtyId (has value), IsProfessionBackground, or IsExperience.");

            RuleFor(x => x.Title)
                .MaximumLength(200)
                    .WithErrorCode("ERR00213")
                    .WithMessage("Title must not exceed 200 characters.")
                .When(x => x.Title != null);

            RuleFor(x => x.Issuer)
                .MaximumLength(200)
                    .WithErrorCode("ERR00214")
                    .WithMessage("Issuer must not exceed 200 characters.")
                .When(x => x.Issuer != null);

            RuleFor(x => x.PeriodOfYear)
                .MaximumLength(50)
                    .WithErrorCode("ERR00215")
                    .WithMessage("PeriodOfYear must not exceed 50 characters.")
                .When(x => x.PeriodOfYear != null);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                    .WithErrorCode("ERR00216")
                    .WithMessage("Description must not exceed 2000 characters.")
                .When(x => x.Description != null);
        }

        private static bool HaveExactlyOneTypeFlag(DocumentUploadMetaDto dto)
        {
            int trueCount = (dto.SpecialtyId.HasValue ? 1 : 0)
                          + (dto.IsProfessionBackground ? 1 : 0)
                          + (dto.IsExperience ? 1 : 0);

            return trueCount == 1;
        }
    }
}
