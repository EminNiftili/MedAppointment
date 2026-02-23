#region Test Frameworks
global using Xunit;
global using NSubstitute;
#endregion

#region Logic & Patterns
global using MedAppointment.Logics.Implementations.ClassifierServices;
global using MedAppointment.Logics.Services.ClassifierServices;
global using MedAppointment.Logics.Services.LocalizationServices;
global using MedAppointment.Logics.Patterns.ResultPattern;
global using MedAppointment.Logics.CustomExpressions.ClassifierExpressions;
#endregion

#region DataAccess
global using MedAppointment.DataAccess.UnitOfWorks;
global using MedAppointment.DataAccess.Repositories.Classifier;
#endregion

#region DTOs
global using MedAppointment.DataTransferObjects.ClassifierDtos;
global using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;
global using MedAppointment.DataTransferObjects.Enums;
#endregion

#region Entities
global using MedAppointment.Entities.Classifier;
#endregion

#region Test Helpers & Magic
global using MedAppointment.Logic.Tests.Magic;
global using MedAppointment.Logic.Tests.TestHelpers;
#endregion

#region System
global using System.Net;
global using System.Linq.Expressions;
global using FluentValidation;
global using Microsoft.Extensions.Logging;
#endregion
