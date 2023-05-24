using System.Net;
using CodeChallenge.Contracts;
using CodeChallenge.Domain;
using CodeChallenge.Helper;
using CodeChallenge.Models;
using CodeChallenge.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

public class CourseController : BaseApiController
{
	private readonly ICoursesService _courseService;
    private readonly ILogger<StudentRepository> _logger;


    public CourseController(ICoursesService coursesService, ILogger<StudentRepository> logger)
	{
		_courseService = coursesService;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateCourse(CourseVM courseVM)
    {

        if (courseVM == null)
        {
            return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(courseVM)} is empty"));
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(HttpStatusCode.BadRequest);
        }
        try
        {
            //Could Use AutoMapper Here... 

            var course = new Courses
            {
                CourseName = courseVM.CourseName,
                CourseCode = courseVM.CourseCode,
                CourseTitle = courseVM.CourseTitle
            };
            var retVal = await _courseService.Create(course);
            if (retVal.CourseId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ResponseError));
            _logger.LogInformation($"Entity Created {retVal} on {DateTime.UtcNow}");

            return Ok(new ApiStatusResponse(HttpStatusCode.Created,
               $"created Successfully"));
           
        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, message);
            return BadRequest(HttpStatusCode.InternalServerError);

        }

    }


    [HttpGet("get-by-id")]
    public async Task<ActionResult<CourseVMList>> GetGrade(int id)
    {
        var course = await _courseService.Fetch(id);

        if (course == null)
        {
            return NotFound();
        }

        //Could use Mapper here
        var retVal = new CourseVMList
        {
            CourseCode = course.CourseCode,
            CourseTitle = course.CourseTitle,
            CourseId = course.Id,
            CourseName = course.CourseName
           
        };
        _logger.LogInformation($"Entity Fetched {retVal} on {DateTime.UtcNow}");
        return Ok(retVal);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCourse(CourseVMList courseVMList)
    {
        try
        {
            if (courseVMList == null)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(courseVMList)} is empty"));
            }

            if (courseVMList.CourseId < 1)
            {
                return BadRequest("Invalid Course Selection");
            }
          
            if (!ModelState.IsValid)
            {
                return BadRequest(HttpStatusCode.BadRequest);
            }

            //Could Use AutoMapper Here...

            var course = new Courses
            {
                CourseCode = courseVMList.CourseCode,
                CourseName = courseVMList.CourseName,
                CourseTitle = courseVMList.CourseTitle,
                Id = courseVMList.CourseId
            };


            var retVal = await _courseService.Update(course);
          


            if (retVal.CourseId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ResponseError.ErrorMessage));

            _logger.LogInformation($"Entity Updated {retVal} on {DateTime.UtcNow}");

            return Ok(new ApiStatusResponse(HttpStatusCode.OK,
                        $"Record Updated"));


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);

            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
                          $"Something Bad Happened, Pls Try Again Later"));

        }

    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest((HttpStatusCode.BadRequest, $"Terrible Parameter"));
            }
            var deletedCourse = await _courseService.Delete(id);

            if (!deletedCourse.IsSuccessful)
            {
                return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest));
            }

            _logger.LogInformation($"Delete Operation {deletedCourse.CourseId} on {DateTime.UtcNow}");
            return Ok("Success");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest, $"Something Bad Happened, Pls Try Again Later"));

        }
    }

}

