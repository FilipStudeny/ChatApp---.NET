namespace Shared;

public static class Messages
{
    // General error messages
    public const string UserNotFound = "User not found";
    public const string SenderNotFound = "Sender not found";
    public const string ReceiverNotFound = "Receiver not found";
    public const string CouldNotSendFriendRequest = "Couldn't send friend request, try again later.";
    public const string AccountNotFound = "Account not found.";
    public const string WrongEmailOrPassword = "Wrong email or password, try again.";
    public const string UsernameOrEmailInUse = "Couldn't create an account, username or email already in use.";
    public const string PasswordsDoNotMatch = "Passwords do not match, try again.";
    public const string PasswordTooShort = "Password must be longer than 6 symbols.";
    public const string FriendRequestSent = "Friend request sent";
    public const string AccountCreated = "Account created.";

    // Message generators
    public static string GenerateFriendRequestMessage(string senderUsername)
    {
        return $"{senderUsername} has sent you a friend request";
    }
    
    public static string GenerateNotFoundMessage(string what)
    {
        return $"{what} not found";
    }
}