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
global using MedAppointment.DataAccess.AppConfig;
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

#region
global using MedAppointment.Logics.Patterns.ResultPattern;
#endregion

#region Validations
global using MedAppointment.Validations.AppConfig;
global using FluentValidation;
global using FluentValidation.Results;
#endregion

#region Mapper
global using AutoMapper;
#endregion

#region System Usings
global using Microsoft.Extensions.Logging;
global using System.Net;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using System.Security.Cryptography;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
#endregion

#region Data Transfer Objects
global using MedAppointment.DataTransferObjects.Enums;
global using MedAppointment.DataTransferObjects.CredentialDtos;
global using MedAppointment.DataTransferObjects.UserDtos;
global using MedAppointment.DataTransferObjects.ClassifierDtos;
global using MedAppointment.DataTransferObjects.PaginationDtos;
global using MedAppointment.DataTransferObjects.DoctorDtos;
#endregion


#region Abstract Logic Services
global using MedAppointment.Logics.Services.ClientServices;
global using MedAppointment.Logics.Services.SecurityServices;
global using MedAppointment.Logics.Services.ClassifierServices;
#endregion

#region Implementation Logic Services
global using MedAppointment.Logics.Implementations.ClientServices;
global using MedAppointment.Logics.Implementations.SecurityServices;
global using MedAppointment.Logics.Implementations.ClassifierServices;
#endregion
