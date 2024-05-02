namespace API.Extensions
{
    public class UserNotFoundException(string message) : Exception(message) { }
    public class AuthenticationFailedException(string message) : Exception(message) { }
    public class UserException(string message) : Exception(message) { }
    public class ServiceException(string message) : Exception(message) { }
    public class InvalidRequest(string message) : Exception(message) { }
    public class DatabaseException(string message) : Exception(message) { }
    public class AuthorizationFailedException(string message) : Exception(message) { }
    public class DuplicateDataException(string message) : Exception(message) { }
    public class MissingDataException(string message) : Exception(message) { }


}
