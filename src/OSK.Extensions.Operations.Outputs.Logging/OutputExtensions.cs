using Microsoft.Extensions.Logging;
using OSK.Operations.Outputs.Models;
using System;

namespace OSK.Extensions.Operations.Outputs.Logging;

public static partial class OutputExtensions
{
    public static void LogOutput(this Output output, ILogger logger)
    {
        if (output.IsSuccessful)
        {
            LogSuccess(logger, output.StatusCode);
            return;
        }

        if (output.ErrorInformation.Exception is null)
        {
            LogErrorInformation(logger, output.StatusCode, output.GetErrorString());
        }
        else
        {
            LogExceptionInformation(logger, output.StatusCode, output.ErrorInformation.Exception);
        }
    }


    [LoggerMessage(eventId: 1, LogLevel.Debug, "Successful output. Status: {outputCode}")]
    private static partial void LogSuccess(ILogger logger, OutputCode outputCode);

    [LoggerMessage(eventId: 2, LogLevel.Error, "Output Failed. Status: {outputCode} Reason: {errorMessage}")]
    private static partial void LogErrorInformation(ILogger logger, OutputCode outputCode, string errorMessage);

    [LoggerMessage(eventId: 3, LogLevel.Critical, "Output Exception. Status: {outputCode}")]
    private static partial void LogExceptionInformation(ILogger logger, OutputCode outputCode, Exception ex);
}
