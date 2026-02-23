using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Entities.Classifier;

namespace MedAppointment.Logic.Tests.Magic;

public static class MagicSpecialty
{
    public static SpecialtyEntity EntityOneWithLocalization => new()
    {
        Id = MagicIds.SpecialtyIdOne,
        Key = "CARDIOLOGY",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
        Name = MagicClassifierHelper.ResourceWithTranslation("specialty_name", MagicIds.LanguageIdOne, "Cardiology"),
        Description = MagicClassifierHelper.ResourceWithTranslation("specialty_desc", MagicIds.LanguageIdOne, "Heart specialist")
    };

    public static ClassifierPaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        DescriptionFilter = null
    };

    public static SpecialtyCreateDto ValidCreateDto => new()
    {
        Key = "NEUROLOGY",
        Name = new List<CreateLocalizationDto>(),
        Description = new List<CreateLocalizationDto>()
    };

    public static SpecialtyUpdateDto ValidUpdateDto => new() { Key = "CARDIOLOGY_UPD" };
}
