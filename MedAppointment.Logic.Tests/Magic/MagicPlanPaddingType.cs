using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Entities.Classifier;

namespace MedAppointment.Logic.Tests.Magic;

public static class MagicPlanPaddingType
{
    public const long PlanPaddingTypeIdOne = 7001;

    public static PlanPaddingTypeEntity EntityOneWithLocalization => new()
    {
        Id = PlanPaddingTypeIdOne,
        Key = "START",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        PaddingPosition = (byte)PlanPaddingPosition.StartOfPeriod,
        PaddingTime = 5,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
        Name = MagicClassifierHelper.ResourceWithTranslation("padding_name", MagicIds.LanguageIdOne, "Start"),
        Description = MagicClassifierHelper.ResourceWithTranslation("padding_desc", MagicIds.LanguageIdOne, "Start of period")
    };

    public static PlanPaddingTypePaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        DescriptionFilter = null,
        PaddingPosition = null,
        PaddingTime = null
    };

    public static PlanPaddingTypeCreateDto ValidCreateDto => new()
    {
        Key = "END",
        PaddingPosition = PlanPaddingPosition.EndOfPeriod,
        PaddingTime = 10,
        Name = new List<CreateLocalizationDto>(),
        Description = new List<CreateLocalizationDto>()
    };

    public static PlanPaddingTypeUpdateDto ValidUpdateDto => new()
    {
        Key = "START_UPD",
        PaddingPosition = PlanPaddingPosition.StartOfPeriod,
        PaddingTime = 15
    };
}
