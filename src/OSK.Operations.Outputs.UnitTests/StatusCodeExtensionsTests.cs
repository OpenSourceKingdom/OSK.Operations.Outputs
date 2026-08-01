using System.Net;
using OSK.Operations.Outputs.Models;
using Xunit;

namespace OSK.Operations.Outputs.UnitTests;

public class StatusCodeExtensionsTests
{
    #region ToHttpStatusCode

    [Theory]
    [InlineData(200, HttpStatusCode.OK)]
    [InlineData(201, HttpStatusCode.Created)]
    [InlineData(202, HttpStatusCode.Accepted)]
    [InlineData(204, HttpStatusCode.NoContent)]
    [InlineData(207, HttpStatusCode.MultiStatus)]
    [InlineData(400, HttpStatusCode.BadRequest)]
    [InlineData(401, HttpStatusCode.Unauthorized)]
    [InlineData(403, HttpStatusCode.Forbidden)]
    [InlineData(404, HttpStatusCode.NotFound)]
    [InlineData(409, HttpStatusCode.Conflict)]
    [InlineData(413, HttpStatusCode.RequestEntityTooLarge)]
    [InlineData(414, HttpStatusCode.RequestUriTooLong)]
    [InlineData(415, HttpStatusCode.UnsupportedMediaType)]
    [InlineData(416, HttpStatusCode.RequestedRangeNotSatisfiable)]
    [InlineData(423, HttpStatusCode.Locked)]
    [InlineData(429, HttpStatusCode.TooManyRequests)]
    [InlineData(500, HttpStatusCode.InternalServerError)]
    [InlineData(501, HttpStatusCode.NotImplemented)]
    [InlineData(502, HttpStatusCode.BadGateway)]
    [InlineData(503, HttpStatusCode.ServiceUnavailable)]
    [InlineData(504, HttpStatusCode.GatewayTimeout)]
    [InlineData(507, HttpStatusCode.InsufficientStorage)]
    [InlineData(508, HttpStatusCode.LoopDetected)]
    public void ToHttpStatusCode_MappedStatuses_ReturnsExpectedHttpStatusCode(int statusCode, HttpStatusCode expectedHttpStatus)
    {
        // Arrange
        var outputStatus = new OutputStatus(statusCode);

        // Act
        var result = outputStatus.ToHttpStatusCode();

        // Assert
        Assert.Equal(expectedHttpStatus, result);
    }

    [Fact]
    public void ToHttpStatusCode_UnrecognizedStatus_ReturnsAmbiguous()
    {
        // Arrange
        var unrecognizedStatus = new OutputStatus(0);

        // Act
        var result = unrecognizedStatus.ToHttpStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.Ambiguous, result);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(150)]
    public void ToHttpStatusCode_1xxStatus_ReturnsAmbiguous(int statusCode)
    {
        // Arrange
        var status = new OutputStatus(statusCode);

        // Act
        var result = status.ToHttpStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.Ambiguous, result);
    }

    [Theory]
    [InlineData(300)]
    [InlineData(350)]
    public void ToHttpStatusCode_3xxStatus_ReturnsAmbiguous(int statusCode)
    {
        // Arrange
        var status = new OutputStatus(statusCode);

        // Act
        var result = status.ToHttpStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.Ambiguous, result);
    }

    [Theory]
    [InlineData(418)]
    [InlineData(420)]
    [InlineData(425)]
    public void ToHttpStatusCode_4xxUnrecognizedStatus_ReturnsDefaultClientError(int statusCode)
    {
        // Arrange
        var status = new OutputStatus(statusCode);

        // Act
        var result = status.ToHttpStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result);
    }

    [Theory]
    [InlineData(505)]
    [InlineData(510)]
    public void ToHttpStatusCode_5xxUnrecognizedStatus_ReturnsDefaultServerError(int statusCode)
    {
        // Arrange
        var status = new OutputStatus(statusCode);

        // Act
        var result = status.ToHttpStatusCode();

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, result);
    }

    #endregion

    #region ToOutputStatus

    [Theory]
    [InlineData(HttpStatusCode.OK, 200)]
    [InlineData(HttpStatusCode.Created, 201)]
    [InlineData(HttpStatusCode.Accepted, 202)]
    [InlineData(HttpStatusCode.NoContent, 204)]
    [InlineData(HttpStatusCode.MultiStatus, 207)]
    [InlineData(HttpStatusCode.BadRequest, 400)]
    [InlineData(HttpStatusCode.Unauthorized, 401)]
    [InlineData(HttpStatusCode.Forbidden, 403)]
    [InlineData(HttpStatusCode.NotFound, 404)]
    [InlineData(HttpStatusCode.Conflict, 409)]
    [InlineData(HttpStatusCode.RequestEntityTooLarge, 413)]
    [InlineData(HttpStatusCode.RequestUriTooLong, 414)]
    [InlineData(HttpStatusCode.UnsupportedMediaType, 415)]
    [InlineData(HttpStatusCode.RequestedRangeNotSatisfiable, 416)]
    [InlineData(HttpStatusCode.Locked, 423)]
    [InlineData(HttpStatusCode.TooManyRequests, 429)]
    [InlineData(HttpStatusCode.InternalServerError, 500)]
    [InlineData(HttpStatusCode.NotImplemented, 501)]
    [InlineData(HttpStatusCode.BadGateway, 502)]
    [InlineData(HttpStatusCode.ServiceUnavailable, 503)]
    [InlineData(HttpStatusCode.GatewayTimeout, 504)]
    [InlineData(HttpStatusCode.RequestTimeout, 504)]
    [InlineData(HttpStatusCode.InsufficientStorage, 507)]
    [InlineData(HttpStatusCode.LoopDetected, 508)]
    public void ToOutputStatus_MappedHttpStatuses_ReturnsExpectedOutputStatus(HttpStatusCode httpStatusCode, int expectedStatusCode)
    {
        // Arrange
        var expectedStatus = new OutputStatus(expectedStatusCode);

        // Act
        var result = httpStatusCode.ToOutputStatus();

        // Assert
        Assert.Equal(expectedStatus, result);
    }

    [Theory]
    [InlineData(HttpStatusCode.Continue)]
    [InlineData(HttpStatusCode.SwitchingProtocols)]
    public void ToOutputStatus_1xxHttpStatus_ReturnsUnrecognizedStatus(HttpStatusCode httpStatusCode)
    {
        // Act
        var result = httpStatusCode.ToOutputStatus();

        // Assert
        Assert.Equal(OutputStatus.UnrecognizedStatus, result);
    }

    [Theory]
    [InlineData(HttpStatusCode.Moved)]
    [InlineData(HttpStatusCode.Redirect)]
    public void ToOutputStatus_3xxHttpStatus_ReturnsUnrecognizedStatus(HttpStatusCode httpStatusCode)
    {
        // Act
        var result = httpStatusCode.ToOutputStatus();

        // Assert
        Assert.Equal(OutputStatus.UnrecognizedStatus, result);
    }

    [Theory]
    [InlineData(HttpStatusCode.PaymentRequired)]
    [InlineData(HttpStatusCode.ProxyAuthenticationRequired)]
    public void ToOutputStatus_4xxUnrecognizedHttpStatus_ReturnsDefaultClientError(HttpStatusCode httpStatusCode)
    {
        // Act
        var result = httpStatusCode.ToOutputStatus();

        // Assert
        Assert.Equal(OutputStatus.InvalidRequest, result);
    }

    [Theory]
    [InlineData(HttpStatusCode.HttpVersionNotSupported)]
    public void ToOutputStatus_5xxUnrecognizedHttpStatus_ReturnsDefaultServerError(HttpStatusCode httpStatusCode)
    {
        // Act
        var result = httpStatusCode.ToOutputStatus();

        // Assert
        Assert.Equal(OutputStatus.InternalError, result);
    }

    #endregion
}