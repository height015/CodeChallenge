using System;

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

