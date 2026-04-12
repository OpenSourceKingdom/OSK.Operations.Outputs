using System.Net;
using OSK.Operations.Outputs.Models;

namespace OSK.Operations.Outputs.Models;

/// <summary>
/// A strongly-typed value that represents a function status. 
/// </summary>
public readonly record struct OutputStatus(int Code)
{
    #region Static

    public static readonly OutputStatus UnrecognizedStatus = new(0);

    // Successful
    public static readonly OutputStatus Success = new(200);
    public static readonly OutputStatus Created = new(201);
    public static readonly OutputStatus Accepted = new(202);
    public static readonly OutputStatus NoContent = new(204);
    public static readonly OutputStatus MultiStatus = new(207);

    // Data Errors
    public static readonly OutputStatus InvalidRequest = new(400);
    public static readonly OutputStatus NotAuthenticated = new(401);
    public static readonly OutputStatus InsufficientPermissions = new(403);
    public static readonly OutputStatus DataNotFound = new(404);
    public static readonly OutputStatus DuplicateData = new(409);
    public static readonly OutputStatus DataTooLarge = new(413);
    public static readonly OutputStatus UriTooLong = new(414);
    public static readonly OutputStatus MediaTypeNotSupported = new(415);
    public static readonly OutputStatus InvalidRange = new(416);
    public static readonly OutputStatus Locked = new(423);
    public static readonly OutputStatus RateLimited = new(429);

    // Operation Errors
    public static readonly OutputStatus InternalError = new(500);
    public static readonly OutputStatus NotImplemented = new(501);
    public static readonly OutputStatus BadGateway = new(502);
    public static readonly OutputStatus ServiceUnavailable = new(503);
    public static readonly OutputStatus Timeout = new(504);
    public static readonly OutputStatus InsufficientStorage = new(507);
    public static readonly OutputStatus LoopDetected = new(508);

    #endregion

    #region Operators

    public static implicit operator OutputStatus(HttpStatusCode httpStatusCode) => new((int)httpStatusCode);
    public static implicit operator OutputStatus(int code) => new(code);

    public static bool operator ==(OutputStatus left, HttpStatusCode right) => left.Equals((OutputStatus)right);
    public static bool operator !=(OutputStatus left, HttpStatusCode right) => !left.Equals((OutputStatus)right);

    public static bool operator ==(HttpStatusCode left, OutputStatus right) => ((OutputStatus)left).Equals(right);
    public static bool operator !=(HttpStatusCode left, OutputStatus right) => !((OutputStatus)left).Equals(right);

    #endregion
}
