using System;
using System.Diagnostics.CodeAnalysis;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// An output that contains a data payload
/// </summary>
/// <typeparam name="TData">The data type in the payload</typeparam>
public class Output<TData> : Output
{
    #region Variables

    [MemberNotNullWhen(true, nameof(IsSuccessful))]
    public TData Data { get; private set; }

    #endregion

    #region Constructors

    public Output(OutputCode statusCode, TData data) 
        : this(statusCode, data, null, null)
    {
    }

    public Output(OutputCode statusCode, TData data, ErrorInformation errorInformation, OriginationSource? originationSource = null) 
        : base(statusCode, errorInformation, originationSource)
    {
        Data = data;

        ValidateProperties();
    }

    #endregion

    #region Api

    public sealed override Output<TType> As<TType>()
    {
        if (StatusCode.IsSuccessful)
        {
            // This cast should only be occurring on error cases
            throw new InvalidOperationException("Unable to cast a typed output that was successful, since value typed data has been set.");
        }

        return base.As<TType>();
    }

    #endregion

    #region Helpers

    protected internal void SetDataInternal(TData data)
    {
        Data = data;
    }

    private void ValidateProperties()
    {
        if (IsSuccessful && Data is null)
        {
            throw new ArgumentException("Value must be provided for successful outputs.");
        }
    }

    #endregion
}
