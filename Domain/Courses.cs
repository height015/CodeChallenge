using System;
using CodeChallenge.Helpers;

namespace CodeChallenge.Domain;


public class Courses : BaseEntity
{

    public Courses()
    {
        StudentCourses = new HashSet<StudentCourse>();
    }
    public string CourseName { get; set; }
    public string CourseTitle { get; set; }
    public string CourseCode { get; set; }

    public ICollection<StudentCourse> StudentCourses { get; set; }
}

public class CoursesResponseObj
{
    public bool IsSuccessful { get; set; }
    public int CourseId { get; set; }
    public ResponseObj ResponseError { get; set; }

}
