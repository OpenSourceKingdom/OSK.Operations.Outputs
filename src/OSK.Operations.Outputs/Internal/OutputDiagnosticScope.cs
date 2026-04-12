using System;
using System.Diagnostics;
using System.Threading;
using OSK.Operations.Outputs.Internal;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Internal;

internal class OutputDiagnosticScope : IDiagnosticsScope
{
    #region Static

    private static AsyncLocal<OutputDiagnosticScope> Current = new();

    public static OutputDiagnostics GetDiagnostics()
    {
        if (Current.Value is null)
        {
            return null;
        }

        var endTime = DateTime.Now;
        var diagnostics = new OutputDiagnostics(endTime.Subtract(Current.Value.Stopwatch.Elapsed), endTime);

        Current.Value.Stopwatch.Restart();
        return diagnostics;
    }

    #endregion

    #region Variables

    private readonly OutputDiagnosticScope _previous;

    internal Stopwatch Stopwatch { get; } = Stopwatch.StartNew();

    #endregion

    #region Constructors

    public OutputDiagnosticScope()
    {
        _previous = Current.Value;
        Current.Value = this;
    }

    #endregion

    #region IDiagnosticsScope

    public void Dispose()
    {
        Stopwatch.Stop();
        Current.Value = _previous;
    }

    #endregion
}
