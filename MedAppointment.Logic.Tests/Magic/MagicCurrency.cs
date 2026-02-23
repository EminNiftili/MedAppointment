using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;
using MedAppointment.Entities.Classifier;
using MedAppointment.Entities.Localization;

namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Magic (test) data for Currency-related unit tests.
/// </summary>
public static class MagicCurrency
{
    /// <summary>
    /// Currency entity with Name and Description navigation populated (for MapCurrency in GetById).
    /// </summary>
    public static CurrencyEntity EntityOneWithLocalization => new()
    {
        Id = MagicIds.CurrencyIdOne,
        Key = "AZN",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        Coefficent = 1.0m,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
        Name = new ResourceEntity
        {
            Key = "currency_name",
            Translations = new List<TranslationEntity> { new() { LanguageId = MagicIds.LanguageIdOne, Text = "AZN" } }
        },
        Description = new ResourceEntity
        {
            Key = "currency_desc",
            Translations = new List<TranslationEntity> { new() { LanguageId = MagicIds.LanguageIdOne, Text = "Azerbaijani Manat" } }
        }
    };

    public static CurrencyEntity EntityOne => new()
    {
        Id = MagicIds.CurrencyIdOne,
        Key = "AZN",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        Coefficent = 1.0m,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false
    };

    public static CurrencyEntity EntityTwo => new()
    {
        Id = MagicIds.CurrencyIdTwo,
        Key = "USD",
        NameTextId = MagicIds.NameTextId + 1,
        DescriptionTextId = MagicIds.DescriptionTextId + 1,
        Coefficent = 1.7m,
        CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false
    };

    public static CurrencyCreateDto ValidCreateDto => new()
    {
        Key = "EUR",
        Coefficent = 1.85m,
        Name = new List<CreateLocalizationDto>(),
        Description = new List<CreateLocalizationDto>()
    };

    public static CurrencyUpdateDto ValidUpdateDto => new()
    {
        Key = "AZN_UPD",
        Coefficent = 1.05m
    };

    public static CurrencyPaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        DescriptionFilter = null,
        CoefficentMin = null,
        CoefficentMax = null
    };

    public static CurrencyPaginationQueryDto PaginationQueryWithCoefficientFilter => new()
    {
        PageNumber = 1,
        PageSize = 5,
        CoefficentMin = 1.0m,
        CoefficentMax = 2.0m
    };
}
