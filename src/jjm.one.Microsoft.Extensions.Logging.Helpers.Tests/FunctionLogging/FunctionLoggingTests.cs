using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;

namespace jjm.one.Microsoft.Extensions.Logging.Helpers.Tests.FunctionLogging;

/// <summary>
///     This class contains the tests for the <see cref="FunctionLogging" /> class.
/// </summary>
public class FunctionLoggingTests
{
    #region private members

    private readonly Mock<ILogger> _logger;

    #endregion

    #region ctors

    /// <summary>
    ///     The default constructor of the <see cref="FunctionLoggingTests" /> class.
    /// </summary>
    public FunctionLoggingTests()
    {
        _logger = new Mock<ILogger>();
    }

    #endregion

    #region tests

    /// <summary>
    ///     1. test of the LogFctCall function.
    /// </summary>
    [Fact]
    public void LogFctCallTest1()
    {
        // arrange
        _logger.Setup(x => x.Log(LogLevel.Debug, 0, It.IsAny<object>(),
            It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()!)).Verifiable();

        // act
        _logger.Object.LogFctCall();

        // assert
        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Debug),
                0,
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Equals($"Function called: {nameof(FunctionLoggingTests)} -> " +
                                         $"{nameof(LogFctCallTest1)}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     2. test of the LogFctCall function.
    /// </summary>
    [Fact]
    public void LogFctCallTest2()
    {
        // arrange
        _logger.Setup(x => x.Log(LogLevel.Debug, 0, It.IsAny<object>(),
            It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()!)).Verifiable();

        // act 
        _logger.Object.LogFctCall(GetType(), MethodBase.GetCurrentMethod());

        // assert
        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Debug),
                0,
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Equals($"Function called: {nameof(FunctionLoggingTests)} -> " +
                                         $"{nameof(LogFctCallTest2)}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     1. test of the LogExcInFctCall function.
    /// </summary>
    [Fact]
    public void LogExcInFctCallTest1()
    {
        // arrange
        var exc = new Exception("Test");
        _logger.Setup(x => x.Log(LogLevel.Error, 0, It.IsAny<object>(),
            It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()!)).Verifiable();

        // act 
        _logger.Object.LogExcInFctCall(exc);

        // assert
        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                0,
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Equals($"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                                         $"{nameof(LogExcInFctCallTest1)}")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     2. test of the LogExcInFctCall function.
    /// </summary>
    [Fact]
    public void LogExcInFctCallTest2()
    {
        // arrange
        var exc = new Exception("Test");
        _logger.Setup(x => x.Log(LogLevel.Error, 0, It.IsAny<object>(),
            It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()!)).Verifiable();

        // act 
        _logger.Object.LogExcInFctCall(exc, "TestMSG");

        // assert
        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                0,
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Equals($"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                                         $"{nameof(LogExcInFctCallTest2)}\nTestMSG")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     3. test of the LogExcInFctCall function.
    /// </summary>
    [Fact]
    public void LogExcInFctCallTest3()
    {
        // arrange
        var exc = new Exception("Test");
        _logger.Setup(x => x.Log(LogLevel.Error, 0, It.IsAny<object>(),
            It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()!)).Verifiable();

        // act 
        _logger.Object.LogExcInFctCall(exc, GetType(),
            MethodBase.GetCurrentMethod());

        // assert
        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                0,
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Equals($"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                                         $"{nameof(LogExcInFctCallTest3)}")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    /// <summary>
    ///     4. test of the LogExcInFctCall function.
    /// </summary>
    [Fact]
    public void LogExcInFctCallTest4()
    {
        // arrange
        var exc = new Exception("Test");
        _logger.Setup(x => x.Log(LogLevel.Error, 0, It.IsAny<object>(),
            It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()!)).Verifiable();

        // act 
        _logger.Object.LogExcInFctCall(exc, GetType(),
            MethodBase.GetCurrentMethod(), "TestMSG");

        // assert
        _logger.Verify(x => x.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                0,
                It.Is<It.IsAnyType>((o, t) =>
                    o.ToString()!.Equals($"Exception thrown in: {nameof(FunctionLoggingTests)} -> " +
                                         $"{nameof(LogExcInFctCallTest4)}\nTestMSG")),
                It.Is<Exception>(e => e == exc),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    #endregion
}