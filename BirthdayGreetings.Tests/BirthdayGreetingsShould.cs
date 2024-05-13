using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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
                }
            };
            return friends;
        }
    }
}
