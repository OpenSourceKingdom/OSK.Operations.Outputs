using System;
using System.Collections.Generic;
using System.Linq;
using OSK.Operations.Outputs.Internal;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs;

public static class Out
{
    #region Diagnostics

    public static IDiagnosticsScope CreateDiagnosticScope()
        => new OutputDiagnosticScope();

    #endregion

    #region Success

    public static Output Success()
        => Status(OutputStatus.Success);

    public static Output<TData> Success<TData>(TData data)
        => Status(OutputStatus.Success, data);

    #endregion

    #region Created

    public static Output Created()
        => Status(OutputStatus.Created);

    public static Output<TData> Created<TData>(TData data)
        => Status(OutputStatus.Created, data);

    #endregion

    #region Generic Status

    public static Output Status(OutputStatus status)
        => new(new OutputCode(status))
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    public static Output<TData> Status<TData>(OutputStatus status, TData data)
        => new(new OutputCode(status), data)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    #endregion

    #region DataNotFound

    public static Output DataNotFound(string message = "Not Found", OriginationSource? originationSource = null)
        => Error(new OutputCode(OutputStatus.DataNotFound), [message], originationSource);

    public static Output DataNotFound(DetailCode detailCode, string message = "Not Found", OriginationSource? originationSource = null)
        => Error(new OutputCode(OutputStatus.DataNotFound, detailCode), [message], originationSource);

    public static Output<TData> DataNotFound<TData>(string message = "Not Found", OriginationSource? originationSource = null)
        => Error<TData>(new OutputCode(OutputStatus.DataNotFound), [message], originationSource);

    public static Output<TData> DataNotFound<TData>(DetailCode detailCode, string message = "Not Found", OriginationSource? originationSource = null)
        => Error<TData>(new OutputCode(OutputStatus.DataNotFound, detailCode), [message], originationSource);

    #endregion

    #region InvalidRequest

    public static Output InvalidRequest(string message = "Invalid Request", OriginationSource? originationSource = null)
        => Error(new OutputCode(OutputStatus.InvalidRequest), [message], originationSource);

    public static Output InvalidRequest(DetailCode detailCode, string message = "Invalid Request", OriginationSource? originationSource = null)
        => Error(new OutputCode(OutputStatus.InvalidRequest, detailCode), [message], originationSource);

    public static Output<TData> InvalidRequest<TData>(string message = "Invalid Request", OriginationSource? originationSource = null)
        => Error<TData>(new OutputCode(OutputStatus.InvalidRequest), [message], originationSource);

    public static Output<TData> InvalidRequest<TData>(DetailCode detailCode, string message = "Invalid Request", OriginationSource? originationSource = null)
        => Error<TData>(new OutputCode(OutputStatus.InvalidRequest, detailCode), [message], originationSource);

    #endregion

    #region DuplicateData

    public static Output DuplicateData(string message = "Duplicate", OriginationSource? originationSource = null)
        => Error(new OutputCode(OutputStatus.DuplicateData, DetailCode.None), [message], originationSource);

    public static Output DuplicateData(DetailCode detailCode, string message = "Duplicate", OriginationSource? originationSource = null)
        => Error(new OutputCode(OutputStatus.DuplicateData, detailCode), [message], originationSource);

    public static Output<TData> DuplicateData<TData>(string message = "Duplicate", OriginationSource? originationSource = null)
        => Error<TData>(new OutputCode(OutputStatus.DuplicateData, DetailCode.None), [message], originationSource);

    public static Output<TData> DuplicateData<TData>(DetailCode detailCode, string message = "Duplicate", OriginationSource? originationSource = null)
        => Error<TData>(new OutputCode(OutputStatus.DuplicateData, detailCode), [message], originationSource);

    #endregion

    #region Error

    public static Output Error(OutputStatus status, string message, OriginationSource? originationSource = null)
        => Error(new OutputCode(status), [message], originationSource);

    public static Output Error<TData>(OutputStatus status, TData data, string message, OriginationSource? originationSource = null)
        => Error(new OutputCode(status), data, [message], originationSource);
    
    public static Output Error(OutputStatus status, IEnumerable<string> errors, OriginationSource? originationSource = null)
        => Error(new OutputCode(status), errors, originationSource);

    public static Output Error<TData>(OutputStatus status, TData data, IEnumerable<string> errors, OriginationSource? originationSource = null)
        => Error(new OutputCode(status), data, errors, originationSource);

    public static Output Error(OutputCode code, string message, OriginationSource? originationSource = null)
        => Error(code, [message], originationSource);

    public static Output Error<TData>(OutputCode code, TData data, string message, OriginationSource? originationSource = null)
        => Error(code, data, [message], originationSource);

    public static Output Error(OutputCode code, IEnumerable<string> errors, OriginationSource? originationSource = null)
        => new(code, new ErrorInformation(errors is null ? [] : [.. errors]), originationSource)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    public static Output<TData> Error<TData>(OutputCode code, IEnumerable<string> errors, OriginationSource? originationSource = null)
        => Error<TData>(code, default, errors, originationSource);

    public static Output<TData> Error<TData>(OutputCode code, TData data, IEnumerable<string> errors, OriginationSource? originationSource = null)
        => new(code, data, new ErrorInformation(errors is null ? [] : [.. errors]), originationSource)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    #endregion

    #region Fault

    public static Output Fault(Exception exception, OriginationSource? originationSource = null)
        => new(new OutputCode(OutputStatus.InternalError, DetailCode.None), new ErrorInformation(exception), originationSource)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    public static Output<TData> Fault<TData>(Exception exception, OriginationSource? originationSource = null)
        => new(new OutputCode(OutputStatus.InternalError, DetailCode.None), default, new ErrorInformation(exception), originationSource)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    #endregion

    #region Multi Output

    public static MultiOutput Multiple(IEnumerable<Output> outputs)
    {
        var multiOutput = new MultiOutput();
        foreach (var output in outputs)
        {
            multiOutput.With(output);
        }

        return multiOutput;
    }

    public static MultiOutput<TData> Multiple<TData>(IEnumerable<Output<TData>> outputs)
    {
        var multiOutput = new MultiOutput<TData>();
        foreach (var output in outputs)
        {
            multiOutput.With(output);
        }

        return multiOutput;
    }

    #endregion

    #region Paginated Output

    public static PaginatedOutput<TData> Page<TData>(IEnumerable<TData> data, long skip, long take, long? total = null)
        => new(new OutputCode(OutputStatus.Success), data?.ToList(), skip, take, total)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    public static PaginatedOutput<TData> PageError<TData>(OutputCode code, IEnumerable<string> errors, OriginationSource? originationSource = null)
        => new(code, new ErrorInformation(errors is null ? [] : errors.ToArray()), originationSource)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    public static PaginatedOutput<TData> PageFault<TData>(Exception exception, OriginationSource? originationSource = null)
        => new(new OutputCode(OutputStatus.InternalError), new ErrorInformation(exception), originationSource)
        {
            Diagnostics = OutputDiagnosticScope.GetDiagnostics()
        };

    #endregion
}
