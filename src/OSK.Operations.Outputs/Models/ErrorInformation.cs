using System;
using System.Collections.Generic;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// Represents error related data for a specific output
/// </summary>
public class ErrorInformation
{
    #region Variables

    public Exception Exception { get; }

    public IReadOnlyList<string> Messages { get; }

    #endregion

    #region Constructors

    public ErrorInformation(Exception exception)
    {
        Exception = exception;
        Messages = exception?.Message is null ? [] : [exception.Message];
    }

    public ErrorInformation(string[] messages)
    {
        Messages = messages;
    }

    #endregion
}
