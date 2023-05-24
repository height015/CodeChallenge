using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models;

public class CourseVM
{
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Course Name contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Course Name is required", AllowEmptyStrings = false)]
    public string CourseName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Course Title contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Course Title is required", AllowEmptyStrings = false)]
    public string CourseTitle { get; set; }

    [StringLength(10, MinimumLength = 2, ErrorMessage = "Course Code contains fewer or more characters than expected. (2 to 10 characters expected)")]
    [Required(ErrorMessage = "Course Code is required", AllowEmptyStrings = false)]
    public string CourseCode { get; set; }

}

public class CourseVMList
{

  public int CourseId { get; set; }

  public string CourseName { get; set; }

  public string CourseTitle { get; set; }

  public string CourseCode { get; set; }

}
