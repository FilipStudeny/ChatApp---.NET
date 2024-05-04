using Shared.Models;

namespace Shared.Builders;

public class FriendBuilder
{
    private Friend _friend = new Friend();

    public FriendBuilder WithUser(User user)
    {
        _friend.Id = user.Id;
        _friend.Username = user.Username;
        return this;
    }

    public Friend Build()
    {
        return _friend;
    }
}