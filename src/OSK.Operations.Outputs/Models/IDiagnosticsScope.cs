using System;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// Represents a scoped object for diagnostic data collection when using the <see cref="Out"/> static calls. Once disposed, the diagnostic data will no longer be collected.
/// </summary>
public interface IDiagnosticsScope: IDisposable
{
}
