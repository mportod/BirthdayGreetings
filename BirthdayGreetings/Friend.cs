using System;

namespace BirthdayGreetings;

public class Friend
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; }

    public bool IsMyBirthday(DateOnly date)
    {
        return (BirthDate.Month == date.Month && BirthDate.Day == date.Day) ||
               (BirthDate.Month == date.Month && BirthDate.Day == 29 && date.Day == 28);
    }
}