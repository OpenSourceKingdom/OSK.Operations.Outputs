using System;
using System.Collections.Generic;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

public abstract class MultiOutputBase<TOutput>: Output<IReadOnlyList<TOutput>>
    where TOutput: Output
{
    #region Variables

    private readonly List<TOutput> _outputs = [];

    private bool _createdAggregateError = false;

    #endregion

    #region Constructors

    public MultiOutputBase()
        : base(new OutputCode(OutputStatus.NoContent), [])
    {
        SetDataInternal(_outputs);
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Include a collection of outputs into the MultiOutput
    /// </summary>
    /// <param name="outputs">The collection of outputs to include</param>
    public void With(params TOutput[] outputs)
    {
        foreach (var output in outputs)
        {
            With(output);
        }
    }

    /// <summary>
    /// Includes an output into the MultiOutput
    /// </summary>
    /// <param name="output">The output to include</param>
    /// <exception cref="ArgumentNullException">Output can not be null</exception>
    public void With(TOutput output)
    {
        if (output is null)
        {
            throw new ArgumentNullException(nameof(output));
        }

        if (_outputs.Count is 0)
        {
            SetStatusCodeInternal(output.StatusCode);
            SetErrorInformationInternal(output.ErrorInformation);
        }
        else
        {
            if (output.StatusCode.Status != StatusCode.Status && StatusCode.Status != OutputStatus.MultiStatus)
            {
                SetStatusCodeInternal(new OutputCode(OutputStatus.MultiStatus));
            }
            if (output.ErrorInformation is not null && !_createdAggregateError)
            {
                SetErrorInformationInternal(new ErrorInformation(["One or more outputs exist with error information, please check data for related error information."]));
                _createdAggregateError = true;
            }
        }

        if (output.Diagnostics is not null)
        {
            if (Diagnostics is null)
            {
                Diagnostics = new OutputDiagnostics(output.Diagnostics.StartTime, output.Diagnostics.CompletionTime);
            }
            else
            {
                var minStart = output.Diagnostics.StartTime < Diagnostics.StartTime
                    ? output.Diagnostics.StartTime : Diagnostics.StartTime;
                var maxStart = output.Diagnostics.CompletionTime > Diagnostics.CompletionTime
                    ? output.Diagnostics.CompletionTime : Diagnostics.CompletionTime;

                Diagnostics.SetDiagnostics(minStart, maxStart);
            }
        }

        _outputs.Add(output);
    }

    #endregion
}
