using System;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

public readonly record struct OutputCode(OutputStatus Status, DetailCode DetailCode)
{
    #region Static

    public static readonly OutputCode Success = new(OutputStatus.Success, DetailCode.None);

    public static bool TryParse(string code, out OutputCode outputCode)
    {
        outputCode = default;
        if (string.IsNullOrWhiteSpace(code))
        {
            return false;
        }

        var span = code.AsSpan().Trim();
        var separator = span.IndexOf('.');

        var parsedStatus = separator < 0 ? int.TryParse(span, out var status) : int.TryParse(span.Slice(0, separator), out status);               
        if (!parsedStatus)
        {
            return false;
        }

        outputCode = status;

        if (separator < 0)
        {
            return parsedStatus;
        }

        if (int.TryParse(span.Slice(separator + 1), out var detail))
        {
            outputCode = new OutputCode(outputCode.Status, detail);
            return true;
        }

        return false;
    }

    #endregion

    #region Constructors

    public OutputCode(OutputStatus status)
        :this(status, DetailCode.None)
    {

    }

    #endregion

    #region Helpers

    public readonly bool IsSuccessful => Status.Code >= OutputStatus.Success.Code
        && Status.Code < OutputStatus.InvalidRequest.Code;

    #endregion

    #region Operators

    public static implicit operator OutputCode(int code) => new(code);

    #endregion
}
