using System;
using System.Collections.Generic;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

public class PaginatedOutput<TValue>: Output<IReadOnlyList<TValue>>
{
    #region Variables

    /// <summary>
    /// The number of items skipped to get to this page
    /// </summary>
    public long Skip { get; }

    /// <summary>
    /// The number of items taken to get the collection of data on the page.
    /// </summary>
    public long Take { get; }

    /// <summary>
    /// An optional value that represents the total number of data available.
    /// </summary>
    public long? Total { get; }

    #endregion

    #region Constructors

    public PaginatedOutput(OutputCode statusCode, IReadOnlyList<TValue> items, long skip, long take, long? total = null) 
        : base(statusCode, items)
    {
        Skip = skip;
        Take = take;
        Total = null;

        ValidateProperties();
    }

    public PaginatedOutput(OutputCode statusCode, ErrorInformation errorInformation, OriginationSource? originationSource = null) 
        : base(statusCode, null, errorInformation, originationSource)
    {
        ValidateProperties();
    }

    #endregion

    #region Helpers

    private void ValidateProperties()
    {
        if (Skip < 0)
        {
            throw new ArgumentException("Skip must be a non-negative integer.", nameof(Skip));
        }
        if (Take < 0)
        {
            throw new ArgumentException("Take must be a non-negative integer.", nameof(Take));
        }
        if (Total.HasValue && Total.Value < 0)
        {
            throw new ArgumentException("Total must be a non-negative integer.", nameof(Total));
        }
        if (IsSuccessful && Data is null)
        {
            throw new ArgumentException("Value must be provided for successful outputs.");
        }
    }

    #endregion
}
