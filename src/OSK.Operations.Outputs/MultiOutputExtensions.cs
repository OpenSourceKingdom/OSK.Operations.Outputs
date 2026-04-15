using OSK.Operations.Outputs.Models;
using System.Collections.Generic;
using System.Linq;

namespace OSK.Operations.Outputs;

public static class MultiOutputExtensions
{
    /// <summary>
    /// Returns outputs from the output collection that are associated to error statuses
    /// </summary>
    /// <param name="output">The <see cref="MultiOutput"/> to get outputs from</param>
    /// <returns>The collection of error outputs</returns>
    public static IEnumerable<Output> GetErrorOutputs(this MultiOutput output)
        => output.Data.Where(o => !o.IsSuccessful);
}
