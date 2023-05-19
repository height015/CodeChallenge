using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CodeChallenge.Helpers;

namespace CodeChallenge.Domain;


public class Student  : BaseEntity
{

    public Student()
    {
        Grades = new HashSet<Grade>();
        StudentCourses = new HashSet<StudentCourse>();
    }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "First Name is required", AllowEmptyStrings = false)]
    public string FirstName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Last Name is required", AllowEmptyStrings = false)]
    public string LastName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Class contains fewer or more characters than expected (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Class is required", AllowEmptyStrings = false)]
    public string Class { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Date Of Birth is required", AllowEmptyStrings = false)]
    public DateTime DateOfBirth { get; set; }


    public ICollection<Grade> Grades { get; set; }
    public ICollection<StudentCourse> StudentCourses { get; set; }

}


public class StudentResponse
{
    public int StudentId { get; set; }
    public bool IsSuccess { get; set; }
    public ResponseObj ErrorResponse { get; set; }
}