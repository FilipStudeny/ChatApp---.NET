namespace API.Extensions
{
    public class CustomException(string message) : Exception(message);
    public class UserNotFoundException(string message) : CustomException(message) { }
    public class AuthenticationFailedException(string message) : CustomException(message) { }
    public class UserException(string message) : CustomException(message) { }
    public class ServiceException(string message) : CustomException(message) { }
    public class InvalidRequest(string message) : CustomException(message) { }
    public class DatabaseException(string message) : CustomException(message) { }
    public class AuthorizationFailedException(string message) : CustomException(message) { }
    public class DuplicateDataException(string message) : CustomException(message) { }
    public class MissingDataException(string message) : CustomException(message) { }


}
