using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace FictionalEnterprise.Shared.ExtensionMethods;

public static class ILoggerExtensions
{
    public static void LogInformationClassNamePrepended(this ILogger logger, string logText)
    {
        // Get the name of the method that called this method
        var callerTypeName = new StackFrame(1)
        .GetMethod()?
        .DeclaringType?
        .Name;

        logger.LogInformation($"{callerTypeName} - {logText}");
    }

    public static void LogErrorClassNamePrepended(this ILogger logger, string logText)
    {
        // Get the name of the method that called this method
        var callerTypeName = new StackFrame(1)
        .GetMethod()?
        .DeclaringType?
        .Name;

        logger.LogError($"{callerTypeName} - {logText}");
    }

    public static void LogWarningClassNamePrepended(this ILogger logger, string logText)
    {
        // Get the name of the method that called this method
        var callerTypeName = new StackFrame(1)
        .GetMethod()?
        .DeclaringType?
        .Name;

        logger.LogWarning($"{callerTypeName} - {logText}");
    }
}