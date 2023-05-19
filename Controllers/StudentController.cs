using System.Globalization;
using System.Net;
using System.Reflection;
using CodeChallenge.Contracts;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Student>>> List(string sortBy, string sortDirection, string filterValue, int pageNumber = 1, int pageSize = 10)
        {

        //var students = await _studentService.List();

        //if (parameters.SortBy != null && parameters.SortDirection != null)
        //{
        //    students = SortStudents(students, parameters.SortBy, parameters.SortDirection);
        //}

        //if (parameters.FilterBy != null)
        //{
        //    students = FilterStudents(students, parameters.FilterBy);
        //}

        //var totalCount = students.Count();

        //if (parameters.PageNumber > 0 && parameters.PageSize > 0)
        //{
        //    students = PaginateStudents(students, parameters.PageNumber, parameters.PageSize);
        //}

        try
        {
            // Get the initial query
            IQueryable<Student> query = await _studentService.List();

            // Apply filtering
            if (!string.IsNullOrEmpty(filterValue))
            {
                var filters = new Dictionary<string, string>
                {
                    // will Set up my filters here based on the filterValue input
                    // For example:
                    // { "FirstName", filterValue },
                    // { "LastName", filterValue }
                };

                query = query.ApplyFiltering(filters);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = query.ApplySorting(sortBy, sortDirection);
            }

            query = query.ApplyPagination(pageNumber, pageSize);

            var students = await query.ToListAsync();

        
            return Ok(students);
        }
        catch (Exception ex)
        {
            // Handle the exception
            // Log the error, return an error response, etc.
            return StatusCode(500, "An error occurred");
        }
    }

           
        

    #region Utility
   
   
    #endregion
}





