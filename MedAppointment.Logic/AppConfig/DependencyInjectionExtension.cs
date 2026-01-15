namespace MedAppointment.Logics.AppConfig
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMedAppointmentLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMedAppointmentDataAccess(configuration);
            services.AddMedAppointmentValidation(configuration);

            AddLogicServices(services);

            return services;
        }

        private static void AddLogicServices(IServiceCollection services)
        {
            services.AddScoped<IClientRegistrationService, ClientRegistrationService>();
        }
    }
}
