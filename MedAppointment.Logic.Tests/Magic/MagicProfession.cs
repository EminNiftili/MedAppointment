using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Entities.Classifier;

namespace MedAppointment.Logic.Tests.Magic;

public static class MagicProfession
{
    public static ProfessionEntity EntityOneWithLocalization => new()
    {
        Id = MagicIds.ProfessionIdOne,
        Key = "DOCTOR",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
        Name = MagicClassifierHelper.ResourceWithTranslation("profession_name", MagicIds.LanguageIdOne, "Doctor"),
        Description = MagicClassifierHelper.ResourceWithTranslation("profession_desc", MagicIds.LanguageIdOne, "Medical doctor")
    };

    public static ClassifierPaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        DescriptionFilter = null
    };

    public static ProfessionCreateDto ValidCreateDto => new()
    {
        Key = "NURSE",
        Name = new List<CreateLocalizationDto>(),
        Description = new List<CreateLocalizationDto>()
    };

    public static ProfessionUpdateDto ValidUpdateDto => new() { Key = "DOCTOR_UPD" };
}
