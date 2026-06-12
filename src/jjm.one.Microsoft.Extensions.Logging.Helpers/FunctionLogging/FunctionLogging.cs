using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace jjm.one.Microsoft.Extensions.Logging.Helpers;

/// <summary>
///     Static class for all function logging helper extensions.
/// </summary>
public static class FunctionLogging
{
    /// <summary>
    ///     Log a function/method call. The class and method names are resolved at compile time via
    ///     caller-info attributes — zero allocation, no stack walk.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="level">The logging level. Default is <see cref="LogLevel.Debug" />.</param>
    /// <param name="memberName">Compiler-supplied calling member name.</param>
    /// <param name="sourceFilePath">Compiler-supplied calling source file path.</param>
    public static void LogFctCall(
        this ILogger logger,
        LogLevel level = LogLevel.Debug,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "")
    {
        if (logger is null) throw new ArgumentNullException(nameof(logger));

        if (!logger.IsEnabled(level))
            return;

        logger.Log(level, "Function called: {ClassName} -> {FctName}",
            Path.GetFileNameWithoutExtension(sourceFilePath), memberName);
    }

    /// <summary>
    ///     Log a function/method call.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="classType">The <see cref="Type" /> of the calling class.</param>
    /// <param name="methodType">The <see cref="MethodBase" /> of the calling function/method.</param>
    /// <param name="level">The logging level. Default is <see cref="LogLevel.Debug" />.</param>
    public static void LogFctCall(
        this ILogger logger,
        Type? classType,
        MethodBase? methodType,
        LogLevel level = LogLevel.Debug)
    {
        if (logger is null) throw new ArgumentNullException(nameof(logger));

        if (!logger.IsEnabled(level))
            return;

        logger.Log(level, "Function called: {ClassName} -> {FctName}",
            classType?.Name, methodType?.Name);
    }

    /// <summary>
    ///     Log an exception within a function/method call. The class and method names are resolved at
    ///     compile time via caller-info attributes — zero allocation, no stack walk.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exc">The exception.</param>
    /// <param name="msg">An optional context message appended to the log entry.</param>
    /// <param name="level">The logging level. Default is <see cref="LogLevel.Error" />.</param>
    /// <param name="memberName">Compiler-supplied calling member name.</param>
    /// <param name="sourceFilePath">Compiler-supplied calling source file path.</param>
    public static void LogExcInFctCall(
        this ILogger logger,
        Exception exc,
        string? msg = null,
        LogLevel level = LogLevel.Error,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "")
    {
        if (logger is null) throw new ArgumentNullException(nameof(logger));
        if (exc is null) throw new ArgumentNullException(nameof(exc));

        if (!logger.IsEnabled(level))
            return;

        var customMsg = string.IsNullOrWhiteSpace(msg) ? string.Empty : "\n" + msg;

        logger.Log(level, exc, "Exception thrown in: {ClassName} -> {FctName}{CustomMsg}",
            Path.GetFileNameWithoutExtension(sourceFilePath), memberName, customMsg);
    }

    /// <summary>
    ///     Log an exception within a function/method call.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="exc">The exception.</param>
    /// <param name="classType">The <see cref="Type" /> of the calling class.</param>
    /// <param name="methodType">The <see cref="MethodBase" /> of the calling function/method.</param>
    /// <param name="msg">An optional context message appended to the log entry.</param>
    /// <param name="level">The logging level. Default is <see cref="LogLevel.Error" />.</param>
    public static void LogExcInFctCall(
        this ILogger logger,
        Exception exc,
        Type? classType,
        MethodBase? methodType,
        string? msg = null,
        LogLevel level = LogLevel.Error)
    {
        if (logger is null) throw new ArgumentNullException(nameof(logger));
        if (exc is null) throw new ArgumentNullException(nameof(exc));

        if (!logger.IsEnabled(level))
            return;

        var customMsg = string.IsNullOrWhiteSpace(msg) ? string.Empty : "\n" + msg;

        logger.Log(level, exc, "Exception thrown in: {ClassName} -> {FctName}{CustomMsg}",
            classType?.Name, methodType?.Name, customMsg);
    }
}
