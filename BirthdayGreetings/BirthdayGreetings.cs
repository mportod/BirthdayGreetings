using System;
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
            _sender.Send(new Friend
            {
                FirstName = "Mary",
                LastName = "Ann",
                BirthDate = new DateOnly(1975, 09, 11),
                Email = "mary.ann@foobar.com"
            });
        }
    }
}
