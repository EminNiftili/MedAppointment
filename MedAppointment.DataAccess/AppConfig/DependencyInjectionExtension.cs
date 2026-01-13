using Microsoft.Extensions.Configuration;

namespace MedAppointment.DataAccess.AppConfig
{
    public static class DependencyInjectionExtension
    {
        public static void AddDataAccess(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            AddUnifOfWorks(serviceProvider);
            AddRepositories(serviceProvider);
            AddDbContext(serviceProvider, configuration);
        }
        private static void AddRepositories(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
        private static void AddUnifOfWorks(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
        private static void AddDbContext(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}
