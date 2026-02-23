using MedAppointment.Logics.Implementations;
using MedAppointment.Logics.Services.LocalizationServices;

namespace MedAppointment.Logics.AppConfig
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMedAppointmentLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMedAppointmentDataAccess(configuration);
            services.AddMedAppointmentValidation(configuration);
            services.AddAutoMapper(typeof(DependencyInjectionExtension).Assembly);

            AddLogicServices(services);

            return services;
        }

        private static void AddLogicServices(IServiceCollection services)
        {
            services.AddScoped<IClientRegistrationService, ClientRegistrationService>();
            services.AddScoped<IPrivateClientInfoService, PrivateClientInfoService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IDoctorService, DoctorService>();

            services.AddScoped<IDoctorPlanManagerService, DoctorPlanManagerService>();
            services.AddScoped<IDoctorCalendarService, DoctorCalendarService>();

            services.AddScoped<ITimeSlotPaddingStrategy, NoPaddingStrategy>();
            services.AddScoped<ITimeSlotPaddingStrategy, StartOfPeriodPaddingStrategy>();
            services.AddScoped<ITimeSlotPaddingStrategy, EndOfPeriodPaddingStrategy>();
            services.AddScoped<ITimeSlotPaddingStrategy, LinearBetweenOfPeriodPaddingStrategy>();
            services.AddScoped<ITimeSlotPaddingStrategy, CenterBetweenOfPeriodPaddingStrategy>();
            services.AddScoped<ITimeSlotPaddingStrategyResolver, TimeSlotPaddingStrategyResolver>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();

            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IPaymentTypeService, PaymentTypeService>();
            services.AddScoped<IPeriodService, PeriodService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<IPlanPaddingTypeService, PlanPaddingTypeService>();

            services.AddScoped<ILocalizerService, LocalizerService>();


            services.AddScoped<ITranslationLookupService, TranslationLookupService>();
            AddClassifierPaginationExpressionStrategies(services);

            services.AddScoped<IHashService, HashService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ITokenService, JwtBearerTokenService>();
        }

        private static void AddClassifierPaginationExpressionStrategies(IServiceCollection services)
        {
            services.AddScoped<IClassifierFilterExpressionStrategy<SpecialtyEntity, ClassifierPaginationQueryDto>, ClassifierFilterExpressionStrategy<SpecialtyEntity>>();
            services.AddScoped<IClassifierFilterExpressionStrategy<PaymentTypeEntity, ClassifierPaginationQueryDto>, ClassifierFilterExpressionStrategy<PaymentTypeEntity>>();
            services.AddScoped<IClassifierFilterExpressionStrategy<PeriodEntity, PeriodPaginationQueryDto>, PeriodFilterExpressionStrategy>();
            services.AddScoped<IClassifierFilterExpressionStrategy<CurrencyEntity, CurrencyPaginationQueryDto>, CurrencyFilterExpressionStrategy>();
            services.AddScoped<IClassifierFilterExpressionStrategy<PlanPaddingTypeEntity, PlanPaddingTypePaginationQueryDto>, PlanPaddingTypeFilterExpressionStrategy>();
        }
    }
}
