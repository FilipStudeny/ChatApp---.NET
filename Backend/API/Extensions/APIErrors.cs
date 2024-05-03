using System;
using System.Net;

namespace API.Extensions
{
    public class CustomException(string message, HttpStatusCode statusCode) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; } = statusCode;
    }

    public class UserNotFoundException(string message) : CustomException(message, HttpStatusCode.NotFound);
    public class AuthenticationFailedException(string message) : CustomException(message, HttpStatusCode.Unauthorized);
    public class UserException(string message) : CustomException(message, HttpStatusCode.BadRequest);
    public class ServiceException(string message) : CustomException(message, HttpStatusCode.InternalServerError);
    public class InvalidRequest(string message) : CustomException(message, HttpStatusCode.BadRequest);
    public class DatabaseException(string message) : CustomException(message, HttpStatusCode.InternalServerError);
    public class AuthorizationFailedException(string message) : CustomException(message, HttpStatusCode.Forbidden);
    public class DuplicateDataException(string message) : CustomException(message, HttpStatusCode.Conflict);
    public class MissingDataException(string message) : CustomException(message, HttpStatusCode.BadRequest);
}