using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OSK.Operations.Outputs;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs;

public static class PageHelper
{
    /// <summary>
    /// Executes a given Task over the entire data set, using pagination as a mechanism to throttle data retrieval. This will retrieve all the data provided via the page get function.
    /// </summary>
    /// <typeparam name="T">The type of data the page contains</typeparam>
    /// <param name="pageFunc">The task function to get the list data for a given page</param>
    /// <param name="processPageFunc">The task that processeses the page data returned from the page function</param>
    /// <param name="skip">The starting point for the current page</param>
    /// <param name="take">The desired number of items in the data set to take per iteration.</param>
    /// <param name="bailOnFailure">If set, the first failure that occurs will cause the function to stop running. Otherwise, failures will be ignored.</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <returns>An output for the execution of the page information.</returns>
    public static async Task<Output> ExecuteAsync<T>(Func<long, long, CancellationToken, Task<PaginatedOutput<T>>> pageFunc,
        Func<IEnumerable<T>, CancellationToken, Task<Output>> processPageFunc, int skip = 0, int take = 100, bool bailOnFailure = true,
        CancellationToken cancellationToken = default)
    {
        var itemsReceived = 0;
        do
        {
            var pageOutput = await pageFunc(skip, take, cancellationToken);
            skip += take;

            if (!pageOutput.IsSuccessful)
            {
                if (bailOnFailure)
                {
                    return pageOutput;
                }

                continue;
            }

            itemsReceived = pageOutput.Data.Count;

            var processPageOutput = await processPageFunc(pageOutput.Data, cancellationToken);
            if (!processPageOutput.IsSuccessful && bailOnFailure)
            {
                return processPageOutput;
            }
        } while (itemsReceived >= take);

        return Out.Success();
    }

    /// <summary>
    /// Executes a given Task over the entire data set, using pagination as a mechanism to throttle data retrieval. This will retrieve all the data provided via the page get function and return a collection of mapped items.
    /// </summary>
    /// <typeparam name="T">The data type for the page</typeparam>
    /// <typeparam name="U">The projected data type after execution</typeparam>
    /// <param name="pageFunc">The task function to get the list data for a given page</param>
    /// <param name="processPageFunc">The task that processeses the page data returned from the page function</param>
    /// <param name="skip">The starting point for the current page</param>
    /// <param name="take">The desired number of items in the data set to take per iteration.</param>
    /// <param name="bailOnFailure">If set, the first failure that occurs will cause the function to stop running. Otherwise, failures will be ignored.</param>
    /// <param name="cancellationToken">The token to cancel the operation</param>
    /// <returns>An output for the execution of the page information.</returns>
    public static async Task<Output<ICollection<U>>> ExecuteAsync<T, U>(Func<long, long, CancellationToken, Task<PaginatedOutput<T>>> pageFunc,
        Func<IEnumerable<T>, CancellationToken, Task<Output<ICollection<U>>>> processPageFunc, int skip = 0, int take = 100, bool bailOnFailure = true,
        CancellationToken cancellationToken = default)
    {
        List<U> results = [];

        var itemsReceived = 0;
        do
        {
            var pageOutput = await pageFunc(skip, take, cancellationToken);
            skip += take;

            if (!pageOutput.IsSuccessful)
            {
                if (bailOnFailure)
                {
                    return pageOutput.As<ICollection<U>>();
                }

                continue;
            }

            itemsReceived = pageOutput.Data.Count;

            var processPageOutput = await processPageFunc(pageOutput.Data, cancellationToken);
            if (!processPageOutput.IsSuccessful && bailOnFailure)
            {
                return processPageOutput;
            }

            results.AddRange(processPageOutput.Data);
        } while (itemsReceived >= take);

        return Out.Success((ICollection<U>)results);
    }
}
