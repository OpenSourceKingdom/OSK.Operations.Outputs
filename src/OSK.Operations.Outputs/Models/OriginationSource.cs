using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// A strongly-typed name for an origination of an output.
/// </summary>
public readonly struct OriginationSource
{
    #region Variables

    /// <summary>
    /// The name of the origination source.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// An integer identifier for the origination source. Comparisons prefer this value.
    /// </summary>
    public int? SourceIdentifier { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a new origination source with the specified name.
    /// </summary>
    public OriginationSource(string name)
        : this(name, null)
    { }

    /// <summary>
    /// Creates a new origination source with the specified name, identifier and integration sub-code.
    /// </summary>
    public OriginationSource(string name, int? sourceIdentifier)
    {
        Name = name;
        SourceIdentifier = sourceIdentifier;
    }

    #endregion

    public override string ToString() => $"Origination: {Name}";

    /// <summary>
    /// Implicitly convert a string name to an <see cref="OriginationSource"/> with a null identifier.
    /// </summary>
    public static implicit operator OriginationSource(string name) => new(name);
}
