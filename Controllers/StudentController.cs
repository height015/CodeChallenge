using System.Net;
using CodeChallenge.Contracts;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Models;
using CodeChallenge.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Controllers;


public class StudentController : BaseApiController
{
    private readonly IStudentService _studentService;

    private readonly IGradeService _gradeService;
    private readonly IStudentCourseService _studentCourseService;

    private readonly ILogger<StudentController> _logger;


    public StudentController(IStudentService studentService, ILogger<StudentController> logger, IStudentCourseService studentCourseService, IGradeService gradeService)
    {
        _gradeService = gradeService;
        _studentCourseService = studentCourseService;
        _studentService = studentService;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateStudent(StudentVM studentVM)
    {
        if (studentVM == null)
        {
            return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(studentVM)} is empty"));
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(HttpStatusCode.BadRequest);
        }
        try
        {
            //Could Use AutoMapper Here... 
            var student = new Student
            {
                FirstName = studentVM.GivenName,
                LastName = studentVM.SurnName,
                Class = studentVM.Class,
                DateOfBirth = studentVM.DateOfBirth
            };
            var retVal = await _studentService.Create(student);
            if (retVal.StudentId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ErrorResponse));

            _logger.LogInformation($"Entity Created {retVal} on {DateTime.UtcNow}");
            return Ok((HttpStatusCode.Created, $"created Successfully"));
        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, message);
            return BadRequest(HttpStatusCode.InternalServerError);

        }

    }

    [HttpGet("get")]
    public async Task<ActionResult<StudenDetailVM>> GetStudent(int id)
    {
        var student = await _gradeService.GetGradeByStudentId(id);

        if (student == null)
        {
            return NotFound();
        }
       
        //Could use Mapper here
        var retVal = new StudenDetailVM
        {
            Class = student.Student.Class,
            DateOfBirth = student.Student.DateOfBirth,
            GivenName = student.Student.FirstName,
            SurnName = student.Student.LastName,
            StudentId = student.Student.Id,
            CourseId = student.Course.Id,
            CourseName = student.Course.CourseName,
            CourseTitle = student.Course.CourseTitle,
            CourseCode  = student.Course.CourseCode,
            GradeId = student.Id,
            Score = student.Score,
            Title = student.Title,
            Description = student.Description,
        };

        _logger.LogInformation($"Entity Feteched {retVal} on {DateTime.UtcNow}");
        return Ok(retVal);
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateStudent(StudentEditVM studentEditVM)
    {
        try
        {
            if (studentEditVM == null)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(studentEditVM)} is empty"));
            }

            if (string.IsNullOrEmpty(studentEditVM.GivenName))
            {
                return BadRequest((HttpStatusCode.BadRequest, $"{(studentEditVM.GivenName)} is required and can't be null or Empty"));
            }

            if (studentEditVM.GivenName.Length < 2 || studentEditVM.GivenName.Length > 49)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"{(studentEditVM.GivenName)} must be between 2 and 50 characters"));
            }

            if (string.IsNullOrEmpty(studentEditVM.SurnName))
            {
                return BadRequest((HttpStatusCode.BadRequest, $"{(studentEditVM.SurnName)} is required and can't be null or Empty"));
            }

            if (studentEditVM.SurnName.Length < 2 || studentEditVM.SurnName.Length > 49)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"{(studentEditVM.SurnName)} must be between 2 and 50 characters"));
            }

            if (studentEditVM.Class.Length < 2 || studentEditVM.Class.Length > 49)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"{(studentEditVM.Class)} must be between 2 and 50 characters"));
            }

            var student = new Student
            {
                Id = studentEditVM.StudentId,
                FirstName = studentEditVM.GivenName,
                Class = studentEditVM.Class,
                LastName = studentEditVM.SurnName,
                DateOfBirth = studentEditVM.DateOfBirth,

            };

            var updatedStudent = await _studentService.Update(student);
            if (updatedStudent.StudentId < 1)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"Unbale to update Record"));
            }

            _logger.LogInformation($"Entity Updated {student} on {DateTime.UtcNow}");
            return Ok((HttpStatusCode.OK, $"Record Updated"));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest((HttpStatusCode.BadRequest, $"Something Bad Happened, Pls Try Again Later"));
        }
   
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"Terrible Parameter"));
            }
            var deletedStudent = await _studentService.Delete(id);
            if (!deletedStudent.IsSuccess)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"Unable to complete operation"));
            }

            _logger.LogInformation($"Entity Deleted {deletedStudent.StudentId} on {DateTime.UtcNow}");

            return Ok("Success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest((HttpStatusCode.BadRequest, $"Something Bad Happened, Pls Try Again Later"));

        }
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<StudenDetailVM>>> List(int studentId = 0,int courseId =0,
        string? courseCode = null, int pageNumber = 1, int pageSize = 5)
    {
           
        
        try
        {
            // Get the initial query
           var query = await _studentService.List(studentId, courseId, courseCode, pageNumber, pageSize);

            var retVal = query.Select(x => new StudenDetailVM
            {
                StudentId = x.Student.Id,
                GivenName = x.Student.FirstName,
                SurnName = x.Student.LastName,
                Class = x.Student.Class,
                CourseCode= x.Course.CourseCode,
                CourseName = x.Course.CourseName,
                CourseTitle = x.Course.CourseTitle,
                Score = x.Score,
                Description = x.Description,
                Title = x.Title,
                GradeId = x.Id,
                
            });

            _logger.LogInformation($"Fetched List {retVal} on {DateTime.UtcNow}");
            return Ok(retVal);
        }
        catch (Exception ex)
        {
            // Handle the exception
            // Log the error, return an error response, etc.
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return StatusCode(500, "An error occurred");
        }
    }

   
}





