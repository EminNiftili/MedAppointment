using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Entities.Classifier;

namespace MedAppointment.Logic.Tests.Magic;

public static class MagicPeriod
{
    public const long PeriodIdOne = 4001;

    public static PeriodEntity EntityOneWithLocalization => new()
    {
        Id = PeriodIdOne,
        Key = "30MIN",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        PeriodTime = 30,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
        Name = MagicClassifierHelper.ResourceWithTranslation("period_name", MagicIds.LanguageIdOne, "30 min"),
        Description = MagicClassifierHelper.ResourceWithTranslation("period_desc", MagicIds.LanguageIdOne, "30 minute slot")
    };

    public static PeriodPaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        DescriptionFilter = null,
        PeriodTime = null
    };

    public static PeriodCreateDto ValidCreateDto => new()
    {
        Key = "60MIN",
        PeriodTime = 60,
        Name = new List<CreateLocalizationDto>(),
        Description = new List<CreateLocalizationDto>()
    };

    public static PeriodUpdateDto ValidUpdateDto => new() { Key = "30MIN_UPD", PeriodTime = 45 };
}
