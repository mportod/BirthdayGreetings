using System;

namespace BirthdayGreetings;

public interface ISender
{
    public void Send(string emailTo, string subject, string message);
}