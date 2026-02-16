using System.Globalization;

namespace MedAppointment.Logics.Extensions
{
    public static class StringExtension
    {
        public static string ToASCIIFromUnicode(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // 1) Unicode-u parçala (ə, ö, ñ və s. → əsas hərf + diakritik)
            var normalized = value.Normalize(NormalizationForm.FormD);

            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var category = Char.GetUnicodeCategory(c);

                // 2) Diakritik işarələri at (accent, umlaut və s.)
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    // 3) Yalnız ASCII simvollar saxla
                    if (c <= 127)
                        sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
