using System;
using System.ComponentModel.DataAnnotations;
using CodeChallenge.Helpers;

namespace CodeChallenge.Domain;


public class Courses : BaseEntity
{

    public Courses()
    {
        StudentCourses = new HashSet<StudentCourse>();
    }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Course Name contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Course Name is required", AllowEmptyStrings = false)]
    public string CourseName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Course Title contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Course Title is required", AllowEmptyStrings = false)]
    public string CourseTitle { get; set; }

    [StringLength(10, MinimumLength = 2, ErrorMessage = "Course Code contains fewer or more characters than expected. (2 to 10 characters expected)")]
    [Required(ErrorMessage = "Course Code is required", AllowEmptyStrings = false)]
    public string CourseCode { get; set; }

    public ICollection<StudentCourse> StudentCourses { get; set; }
}

public class CoursesResponseObj
{
    public bool IsSuccessful { get; set; }
    public int CourseId { get; set; }
    public ResponseObj ResponseError { get; set; }

}
