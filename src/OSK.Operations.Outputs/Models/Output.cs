using System;
using System.Diagnostics.CodeAnalysis;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// The standard output for a function. Provides contextual information and data related to the output for the caller
/// </summary>
public class Output
{
    #region Variables

    public bool IsSuccessful => StatusCode.IsSuccessful;

    public OutputCode StatusCode { get; private set; }


    [MemberNotNullWhen(false, nameof(IsSuccessful))]
    public ErrorInformation ErrorInformation { get; private set; }

    public OriginationSource? OriginationSource { get; private set; }

    public OutputDiagnostics Diagnostics { get; internal set; }

    #endregion

    #region Constructors

    public Output(OutputCode statusCode)
        : this(statusCode, null)
    {
    }

    public Output(OutputCode statusCode, ErrorInformation errorInformation, OriginationSource? originationSource = null)
    {
        StatusCode = statusCode;
        ErrorInformation = errorInformation;
        OriginationSource = originationSource;

        ValidateProperties();
    }

    #endregion

    #region Api

    /// <summary>
    /// This will generate an error string based on the error information; i.e. consilidating multiple errors into a single error string, exception message into one, etc. 
    /// </summary>
    /// <param name="separator">The separator to use when concatenating multiple error messages</param>
    /// <param name="includeStackTrace">Whether to include the stack trace in the error string</param>
    /// <returns>A single consolidated error string for the output</returns>
    public string GetErrorString(string separator = "\n", bool includeStackTrace = false)
    {
        if (ErrorInformation?.Messages is null)
        {
            return string.Empty;
        }

        var content = includeStackTrace && ErrorInformation.Exception is not null
            ? [.. ErrorInformation.Messages, ErrorInformation.Exception.StackTrace]
            : ErrorInformation.Messages;
        return string.Join(separator, content);
    }

    /// <summary>
    /// Sets the origination source for the output and returns the updated instance.
    /// </summary>
    /// <param name="originationSource">The origination source to associate with the output. This value determines the source context for the output instance.</param>
    /// <returns>The current instance of <see cref="Output"/> with the specified origination source set.</returns>
    public Output WithOrigination(OriginationSource originationSource)
    {
        OriginationSource = originationSource;
        return this;
    }

    /// <summary>
    /// Attempts to cast the output to another type. This may fail if the output has data attached.
    /// </summary>
    /// <typeparam name="TValue">The desired type to simulate</typeparam>
    /// <returns>An output that contains the same contextual information as the current output</returns>
    public virtual Output<TValue> As<TValue>()
        => new(StatusCode, default, ErrorInformation, OriginationSource);

    /// <summary>
    /// Attempts to cast the output to another type. This may fail if the output has data attached.
    /// </summary>
    /// <typeparam name="TPage">The desired type to simulate</typeparam>
    /// <returns>A paginated output that contains the same contextual information as the current output</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public PaginatedOutput<TPage> AsPage<TPage>()
    {
        if (IsSuccessful)
        {
            // This cast should only be occurring on error cases
            throw new InvalidOperationException("Unable to cast a typed output that was successful, since value typed data has been set.");
        }

        return new PaginatedOutput<TPage>(StatusCode, ErrorInformation, OriginationSource);
    }

    #endregion

    #region Helpers

    protected internal void SetStatusCodeInternal(OutputCode code)
    {
        StatusCode = code;
    }

    protected internal void SetErrorInformationInternal(ErrorInformation errorInformation)
    {
        ErrorInformation = errorInformation;
    }

    private void ValidateProperties()
    {
        if (IsSuccessful && ErrorInformation is not null)
        {
            throw new ArgumentException("Error information should be null for successful outputs.");
        }
        if (!IsSuccessful && ErrorInformation is null)
        {
            throw new ArgumentException("Error information must be provided for unsuccessful outputs.");
        }
    }

    #endregion
}
