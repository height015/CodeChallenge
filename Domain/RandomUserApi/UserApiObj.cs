using System;

namespace CodeChallenge.Domain;

public class UserApiObj
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Class { get; set; }
    public DateTime DateOfBirth { get; set; }



    public static Student MapUserApiResponseToStudent(UserApiObj userApiObj)
    {
        return new Student
        {
            FirstName = userApiObj.FirstName,
            LastName = userApiObj.LastName,
            Class = userApiObj.Class,
            DateOfBirth = userApiObj.DateOfBirth
        };
    }
}



