#region Patterns
global using MedAppointment.Logics.Patterns.ResultPattern;
#endregion


#region System Usings
global using MedAppointment.Logics.AppConfig;
#endregion

#region Api Usings
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.OpenApi.Models;
global using Microsoft.AspNetCore.Authorization;
#endregion

#region Logger Usings
global using Serilog;
#endregion


#region Data Transfer Objects
global using MedAppointment.DataTransferObjects.UserDtos;
global using MedAppointment.DataTransferObjects.ClassifierDtos;
#endregion


#region Abstract Logic Services
global using MedAppointment.Logics.Services.ClientServices;
global using MedAppointment.Logics.Services.ClassifierServices;
#endregion
