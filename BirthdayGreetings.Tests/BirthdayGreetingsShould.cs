using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BirthdayGreetings.Tests
{
    public class BirthdayGreetingsShould
    {
        private IClock clock = default!;
        private IFriendsReader friendsReader = default!;
        private ISender sender = default!;
        private BirthdayGreetings sut = default!;

        [SetUp]
        public void Setup()
        {
            clock = Substitute.For<IClock>();
            friendsReader = Substitute.For<IFriendsReader>();
            sender = Substitute.For<ISender>();
            sut = new BirthdayGreetings(clock,friendsReader,sender);
        }

        [Test]
        public void send_message_to_Mary_Ann_on_his_birthday()
        {
            var subject = "Happy Birthday!";
            var message = "Happy Birthday, dear Mary!";
            var todayDate = new DateOnly(2024,09,11);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);

            sut.Send(todayDate);

            sender.Received().Send("mary.ann@mail.com", subject,message);
        }
        
        [Test]
        public void send_message_to_John_Doe_on_his_birthday()
        {
            var subject = "Happy Birthday!";
            var message = "Happy Birthday, dear John!";
            var todayDate = new DateOnly(2024, 10, 08);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);
            
            sut.Send(todayDate);

            sender.Received().Send("john.doe@mail.com", subject, message);
        }

        [Test]
        public void send_birthday_message_to_people_born_on_february_twenty_ninth()
        {
            var subject = "Happy Birthday!";
            var message = "Happy Birthday, dear Angela!";
            var todayDate = new DateOnly(2024, 02, 28);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);
           
            sut.Send(todayDate);

            sender.Received().Send("angela.korvell@mail.com", subject, message);
        }

        [Test]
        public void send_reminder_message_to_friends_when_mary_is_on_birthday()
        {
            var friends = GetFriends();
            var todayDate = new DateOnly(2024, 09, 11);
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);
            var birthdayFriendsNames = new List<string> { "Mary Ann", "Peter Dagan" };

            sut.Send(todayDate);

            var expectedSubject = "Birthday Reminder";
            var expectedMessage = GetReminderMessage("John", birthdayFriendsNames);
            sender.Received().Send("john.doe@mail.com", expectedSubject, expectedMessage);
            expectedMessage = GetReminderMessage("Angela", birthdayFriendsNames);
            sender.Received().Send("angela.korvell@mail.com", expectedSubject, expectedMessage);
        }

        private static List<Friend> GetFriends()
        {
            var friends = new List<Friend>
            {
                new Friend
                {
                    FirstName = "Mary",
                    LastName = "Ann",
                    BirthDate = new DateOnly(1975, 09, 11),
                    Email = "mary.ann@mail.com"
                },
                new Friend
                {
                    FirstName = "Peter",
                    LastName = "Dagan",
                    BirthDate = new DateOnly(1987, 09, 11),
                    Email = "peter.dagan@mail.com"
                },
                new Friend
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateOnly(1982, 10, 08),
                    Email = "john.doe@mail.com"
                },
                new Friend
                {
                    FirstName = "Angela",
                    LastName = "Korvell",
                    BirthDate = new DateOnly(2000, 02, 29),
                    Email = "angela.korvell@mail.com"
                }
            };
            return friends;
        }

        private static string GetReminderMessage(string reminderFriendName, List<string> birthdayFriendsNames)
        {
            return $"Dear {reminderFriendName},\nToday is {string.Join(", ", birthdayFriendsNames)}\nDon't forget to send them a message!";
        }
    }
}
