using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;
using MedAppointment.Entities.Classifier;

namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Magic (test) data for Language-related unit tests.
/// </summary>
public static class MagicLanguage
{
    public static LanguageEntity EntityOne => new()
    {
        Id = MagicIds.LanguageIdOne,
        Name = "Azərbaycan",
        IsDefault = true,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false
    };

    public static LanguageEntity EntityTwo => new()
    {
        Id = 2002,
        Name = "English",
        IsDefault = false,
        CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false
    };

    public static LanguageCreateDto ValidCreateDto => new()
    {
        Name = "Türkçe",
        IsDefault = false
    };

    public static LanguageUpdateDto ValidUpdateDto => new()
    {
        Name = "Azərbaycan (updated)",
        IsDefault = false
    };

    public static LanguagePaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        IsDefaultFilter = null
    };
}
