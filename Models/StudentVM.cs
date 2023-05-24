using System;
using CodeChallenge.Domain;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models;

public class StudentVM
{
     
    [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Given Name is required", AllowEmptyStrings = false)]
    public string GivenName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "SurnName is required", AllowEmptyStrings = false)]
    public string SurnName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Class contains fewer or more characters than expected (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Class is required", AllowEmptyStrings = false)]
    public string Class { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Date Of Birth is required", AllowEmptyStrings = false)]
    public DateTime DateOfBirth { get; set; }


}

public class StudenDetailVM
{
    public int StudentId { get; set; }
    public string GivenName { get; set; }
    public string SurnName { get; set; }
    public string Class { get; set; }
    public DateTime DateOfBirth { get; set; }

    //Course
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseTitle { get; set; }
    public string CourseCode { get; set; }

    //Grade
    public int GradeId { get; set; }
    public int Score { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

}

public class SearchObj
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string CourseCode { get; set; }
    public int Score { get; set; }
}


public class StudentEditVM
{
    public int StudentId { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Given Name is required", AllowEmptyStrings = false)]
    public string GivenName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "SurnName is required", AllowEmptyStrings = false)]
    public string SurnName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Class contains fewer or more characters than expected (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Class is required", AllowEmptyStrings = false)]
    public string Class { get; set; }

    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Date Of Birth is required", AllowEmptyStrings = false)]
    public DateTime DateOfBirth { get; set; }
}

