using System.Collections.Generic;

namespace BirthdayGreetings;

public interface IFriendsReader
{
    IEnumerable<Friend> GetFriends();
}