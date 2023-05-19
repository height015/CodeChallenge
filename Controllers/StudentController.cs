using System;
using System.Net;
using System.Reflection;
using CodeChallenge.Contracts;
using CodeChallenge.Domain;
using CodeChallenge.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CodeChallenge.Controllers;


public class StudentController : BaseApiController
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentRepository> _logger;


    public StudentController(IStudentService studentService, ILogger<StudentRepository> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }


    [HttpGet("get")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _studentService.Fetch(id);
        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateStudent(Student student)
    {

        if (student == null)
        {
            return BadRequest((HttpStatusCode.BadRequest, $"this object: {nameof(student)} is empty"));
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(HttpStatusCode.BadRequest);
        }
        try
        {

            var retVal = await _studentService.Create(student);
            if (retVal.StudentId < 1)
                return BadRequest((HttpStatusCode.BadRequest, retVal.ErrorResponse));

            return Ok((HttpStatusCode.Created, $"created Successfully"));
        }
        catch (Exception ex)
        {
            var message = ex.InnerException;
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, message);
            return BadRequest(HttpStatusCode.InternalServerError);

        }

    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateStudent(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        var updatedStudent = await _studentService.Update(student);
        if (updatedStudent == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var deletedStudent = await _studentService.Delete(id);
        if (deletedStudent == null)
        {
            return NotFound();
        }

        return NoContent();
    }


        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<Student>>> List([FromQuery] QueryParameters parameters)
        {
            var students = await _studentService.List();

            if (parameters.SortBy != null && parameters.SortDirection != null)
            {
                students = SortStudents(students, parameters.SortBy, parameters.SortDirection);
            }

            if (parameters.FilterBy != null)
            {
                students = FilterStudents(students, parameters.FilterBy);
            }

            var totalCount = students.Count();

            if (parameters.PageNumber > 0 && parameters.PageSize > 0)
            {
                students = PaginateStudents(students, parameters.PageNumber, parameters.PageSize);
            }

            var response = new
            {
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                Students = students
            };

            return Ok(response);
        }

    #region Utility

    private IEnumerable<Student> SortStudents(IEnumerable<Student> students, string sortBy, string sortDirection)
    {
        var propertyInfo = typeof(Student).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            if (string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase))
            {
                students = students.OrderByDescending(x => propertyInfo.GetValue(x, null));
            }
            else
            {
                students = students.OrderBy(x => propertyInfo.GetValue(x, null));
            }
        }
        return students;
    }

    private IEnumerable<Student> FilterStudents(IEnumerable<Student> students, Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            students = students.Where(x => x.GetType().GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                .GetValue(x, null).ToString().Contains(filter.Value, StringComparison.OrdinalIgnoreCase));
        }
        return students;
    }

    private IEnumerable<Student> PaginateStudents(IEnumerable<Student> students, int pageNumber, int pageSize)
    {
        return students.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
    #endregion
}





