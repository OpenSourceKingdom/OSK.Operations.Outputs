using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// A strongly-typed value that represents a function output detail code. This is meant to be used in conjunction with an <see cref="OutputStatus"/> to provide more detailed information about an output response. 
/// The detail code provides more granular information about the output that is not captured by the status code.
/// </summary>
/// <param name="Value">The integer value representing the detail code.</param>
public readonly record struct DetailCode(int Value)
{
    #region static

    public static readonly DetailCode None = new(0);

    #endregion

    #region Operators

    public static implicit operator DetailCode(int code) => new(code);

    #endregion
}
