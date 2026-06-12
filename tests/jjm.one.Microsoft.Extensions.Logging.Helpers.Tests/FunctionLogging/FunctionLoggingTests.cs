using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;

namespace jjm.one.Microsoft.Extensions.Logging.Helpers.Tests.FunctionLogging;

/// <summary>
///     Tests for <see cref="FunctionLogging" />.
/// </summary>
public class FunctionLoggingTests
{
    #region private members

    private readonly Mock<ILogger> _logger;

    #endregion

    #region ctors

    public FunctionLoggingTests()
    {
        _logger = new Mock<ILogger>();
        // Enable logging for all levels by default so positive tests don't need individual setup.
        _logger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
    }

    #endregion

    #region LogFctCall — auto-detect overload

    [Fact]
    public void LogFctCall_AutoDetect_DefaultLevel_LogsDebug()
    {
        _logger.Object.LogFctCall();

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Debug),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Function called: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogFctCall_AutoDetect_DefaultLevel_LogsDebug)}")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Theory]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Critical)]
    public void LogFctCall_AutoDetect_CustomLevel_LogsAtThatLevel(LogLevel level)
    {
        _logger.Object.LogFctCall(level);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == level),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogFctCall_AutoDetect_LoggingDisabled_DoesNotLog()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        logger.Object.LogFctCall();

        logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Never);
    }

    [Fact]
    public void LogFctCall_AutoDetect_NullLogger_ThrowsArgumentNullException()
    {
        ILogger? nullLogger = null;
        Assert.Throws<ArgumentNullException>(() => nullLogger!.LogFctCall());
    }

    #endregion

    #region LogFctCall — explicit type/method overload

    [Fact]
    public void LogFctCall_Explicit_DefaultLevel_LogsDebug()
    {
        _logger.Object.LogFctCall(GetType(), MethodBase.GetCurrentMethod());

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Debug),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Function called: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogFctCall_Explicit_DefaultLevel_LogsDebug)}")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Theory]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Critical)]
    public void LogFctCall_Explicit_CustomLevel_LogsAtThatLevel(LogLevel level)
    {
        _logger.Object.LogFctCall(GetType(), MethodBase.GetCurrentMethod(), level);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == level),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogFctCall_Explicit_LoggingDisabled_DoesNotLog()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(false);

        logger.Object.LogFctCall(GetType(), MethodBase.GetCurrentMethod());

        logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Never);
    }

    [Fact]
    public void LogFctCall_Explicit_NullLogger_ThrowsArgumentNullException()
    {
        ILogger? nullLogger = null;
        Assert.Throws<ArgumentNullException>(() => nullLogger!.LogFctCall(GetType(), MethodBase.GetCurrentMethod()));
    }

    [Fact]
    public void LogFctCall_Explicit_NullClassType_LogsNullClassName()
    {
        _logger.Object.LogFctCall(null, MethodBase.GetCurrentMethod());

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Debug),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.StartsWith("Function called: (null) -> ")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogFctCall_Explicit_NullMethodType_LogsNullMethodName()
    {
        _logger.Object.LogFctCall(GetType(), null);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Debug),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.StartsWith($"Function called: {nameof(FunctionLoggingTests)} -> ")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    #endregion

    #region LogExcInFctCall — auto-detect overload

    [Fact]
    public void LogExcInFctCall_AutoDetect_NoMsg_LogsErrorWithoutCustomMsg()
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogExcInFctCall_AutoDetect_NoMsg_LogsErrorWithoutCustomMsg)}")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogExcInFctCall_AutoDetect_WithMsg_AppendsNewlineAndMsg()
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, "TestMSG");

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogExcInFctCall_AutoDetect_WithMsg_AppendsNewlineAndMsg)}\nTestMSG")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void LogExcInFctCall_AutoDetect_NullOrWhitespaceMsg_LogsWithoutCustomMsg(string? msg)
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, msg);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogExcInFctCall_AutoDetect_NullOrWhitespaceMsg_LogsWithoutCustomMsg)}")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Theory]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Critical)]
    public void LogExcInFctCall_AutoDetect_CustomLevel_LogsAtThatLevel(LogLevel level)
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, null, level);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == level),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogExcInFctCall_AutoDetect_LoggingDisabled_DoesNotLog()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Error)).Returns(false);
        var exc = new Exception("Test");

        logger.Object.LogExcInFctCall(exc);

        logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Never);
    }

    [Fact]
    public void LogExcInFctCall_AutoDetect_NullLogger_ThrowsArgumentNullException()
    {
        ILogger? nullLogger = null;
        Assert.Throws<ArgumentNullException>(() => nullLogger!.LogExcInFctCall(new Exception()));
    }

    [Fact]
    public void LogExcInFctCall_AutoDetect_NullException_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _logger.Object.LogExcInFctCall(null!));
    }

    #endregion

    #region LogExcInFctCall — explicit type/method overload

    [Fact]
    public void LogExcInFctCall_Explicit_NoMsg_LogsErrorWithoutCustomMsg()
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod());

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogExcInFctCall_Explicit_NoMsg_LogsErrorWithoutCustomMsg)}")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogExcInFctCall_Explicit_WithMsg_AppendsNewlineAndMsg()
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), "TestMSG");

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogExcInFctCall_Explicit_WithMsg_AppendsNewlineAndMsg)}\nTestMSG")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void LogExcInFctCall_Explicit_NullOrWhitespaceMsg_LogsWithoutCustomMsg(string? msg)
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), msg);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.Equals(
                        $"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                        $"{nameof(LogExcInFctCall_Explicit_NullOrWhitespaceMsg_LogsWithoutCustomMsg)}")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Theory]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    [InlineData(LogLevel.Critical)]
    public void LogExcInFctCall_Explicit_CustomLevel_LogsAtThatLevel(LogLevel level)
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod(), null, level);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == level),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogExcInFctCall_Explicit_LoggingDisabled_DoesNotLog()
    {
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Error)).Returns(false);
        var exc = new Exception("Test");

        logger.Object.LogExcInFctCall(exc, GetType(), MethodBase.GetCurrentMethod());

        logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Never);
    }

    [Fact]
    public void LogExcInFctCall_Explicit_NullLogger_ThrowsArgumentNullException()
    {
        ILogger? nullLogger = null;
        Assert.Throws<ArgumentNullException>(() =>
            nullLogger!.LogExcInFctCall(new Exception(), GetType(), MethodBase.GetCurrentMethod()));
    }

    [Fact]
    public void LogExcInFctCall_Explicit_NullException_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _logger.Object.LogExcInFctCall(null!, GetType(), MethodBase.GetCurrentMethod()));
    }

    [Fact]
    public void LogExcInFctCall_Explicit_NullClassType_LogsNullClassName()
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, null, MethodBase.GetCurrentMethod());

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.StartsWith("Exception thrown in: (null) -> ")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    [Fact]
    public void LogExcInFctCall_Explicit_NullMethodType_LogsNullMethodName()
    {
        var exc = new Exception("Test");

        _logger.Object.LogExcInFctCall(exc, GetType(), null);

        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, _) =>
                    o.ToString()!.StartsWith($"Exception thrown in: {nameof(FunctionLoggingTests)} -> ")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()!),
            Times.Once);
    }

    #endregion
}
