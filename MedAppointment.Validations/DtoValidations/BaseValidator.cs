namespace MedAppointment.Validations.DtoValidations
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>
    {
        private static Dictionary<string, string> PhonePatternByCountries = new Dictionary<string, string>()
        {
            { "AZ" , @"^(?:\+994)(10|50|51|55|60|70|77|99)\d{7}$" }
        };
        protected static bool BeValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            bool isValid = false;
            foreach(var phonePattern in PhonePatternByCountries)
            {
                if(System.Text.RegularExpressions.Regex.IsMatch(phone, phonePattern.Value))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }

        protected static bool BeValidEmail(string? s)
        {
            s = s?.Trim();
            if (string.IsNullOrWhiteSpace(s)) return false;
            try { var _ = new MailAddress(s); return true; } catch { return false; }
        }
        protected static bool BeOnlyLetter(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            foreach (var c in s)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            return true;
        }
        protected static bool ContainsLower(string? s) => s?.Any(char.IsLower) == true;
        protected static bool ContainsUpper(string? s) => s?.Any(char.IsUpper) == true;
        protected static bool ContainsDigit(string? s) => s?.Any(char.IsDigit) == true;
        protected static bool ContainsSpecial(string? s)
            => !string.IsNullOrEmpty(s) && s.Any(ch => !char.IsLetterOrDigit(ch));
        protected static bool NoWhitespace(string? s) => s?.Any(char.IsWhiteSpace) == false;


    }
}
