using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void not_send_message_to_John_Doe_when_today_is_not_his_birthday()
        {
            var todayDate = new DateOnly(2024, 05, 10);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);
            
            sut.Send(todayDate);

            sender.DidNotReceive().Send(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void not_send_message_when_no_one_is_on_birthday()
        {
            var todayDate = new DateOnly(2024, 01, 01);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);

            sut.Send(todayDate);

            sender.DidNotReceive().Send(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
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
            var expectedSubject = "Birthday Reminder";
            
            sut.Send(todayDate);

            var expectedMessage = GetReminderMessage("John", "Mary", "Ann");
            sender.Received().Send("john.doe@mail.com", expectedSubject, expectedMessage);
            expectedMessage = GetReminderMessage("Angela", "Mary", "Ann");
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
