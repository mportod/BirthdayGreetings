using System;

namespace BirthdayGreetings;

public interface IClock
{
    DateOnly GetCurrentDate();
}