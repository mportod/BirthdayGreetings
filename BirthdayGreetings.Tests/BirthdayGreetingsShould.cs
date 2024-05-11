using System;
using System.Collections.Generic;
using FluentAssertions.Common;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using NSubstitute;
using NUnit.Framework;

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
            var todayDate = new DateOnly(2024,09,11);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);

            sut.Send(todayDate);

            sender.Received().Send(Arg.Is<Friend>(f => f.FirstName == "Mary" && f.LastName == "Ann"));
        }
        
        [Test]
        public void send_message_to_John_Doe_on_his_birthday()
        {
            var todayDate = new DateOnly(2024, 10, 08);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);

            sut.Send(todayDate);

            sender.Received().Send(Arg.Is<Friend>(f => f.FirstName == "John" && f.LastName == "Doe"));
        }

        [Test]
        public void not_send_message_to_John_Doe_when_today_is_not_his_birthday()
        {
            var todayDate = new DateOnly(2024, 05, 10);
            var friends = GetFriends();
            clock.GetCurrentDate().Returns(todayDate);
            friendsReader.GetFriends().Returns(friends);

            sut.Send(todayDate);

            sender.DidNotReceive().Send(Arg.Is<Friend>(f => f.FirstName == "John" && f.LastName == "Doe"));
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
                    Email = "mary.ann@foobar.com"
                },
                new Friend
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = new DateOnly(1982, 10, 08),
                    Email = "john.doe@foobar.com"
                }
            };
            return friends;
        }
    }
}
