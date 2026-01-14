namespace MedAppointment.Validations.DtoValidations
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>
    {
        protected static bool IsValidEmail(string? s)
        {
            s = s?.Trim();
            if (string.IsNullOrWhiteSpace(s)) return false;
            try { var _ = new MailAddress(s); return true; } catch { return false; }
        }

        protected static bool ContainsLower(string? s) => s?.Any(char.IsLower) == true;
        protected static bool ContainsUpper(string? s) => s?.Any(char.IsUpper) == true;
        protected static bool ContainsDigit(string? s) => s?.Any(char.IsDigit) == true;
        protected static bool ContainsSpecial(string? s)
            => !string.IsNullOrEmpty(s) && s.Any(ch => !char.IsLetterOrDigit(ch));
        protected static bool NoWhitespace(string? s) => s?.Any(char.IsWhiteSpace) == false;


    }
}
