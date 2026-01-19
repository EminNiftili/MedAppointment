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
            services.AddScoped<IPrivateClientInfoService, PrivateClientInfoService>();


            services.AddScoped<IHashService, HashService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ITokenService, JwtBearerTokenService>();
        }
    }
}
