namespace MedAppointment.Logic.Tests.Magic;

/// <summary>
/// Magic data for DoctorCalendar and PlanManager unit tests.
/// </summary>
public static class MagicCalendar
{
    /// <summary>
    /// Monday of a week for plan creation.
    /// </summary>
    public static DateTime MondayStart => new DateTime(2024, 6, 3, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Valid CreateDayPlansFromSchemaDto with 7 day schemas (1-7), all closed for simplicity.
    /// </summary>
    public static CreateDayPlansFromSchemaDto ValidCreateDayPlansFromSchemaDto => new()
    {
        WeeklySchema = new WeeklySchemaDto
        {
            DoctorId = DoctorIdOne,
            Id = 1,
            Name = "Default",
            ColorHex = "#000000FF",
            DaySchemas = Enumerable.Range(1, 7).Select(d => new DaySchemaDto
            {
                Id = d,
                WeeklySchemaId = 1,
                SpecialtyId = MagicIds.SpecialtyIdOne,
                PeriodId = MagicPeriod.PeriodIdOne,
                PeriodTimeMinutes = 30,
                PlanPaddingTypeId = null,
                DayOfWeek = (byte)d,
                OpenTime = TimeSpan.FromHours(9),
                PeriodCount = 0,
                IsClosed = true,
                IsOnlineService = false,
                IsOnSiteService = true,
                DayBreaks = new List<DayBreakDto>()
            }).ToList()
        },
        StartDate = MondayStart,
        CurrencyId = MagicIds.CurrencyIdOne,
        PricePerPeriod = 50m
    };

    /// <summary>
    /// Valid DoctorSchemaCreateDto with 7 day schemas.
    /// </summary>
    public static DoctorSchemaCreateDto ValidDoctorSchemaCreateDto => new()
    {
        DoctorId = DoctorIdOne,
        Name = "Default Week",
        ColorHex = "#000000FF",
        DaySchemas = Enumerable.Range(1, 7).Select(d => new DaySchemaCreateDto
        {
            SpecialtyId = MagicIds.SpecialtyIdOne,
            PeriodId = MagicPeriod.PeriodIdOne,
            PlanPaddingTypeId = null,
            DayOfWeek = (byte)d,
            OpenTime = TimeSpan.FromHours(9),
            PeriodCount = 2,
            IsClosed = false,
            IsOnlineService = true,
            IsOnSiteService = true,
            DayBreaks = new List<DayBreakCreateDto>()
        }).ToList()
    };


    public const long DoctorIdOne = 8001;
    public const long DayPlanIdOne = 8002;
    public const long PeriodPlanIdOne = 8003;
    public const long NonExistentId = 99999;

    public static DoctorCalendarWeekQueryDto ValidWeekQuery => new()
    {
        DoctorId = DoctorIdOne,
        WeekStartDate = new DateTime(2024, 6, 3, 0, 0, 0, DateTimeKind.Utc) // Monday
    };

    public static EditDayPlanDto ValidEditDayPlanDto => new()
    {
        DayPlanId = DayPlanIdOne,
        DoctorId = DoctorIdOne,
        SpecialtyId = MagicIds.SpecialtyIdOne,
        IsClosed = false
    };

    public static EditPeriodPlanDto ValidEditPeriodPlanDto => new()
    {
        PeriodPlanId = PeriodPlanIdOne,
        DoctorId = DoctorIdOne,
        PeriodStart = TimeSpan.FromHours(9),
        PeriodStop = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
        IsOnlineService = true,
        IsOnSiteService = true,
        PricePerPeriod = 50m,
        CurrencyId = MagicIds.CurrencyIdOne,
        IsBusy = false
    };

    public static DoctorEntity DoctorOne => new()
    {
        Id = DoctorIdOne,
        IsDeleted = false,
        IsConfirm = true,
        CreatedAt = DateTime.UtcNow
    };

    public static DayPlanEntity DayPlanOne => new()
    {
        Id = DayPlanIdOne,
        DoctorId = DoctorIdOne,
        SpecialtyId = MagicIds.SpecialtyIdOne,
        PeriodId = MagicPeriod.PeriodIdOne,
        BelongDate = ValidWeekQuery.WeekStartDate,
        DayOfWeek = 1,
        OpenTime = TimeSpan.FromHours(9),
        CloseTime = TimeSpan.FromHours(17),
        IsClosed = false,
        IsDeleted = false,
        CreatedAt = DateTime.UtcNow
    };

    public static PeriodPlanEntity PeriodPlanOne => new()
    {
        Id = PeriodPlanIdOne,
        DayPlanId = DayPlanIdOne,
        PeriodStart = TimeSpan.FromHours(9),
        PeriodStop = TimeSpan.FromHours(9).Add(TimeSpan.FromMinutes(30)),
        IsBusy = false,
        IsDeleted = false,
        DayPlan = DayPlanOne,
        CurrencyId = MagicIds.CurrencyIdOne,
        Currency = new CurrencyEntity { Id = MagicIds.CurrencyIdOne, Key = "AZN" },
        PricePerPeriod = 50m
    };
}
