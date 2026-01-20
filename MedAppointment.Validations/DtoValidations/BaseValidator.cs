namespace MedAppointment.Validations.DtoValidations
{
    public abstract class BaseValidator<TModel> : AbstractValidator<TModel>
    {
        private static readonly char[] AllowedClassifierNameSymbols = { '-', '_', '.' };
        private static readonly char[] AllowedClassifierDescriptionSymbols = { '-', '_', '.', ',', ':', ';', '(', ')', '/', '&', '+', '\'' };
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
        protected static bool BeValidBase64(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            var normalized = s.Trim();
            if (normalized.Length % 4 != 0)
                return false;

            try
            {
                Convert.FromBase64String(normalized);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        protected static bool BeValidClassifierName(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            foreach (var c in s)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (!AllowedClassifierNameSymbols.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        protected static bool BeValidClassifierDescription(string? s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;

            foreach (var c in s)
            {
                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (!AllowedClassifierDescriptionSymbols.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }


    }
}
