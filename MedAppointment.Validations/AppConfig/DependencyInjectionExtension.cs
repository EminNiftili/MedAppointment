namespace MedAppointment.Validations.AppConfig
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddMedAppointmentValidation(this IServiceCollection services, IConfiguration configuration)
        {
            AddFluentValidation(services);

            return services;
        }

        private static void AddFluentValidation(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<AssemblyReader>();
        }
    }
}
