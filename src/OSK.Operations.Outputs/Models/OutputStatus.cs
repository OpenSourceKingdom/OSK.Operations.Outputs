using System.Net;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// A strongly-typed value that represents a function status. Responses may line up with HTTP status codes, but this type is not limited to those codes. It can represent any status code that is relevant to the operation being performed.
/// </summary>
public readonly record struct OutputStatus(int Code)
{
    #region Static

    /// <summary>
    /// Represents an unrecognized status code. This is used when the status code does not match any known or defined status codes in the system.
    /// </summary>
    public static readonly OutputStatus UnrecognizedStatus = new(0);

    // Successful

    /// <summary>
    /// The operation completed successfully
    /// </summary>
    public static readonly OutputStatus Success = new(200);

    /// <summary>
    /// The operation completed successfully and new data was created as a result.
    /// </summary>
    public static readonly OutputStatus Created = new(201);

    /// <summary>
    /// The operation completed successfully, but the data will be further processed by the system.
    /// </summary>
    public static readonly OutputStatus Accepted = new(202);

    /// <summary>
    /// The operation completed successfully, and existing data was updated as a result.
    /// </summary>
    public static readonly OutputStatus Updated = new(203);

    /// <summary>
    /// The operation completed successfully, but there is no content to return in the response.
    /// </summary>
    public static readonly OutputStatus NoContent = new(204);

    /// <summary>
    /// The operation completed, but the response contains multiple status codes for different parts of the operation.
    /// </summary>
    public static readonly OutputStatus MultiStatus = new(207);

    // Data Errors

    /// <summary>
    /// The operation failed due to an invalid request.
    /// </summary>
    public static readonly OutputStatus InvalidRequest = new(400);

    /// <summary>
    /// The operation failed because the user or application is not authenticated.
    /// </summary>
    public static readonly OutputStatus NotAuthenticated = new(401);

    /// <summary>
    /// The operation failed because the user or application does not have sufficient permissions to perform the requested action.
    /// </summary>
    public static readonly OutputStatus InsufficientPermissions = new(403);

    /// <summary>
    /// The operation failed because requested data was not found.
    /// </summary>
    public static readonly OutputStatus DataNotFound = new(404);

    /// <summary>
    /// The operation failed because there was already existing data
    /// </summary>
    public static readonly OutputStatus DuplicateData = new(409);

    /// <summary>
    /// The operation failed because the data provided was too large to process.
    /// </summary>
    public static readonly OutputStatus DataTooLarge = new(413);

    /// <summary>
    /// The operation failed because the requested URI is too long to process.
    /// </summary>
    public static readonly OutputStatus UriTooLong = new(414);

    /// <summary>
    /// THe operation failed because the media type of the request is not supported by the server.
    /// </summary>
    public static readonly OutputStatus MediaTypeNotSupported = new(415);

    /// <summary>
    /// The operation failed because the requested range is not satisfiable or valid.
    /// </summary>
    public static readonly OutputStatus InvalidRange = new(416);

    /// <summary>
    /// The operation failed because a required resource is currently locked and cannot be accessed or modified.
    /// </summary>
    public static readonly OutputStatus Locked = new(423);

    /// <summary>
    /// The operation failed because the user or application has exceeded the allowed rate limit for requests.
    /// </summary>
    public static readonly OutputStatus RateLimited = new(429);

    // Operation Errors

    /// <summary>
    /// The operation failed due to an internal server error.
    /// </summary>
    public static readonly OutputStatus InternalError = new(500);

    /// <summary>
    /// The operation failed because the requested functionality is not implemented.
    /// </summary>
    public static readonly OutputStatus NotImplemented = new(501);

    /// <summary>
    /// The operation failed because the server received an invalid response from an upstream server while acting as a gateway or proxy.
    /// </summary>
    public static readonly OutputStatus BadGateway = new(502);

    /// <summary>
    /// The operation failed because the system needed an external service was unavailable due to temporary overloading or maintenance of the server.
    /// </summary>
    public static readonly OutputStatus ServiceUnavailable = new(503);

    /// <summary>
    /// The operation failed because it did complete within an expected period of time
    /// </summary>
    public static readonly OutputStatus Timeout = new(504);

    /// <summary>
    /// The operation failed because the system does not have enough storage to complete the operation.
    /// </summary>
    public static readonly OutputStatus InsufficientStorage = new(507);

    /// <summary>
    /// The operation failed because a loop was detected while processing the request
    /// </summary>
    public static readonly OutputStatus LoopDetected = new(508);

    #endregion

    #region Operators

    public static implicit operator OutputStatus(HttpStatusCode httpStatusCode) => httpStatusCode.ToOutputStatus();
    public static implicit operator OutputStatus(int code) => new(code);

    public static bool operator ==(OutputStatus left, HttpStatusCode right) => left.Equals(right.ToOutputStatus());
    public static bool operator !=(OutputStatus left, HttpStatusCode right) => !left.Equals(right.ToOutputStatus());

    public static bool operator ==(HttpStatusCode left, OutputStatus right) => (new OutputStatus((int)left)).Equals(right);
    public static bool operator !=(HttpStatusCode left, OutputStatus right) => !(new OutputStatus((int)left)).Equals(right);

    #endregion
}
