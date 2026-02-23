namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Centralized IDs for unit tests. Use these constants instead of magic numbers.
/// </summary>
public static class MagicIds
{
    public const long CurrencyIdOne = 1001;
    public const long CurrencyIdTwo = 1002;
    public const long CurrencyIdNonExistent = 99999;

    public const long LanguageIdOne = 2001;
    public const long LanguageIdNonExistent = 99998;

    public const long SpecialtyIdOne = 3001;
    public const long SpecialtyIdNonExistent = 99997;

    public const long NameTextId = 5001;
    public const long DescriptionTextId = 5002;

    /// <summary>
    /// Generic non-existent ID for entities (e.g. WeeklySchema).
    /// </summary>
    public const long NonExistentId = 99990;
}
