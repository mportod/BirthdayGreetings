using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            var birthdayFriends = GetBirthdayFriends(friends, currentDate);
            foreach (var birthdayFriend in birthdayFriends)
            {   
                _sender.Send(birthdayFriend.Email,
                    HappyBirthdaySubject,
                    string.Format(HappyBirthdayMessage,birthdayFriend.FirstName));
            }

            var reminderFriends = friends.Except(birthdayFriends).ToList();
            foreach (var reminderFriend in reminderFriends)
            {
                foreach (var birthdayFriend in birthdayFriends)
                {
                    var message = GetReminderMessage(reminderFriend.FirstName, birthdayFriend.FirstName, birthdayFriend.LastName);
                    _sender.Send(reminderFriend.Email, ReminderSubject, message);
                }
            }
        }

        private static List<Friend> GetBirthdayFriends(IEnumerable<Friend> friends, DateOnly currentDate)
        {
            return friends.Where(f => f.IsMyBirthday(currentDate)).ToList();
        }

        private static string GetReminderMessage(string name, string birthdayFriendName, string birthdayFriendLastName)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Dear {name},");
            stringBuilder.AppendLine($"Today is {birthdayFriendName} {birthdayFriendLastName}'s birthday.");
            stringBuilder.AppendLine($"Don't forget to send them a message!");
            return stringBuilder.ToString();
        }
    }
}
