using System;
using System.Collections.Generic;
using System.Linq;

namespace BirthdayGreetings
{
    public class BirthdayGreetings
    {
        private readonly IClock _clock;
        private readonly IFriendsReader _friendsReader;
        private readonly ISender _sender;

        public BirthdayGreetings(IClock clock, IFriendsReader friendsReader, ISender sender)
        {
            _clock = clock;
            _friendsReader = friendsReader;
            _sender = sender;
        }

        public void Send(DateOnly date)
        {
            var currentDate = _clock.GetCurrentDate();
            var friends = _friendsReader.GetFriends();
            var birthdayFriends = GetBirthdayFriends(friends, currentDate);
            foreach (var birthdayFriend in birthdayFriends)
            {
                _sender.Send(birthdayFriend);
            }
        }

        private static IEnumerable<Friend> GetBirthdayFriends(IEnumerable<Friend> friends, DateOnly currentDate)
        {
            return friends.Where(f => f.IsMyBirthday(currentDate));
        }
    }
}
