using System.Reflection;

namespace MedAppointment.Logic.Tests.TestHelpers;

/// <summary>
/// Creates internal Logic service implementations via reflection so tests can run without InternalsVisibleTo.
/// </summary>
public static class ServiceReflectionHelper
{
    private static readonly Assembly LogicAssembly = typeof(MedAppointment.Logics.Services.ClassifierServices.ICurrencyService).Assembly;

    /// <summary>
    /// Creates ILogger for an internal service type using NullLogger so NSubstitute does not need to proxy internal types.
    /// </summary>
    public static ILogger CreateLoggerFor(string implementationTypeName)
    {
        var serviceType = LogicAssembly.GetType(implementationTypeName)
            ?? throw new InvalidOperationException($"Type not found: {implementationTypeName}");
        var abstractionsAssembly = typeof(ILogger).Assembly;
        var nullLoggerOpenType = abstractionsAssembly.GetTypes()
            .FirstOrDefault(t => t.IsGenericTypeDefinition && t.Name == "NullLogger`1");
        var nullLoggerType = nullLoggerOpenType?.MakeGenericType(serviceType)
            ?? throw new InvalidOperationException("NullLogger<T> type not found");
        var instanceField = nullLoggerType.GetField("Instance", BindingFlags.Public | BindingFlags.Static);
        var instanceProp = nullLoggerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
        var instance = instanceField?.GetValue(null) ?? instanceProp?.GetValue(null)
            ?? throw new InvalidOperationException("NullLogger<T>.Instance not found or returned null");
        return (ILogger)instance;
    }

    public static TInterface CreateClassifierService<TInterface>(string implementationTypeName, params object?[] constructorArgs)
        where TInterface : class
    {
        return CreateService<TInterface>(implementationTypeName, constructorArgs);
    }

    /// <summary>
    /// Creates any internal Logic service by type name (Calendar, PlanManager, Localization, etc.).
    /// </summary>
    public static TInterface CreateService<TInterface>(string implementationTypeName, params object?[] constructorArgs)
        where TInterface : class
    {
        var type = LogicAssembly.GetType(implementationTypeName)
            ?? throw new InvalidOperationException($"Type not found: {implementationTypeName}");
        var instance = Activator.CreateInstance(type, constructorArgs)
            ?? throw new InvalidOperationException($"Failed to create instance of {implementationTypeName}");
        return (TInterface)instance;
    }
}
