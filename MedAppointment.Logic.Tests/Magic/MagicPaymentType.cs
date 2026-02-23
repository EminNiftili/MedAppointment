using MedAppointment.DataTransferObjects.ClassifierDtos;
using MedAppointment.DataTransferObjects.LocalizationDtos;
using MedAppointment.Entities.Classifier;

namespace MedAppointment.Logic.Tests.Magic;

public static class MagicPaymentType
{
    public const long PaymentTypeIdOne = 6001;

    public static PaymentTypeEntity EntityOneWithLocalization => new()
    {
        Id = PaymentTypeIdOne,
        Key = "CASH",
        NameTextId = MagicIds.NameTextId,
        DescriptionTextId = MagicIds.DescriptionTextId,
        CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        IsDeleted = false,
        Name = MagicClassifierHelper.ResourceWithTranslation("payment_name", MagicIds.LanguageIdOne, "Cash"),
        Description = MagicClassifierHelper.ResourceWithTranslation("payment_desc", MagicIds.LanguageIdOne, "Cash payment")
    };

    public static ClassifierPaginationQueryDto ValidPaginationQuery => new()
    {
        PageNumber = 1,
        PageSize = 10,
        NameFilter = null,
        DescriptionFilter = null
    };

    public static PaymentTypeCreateDto ValidCreateDto => new()
    {
        Key = "CARD",
        Name = new List<CreateLocalizationDto>(),
        Description = new List<CreateLocalizationDto>()
    };

    public static PaymentTypeUpdateDto ValidUpdateDto => new() { Key = "CASH_UPD" };
}
