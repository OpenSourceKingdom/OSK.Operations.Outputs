using System;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// Contextual diagnostic information collected during the output generation process
/// </summary>
public class OutputDiagnostics
{
    #region Variables

    public DateTime StartTime { get; private set; }

    public DateTime CompletionTime { get; private set; }

    public TimeSpan Runtime { get; private set; }

    #endregion

    #region Constructors

    public OutputDiagnostics(DateTime start, DateTime end)
    {
        if (end < start)
        {
            throw new ArgumentException($"Diagnostics start time must be before end time. Start: {start} End: {end}.");
        }

        SetDiagnostics(start, end);
    }
 
    #endregion

    #region Helpers

    internal void SetDiagnostics(DateTime start, DateTime end)
    {
        StartTime = start;
        CompletionTime = end;

        Runtime = end - start;
    }

    #endregion
}
