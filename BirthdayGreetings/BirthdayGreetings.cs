using System;
using System.Linq;

namespace BirthdayGreetings
{
    public class BirthdayGreetings
    {
        private const string HappyBirthdaySubject = "Happy Birthday!";
        private const string HappyBirthdayMessage = "Happy Birthday, dear {0}!";
        private const string ReminderSubject = "Birthday Reminder";
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

            var birthdayFriends = friends.Where(f => f.IsMyBirthday(currentDate)).ToList();
            var reminderFriends = friends.Except(birthdayFriends).ToList();

            foreach (var birthdayFriend in birthdayFriends)
            {
                _sender.Send(birthdayFriend.Email, HappyBirthdaySubject, string.Format(HappyBirthdayMessage, birthdayFriend.FirstName));
            }

            var birthdayFriendsNames = birthdayFriends.Select(f => $"{f.FirstName} {f.LastName}").ToList();
            foreach (var reminderFriend in reminderFriends)
            {
                var reminderMessage = $"Dear {reminderFriend.FirstName},\nToday is {string.Join(", ", birthdayFriendsNames)}\nDon't forget to send them a message!";
                _sender.Send(reminderFriend.Email, ReminderSubject, reminderMessage);
            }
        }
    }
}
