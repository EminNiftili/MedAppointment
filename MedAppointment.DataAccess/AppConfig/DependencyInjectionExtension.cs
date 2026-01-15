namespace MedAppointment.DataAccess.AppConfig
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMedAppointmentDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddUnifOfWorks(services);

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMeetRepository, MeetRepository>();

            services.AddScoped<IOrganizationUserRepository, OrganizationUserRepository>();

            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<ITraditionalUserRepository, TraditionalUserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDayPlanRepository, DayPlanRepository>();
            services.AddScoped<IPeriodPlanRepository, PeriodPlanRepository>();

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
            services.AddScoped<IPeriodRepository, PeriodRepository>();
            services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        }

        private static void AddUnifOfWorks(IServiceCollection services)
        {
            services.AddScoped<IUnitOfClassifier, UnitOfClassifier>();
            services.AddScoped<IUnitOfClient, UnitOfClient>();
            services.AddScoped<IUnitOfCommunication, UnitOfCommunication>();
            services.AddScoped<IUnitOfFile, UnitOfFile>();
            services.AddScoped<IUnitOfPayment, UnitOfPayment>();
            services.AddScoped<IUnitOfSecurity, UnitOfSecurity>();
            services.AddScoped<IUnitOfService, UnitOfService>();
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MedicalAppointmentContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MedicalAppointmentContext")));
        }
    }
}
