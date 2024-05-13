using System.Collections.Generic;

namespace BirthdayGreetings;

public interface IFriendsReader
{
    List<Friend> GetFriends();
}