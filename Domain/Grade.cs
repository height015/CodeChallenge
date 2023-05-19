using System;
using System.ComponentModel.DataAnnotations;
using CodeChallenge.Helpers;

namespace CodeChallenge.Domain;


public class Grade : BaseEntity
{
    [Range(0, int.MaxValue, ErrorMessage = "Minimium Score must not be less than 0")]
    [Required(ErrorMessage = "Score is required", AllowEmptyStrings = false)]
    public int Score { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Title contains fewer or more characters than expected. (2 to 50 characters expected)")]
    [Required(ErrorMessage = "Title is required", AllowEmptyStrings = false)]
    public string Title { get; set; }

    [StringLength(500, MinimumLength = 5, ErrorMessage = "Description contains fewer or more characters than expected. (5 to 500 characters expected)")]
    [Required(ErrorMessage = "Description is required", AllowEmptyStrings = false)]
    public string Description { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

}

public class GradeResponseObj
{
    public bool IsSuccessful { get; set; }
    public int GradeId { get; set; }
    public ResponseObj ResponseError { get; set; }

}
