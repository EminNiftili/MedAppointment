#region Test Frameworks
global using Xunit;
global using NSubstitute;
#endregion

#region Logic & Patterns
global using MedAppointment.Logics.Implementations.LocalizationServices;
global using MedAppointment.Logics.Services.ClassifierServices;
global using MedAppointment.Logics.Services.LocalizationServices;
global using MedAppointment.Logics.Patterns.ResultPattern;
global using MedAppointment.Logics.CustomExpressions.ClassifierExpressions;
#endregion

#region DataAccess
global using MedAppointment.DataAccess.UnitOfWorks;
global using MedAppointment.DataAccess.Implementations.EntityFramework.UnitOfWorks;
global using MedAppointment.DataAccess.Repositories.Classifier;
global using MedAppointment.DataAccess.Repositories.Service;
global using MedAppointment.DataAccess.Repositories.Client;
global using MedAppointment.DataAccess.Repositories.Localization;
global using MedAppointment.DataAccess.Repositories.Doctor;
global using MedAppointment.DataAccess.Repositories.Composition;
global using MedAppointment.DataAccess.Repositories.Security;
#endregion

#region DTOs
global using MedAppointment.DataTransferObjects.ClassifierDtos;
global using MedAppointment.DataTransferObjects.PaginationDtos;
global using MedAppointment.DataTransferObjects.PaginationDtos.ClassifierPagination;
global using MedAppointment.DataTransferObjects.PaginationDtos.UserPagination;
global using MedAppointment.DataTransferObjects.Enums;
global using MedAppointment.DataTransferObjects.DoctorDtos;
global using MedAppointment.DataTransferObjects.LocalizationDtos;
global using MedAppointment.DataTransferObjects.UserDtos;
global using MedAppointment.DataTransferObjects.CredentialDtos;
#endregion

#region Entities
global using MedAppointment.Entities.Classifier;
global using MedAppointment.Entities.Service;
global using MedAppointment.Entities.Client;
global using MedAppointment.Entities.Composition;
global using MedAppointment.Entities.Localization;
global using MedAppointment.Entities.Doctor;
global using MedAppointment.Entities.Security;
#endregion

#region Logic Services (Calendar, PlanManager, Schedule, Client, Security)
global using MedAppointment.Logics.Services.CalendarServices;
global using MedAppointment.Logics.Services.PlanManagerServices;
global using MedAppointment.Logics.Services.ScheduleServices;
global using MedAppointment.Logics.Services.ClientServices;
global using MedAppointment.Logics.Services.SecurityServices;
#endregion

#region Mapper
global using AutoMapper;
#endregion

#region Test Helpers & Magic
global using MedAppointment.Logic.Tests.Magic;
global using MedAppointment.Logic.Tests.TestHelpers;
#endregion

#region System
global using System.Net;
global using System.Linq.Expressions;
global using System.Security.Claims;
global using FluentValidation;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
#endregion
