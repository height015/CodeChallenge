
using System.Net;
using CodeChallenge.Contracts;
using CodeChallenge.Domain;
using CodeChallenge.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

public class GradeController : BaseApiController
{
    private readonly ICoursesService _courseService;
    private readonly IStudentService _studentService;
    private readonly IGradeService _gradeService;
    private readonly IStudentCourseService _studentCourseService;
    private readonly ILogger<GradeController> _logger;

    public GradeController(ICoursesService courseService, ILogger<GradeController> logger, IStudentService studentService, IGradeService gradeService, IStudentCourseService studentCourseService)
	{
        _courseService = courseService;
        _studentService = studentService;
        _gradeService = gradeService;
        _studentCourseService = studentCourseService;
        _logger = logger;

    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateGrade(GradeVM gradeVM)
    {
        if (gradeVM == null)
        {
            return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(gradeVM)} is empty"));
        }

        try
        {
            var gradeCollectionObj = new List<Grade>();
            var studentCourseCollectObj = new List<StudentCourse>();

            if (gradeVM.CourseId < 1)
            {
                return BadRequest("Invalid Course Selection");
            }
            if (gradeVM.StudentId < 1)
            {
                return BadRequest("Invalid Student Selection");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(HttpStatusCode.BadRequest);
            }

            var studentObj = await _studentService.Fetch(gradeVM.StudentId);
            var courseObj = await _courseService.Fetch(gradeVM.CourseId);


            //Could Use AutoMapper Here...

            var grade = new Grade
            {
                Score = gradeVM.Score,
                Title = gradeVM.Title,
                Description = gradeVM.Description,
                StudentId = gradeVM.StudentId,
                CourseId = gradeVM.CourseId,
                Course = courseObj,
                Student = studentObj,

            };

 
            var studentCourse = new StudentCourse
            {
                CoursesId = courseObj.Id,
                Course = courseObj,
                StudentId = studentObj.Id,
                Student = studentObj,
            };

            gradeCollectionObj.Add(grade);
            studentCourseCollectObj.Add(studentCourse);


           

            var retVal = await _gradeService.Create(grade);
            var retVal2 = await _studentCourseService.Create(studentCourse);

          
            if (retVal.GradeId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ResponseError.ErrorMessage));

            if (retVal2.StudentCourseId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ResponseError.ErrorMessage));

            _logger.LogInformation($"Entity Updated {retVal} on {DateTime.UtcNow}");
            return Ok((HttpStatusCode.Created, $"created Successfully"));
        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, message);
            return BadRequest(HttpStatusCode.InternalServerError);

        }

    }

    [HttpGet("get-by-id")]
    public async Task<ActionResult<GradeListVM>> GetGrade(int id)
    {
        var grade = await _gradeService.Fetch(id);
        var firstname = (await _studentService.Fetch(grade.StudentId)).FirstName;
        var courseName = (await _courseService.Fetch(grade.CourseId)).CourseName;

        if (grade == null)
        {
            return NotFound();
        }

        //Could use Mapper here
        var retVal = new GradeListVM
        {
            GradeId = grade.Id,
            StudentId = grade.Student.Id,
            CourseId = grade.Course.Id,
            Score = grade.Score,
            Title = grade.Title,
            Description = grade.Description,
            StudentName = firstname,
            CourseName = courseName,
        };

        _logger.LogInformation($"Entity Fetched {retVal} on {DateTime.UtcNow}");
        return Ok(retVal);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateStudent(GradeListVM gradeVm)
    {
        try
        {
            if (gradeVm == null)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(gradeVm)} is empty"));
            }

            var gradeCollectionObj = new List<Grade>();
            var studentCourseCollectObj = new List<StudentCourse>();

            if (gradeVm.CourseId < 1)
            {
                return BadRequest("Invalid Course Selection");
            }
            if (gradeVm.StudentId < 1)
            {
                return BadRequest("Invalid Student Selection");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(HttpStatusCode.BadRequest);
            }

            var studentObj = await _studentService.Fetch(gradeVm.StudentId);
            var courseObj = await _courseService.Fetch(gradeVm.CourseId);


            //Could Use AutoMapper Here...

            var grade = new Grade
            {
                Score = gradeVm.Score,
                Title = gradeVm.Title,
                Description = gradeVm.Description,
                StudentId = gradeVm.StudentId,
                CourseId = gradeVm.CourseId,
                Course = courseObj,
                Student = studentObj,

            };


            var studentCourse = new StudentCourse
            {
                CoursesId = courseObj.Id,
                Course = courseObj,
                StudentId = studentObj.Id,
                Student = studentObj,
            };

            gradeCollectionObj.Add(grade);
            studentCourseCollectObj.Add(studentCourse);




            var retVal = await _gradeService.Update(grade);
            var retVal2 = await _studentCourseService.Update(studentCourse);


            if (retVal.GradeId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ResponseError.ErrorMessage));

            if (retVal2.StudentCourseId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ResponseError.ErrorMessage));

            _logger.LogInformation($"Entity Updated {retVal} on {DateTime.UtcNow}");
            return Ok((HttpStatusCode.OK, $"Record Updated"));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest((HttpStatusCode.BadRequest, $"Something Bad Happened, Pls Try Again Later"));
        }

    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"Terrible Parameter"));
            }
            var deletedStudent = await _gradeService.Delete(id);

            if (!deletedStudent.IsSuccessful)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"Unable to complete operation"));
            }

            _logger.LogInformation($"Delete Operation {deletedStudent.GradeId} on {DateTime.UtcNow}");
            return Ok("Success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest((HttpStatusCode.BadRequest, $"Something Bad Happened, Pls Try Again Later"));

        }
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<GradeListVM>>> List(int studentId = 0, int courseId = 0,
        string? courseCode = null, int pageNumber = 1, int pageSize = 5)
    {
        try
        {
            // Get the initial query
            var query = await _studentService.List(studentId, courseId, courseCode, pageNumber, pageSize);

            var retVal = query.Select(x => new GradeListVM
            {
                StudentId = x.Student.Id,
                StudentName = x.Student.FirstName,
                CourseName = x.Course.CourseName,
                Score = x.Score,
                Description = x.Description,
                Title = x.Title,
                GradeId = x.Id,

            });

            _logger.LogInformation($"List Fecthed {retVal} on {DateTime.UtcNow}");
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

