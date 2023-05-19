using System;
using CodeChallenge.Helpers;

namespace CodeChallenge.Domain;

public class StudentCourse : BaseEntity
{
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public int CoursesId { get; set; }
    public Courses Course { get; set; }

    public int GradeId { get; set; }
    public Grade Grade { get; set; }

}

public class StudentCourseResponseObj
{
    public bool IsSuccessful { get; set; }
    public int StudentCourseId { get; set; }
    public ResponseObj ResponseError { get; set; }

}