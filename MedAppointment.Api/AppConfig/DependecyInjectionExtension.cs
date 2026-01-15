namespace MedAppointment.Api.AppConfig
{
    public static class DependecyInjectionExtension
    {
        public static void AddMedAppointmentApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMedAppointmentLogic(configuration);
            AddSwagger(services);
        }

        private static void AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new() { Title = "GiriPet API", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter 'Bearer' [space] and then your token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                opt.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        jwtSecurityScheme,
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
