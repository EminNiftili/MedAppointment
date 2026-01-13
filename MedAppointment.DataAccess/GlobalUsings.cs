#region Entities
global using MedAppointment.Entities;
global using MedAppointment.Entities.Classifier;
global using MedAppointment.Entities.Client;
global using MedAppointment.Entities.Communication;
global using MedAppointment.Entities.Composition;
global using MedAppointment.Entities.File;
global using MedAppointment.Entities.Payment;
global using MedAppointment.Entities.Security;
global using MedAppointment.Entities.Service;
#endregion

#region Abstract DataAccess
global using MedAppointment.DataAccess.UnitOfWorks;
global using MedAppointment.DataAccess.Repositories;
global using MedAppointment.DataAccess.Repositories.Classifier;
global using MedAppointment.DataAccess.Repositories.Client;
global using MedAppointment.DataAccess.Repositories.Communication;
global using MedAppointment.DataAccess.Repositories.Composition;
global using MedAppointment.DataAccess.Repositories.File;
global using MedAppointment.DataAccess.Repositories.Payment;
global using MedAppointment.DataAccess.Repositories.Security;
global using MedAppointment.DataAccess.Repositories.Service;
#endregion


#region Implementation Entity Framework DataAccess
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Contexts.MedicalAppointment;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Classifier;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Client;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Communication;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Composition;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.File;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Payment;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Security;
global using MedAppointment.DataAccess.Implementations.EntityFramework.SqlServer.Configurations.Service;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Classifier;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Client;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Communication;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Compositon;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.File;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Payment;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Security;
global using MedAppointment.DataAccess.Implementations.EntityFramework.Repositories.Service;
global using MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks;
#endregion


#region Entity Framework
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
#endregion

#region Microsoft Extensions
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
#endregion

#region System Usings
global using System.Linq.Expressions;
#endregion
