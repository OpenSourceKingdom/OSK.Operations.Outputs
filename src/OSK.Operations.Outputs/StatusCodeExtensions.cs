using OSK.Operations.Outputs.Models;
using System.Net;

namespace OSK.Operations.Outputs;

public static class StatusCodeExtensions
{
    /// <summary>
    /// Maps an <see cref="OutputStatus"/> to the closest corresponding HTTP status code.
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>Not all HTTP status codes are fully supported by an output status and vice versa, in the cases where an equivalent is not found, 
    ///     this will default to the general <see cref="HttpStatusCode.OK"/>, <see cref="HttpStatusCode.BadRequest"/>, and <see cref="HttpStatusCode.InternalServerError"/> related codes.</item>
    /// <item>For 1xx and 3xx status codes, a generic <see cref="HttpStatusCode.Ambiguous"/> will be returned, as these do not have direct equivalents.</item>
    /// <item>This mapping is not exhaustive and may be extended in the future.</item>
    /// </list>
    /// </remarks>
    /// <returns>An equivalent HTTP status code.</returns>
    public static HttpStatusCode ToHttpStatusCode(this OutputStatus status) 
        => status.Code switch
        {
            200 => HttpStatusCode.OK,
            201 => HttpStatusCode.Created,
            202 => HttpStatusCode.Accepted,
            // Update status code does not have a direct equivalent in HttpStatusCode, so we will map it to 200 OK.
            203 => HttpStatusCode.OK,
            204 => HttpStatusCode.NoContent,
            207 => HttpStatusCode.MultiStatus,

            400 => HttpStatusCode.BadRequest,
            401 => HttpStatusCode.Unauthorized,
            403 => HttpStatusCode.Forbidden,
            404 => HttpStatusCode.NotFound,
            409 => HttpStatusCode.Conflict,
            413 => HttpStatusCode.RequestEntityTooLarge,
            414 => HttpStatusCode.RequestUriTooLong,
            415 => HttpStatusCode.UnsupportedMediaType,
            416 => HttpStatusCode.RequestedRangeNotSatisfiable,
            423 => HttpStatusCode.Locked,
            429 => HttpStatusCode.TooManyRequests,

            500 => HttpStatusCode.InternalServerError,
            501 => HttpStatusCode.NotImplemented,
            502 => HttpStatusCode.BadGateway,
            503 => HttpStatusCode.ServiceUnavailable,
            504 => HttpStatusCode.GatewayTimeout,
            507 => HttpStatusCode.InsufficientStorage,
            508 => HttpStatusCode.LoopDetected,

            _ => status.Code >= 500 ? HttpStatusCode.InternalServerError :
                 status.Code >= 400 ? HttpStatusCode.BadRequest :
                 status.Code >= 300 ? HttpStatusCode.Ambiguous :
                 status.Code >= 200 ? HttpStatusCode.OK :
                 HttpStatusCode.Ambiguous
        };

    /// <summary>
    /// Maps an <see cref="HttpStatusCode"/> to the closest <see cref="OutputStatus"/>.
    /// </summary>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>Not all HTTP status codes are fully supported by an output status and vice versa, in the cases where an equivalent is not found, 
    ///     this will default to the general <see cref="OutputStatus.Success"/>, <see cref="OutputStatus.InvalidRequest"/>, and <see cref="OutputStatus.InternalError"/> related codes.</item>
    /// <item>For 1xx and 3xx status codes, a generic <see cref="OutputStatus.UnrecognizedStatus"/> will be returned, as these do not have direct equivalents.</item>
    /// <item>This mapping is not exhaustive and may be extended in the future.</item>
    /// </list>
    /// </remarks>
    /// <returns>An equivalent output status.</returns>
    public static OutputStatus ToOutputStatus(this HttpStatusCode statusCode) 
        => statusCode switch
        {
            HttpStatusCode.OK => OutputStatus.Success,
            HttpStatusCode.Created => OutputStatus.Created,
            HttpStatusCode.Accepted => OutputStatus.Accepted,
            HttpStatusCode.NoContent => OutputStatus.NoContent,
            HttpStatusCode.MultiStatus => OutputStatus.MultiStatus,
            HttpStatusCode.NonAuthoritativeInformation => OutputStatus.Success,

            HttpStatusCode.BadRequest => OutputStatus.InvalidRequest,
            HttpStatusCode.Unauthorized => OutputStatus.NotAuthenticated,
            HttpStatusCode.Forbidden => OutputStatus.InsufficientPermissions,
            HttpStatusCode.NotFound => OutputStatus.DataNotFound,
            HttpStatusCode.Conflict => OutputStatus.DuplicateData,
            HttpStatusCode.RequestEntityTooLarge => OutputStatus.DataTooLarge,
            HttpStatusCode.RequestUriTooLong => OutputStatus.UriTooLong,
            HttpStatusCode.UnsupportedMediaType => OutputStatus.MediaTypeNotSupported,
            HttpStatusCode.RequestedRangeNotSatisfiable => OutputStatus.InvalidRange,
            HttpStatusCode.Locked => OutputStatus.Locked,
            HttpStatusCode.TooManyRequests => OutputStatus.RateLimited,

            HttpStatusCode.InternalServerError => OutputStatus.InternalError,
            HttpStatusCode.NotImplemented => OutputStatus.NotImplemented,
            HttpStatusCode.BadGateway => OutputStatus.BadGateway,
            HttpStatusCode.ServiceUnavailable => OutputStatus.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout => OutputStatus.Timeout,
            HttpStatusCode.RequestTimeout => OutputStatus.Timeout,
            HttpStatusCode.InsufficientStorage => OutputStatus.InsufficientStorage,
            HttpStatusCode.LoopDetected => OutputStatus.LoopDetected,

            _ => statusCode >= HttpStatusCode.InternalServerError ? OutputStatus.InternalError :
                 statusCode >= HttpStatusCode.BadRequest ? OutputStatus.InvalidRequest :
                 statusCode >= HttpStatusCode.MultipleChoices ? OutputStatus.UnrecognizedStatus :
                 statusCode >= HttpStatusCode.OK ? OutputStatus.Success :
                 OutputStatus.UnrecognizedStatus
        };
}
