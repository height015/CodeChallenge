using CodeChallenge.Contracts;
using CodeChallenge.Data;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Helpers;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Service;

public class StudentRepository : IStudentService
{

    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Grade> _studentGradeRepository;
    private readonly IRepository<StudentCourse> _studentCoursesRepository;
    private readonly ILogger<StudentRepository> _logger;
    private ResponseObj _errorObj;


  

    public StudentRepository(ILogger<StudentRepository> logger,
        IRepository<Grade> studentGradeRepository,
        IRepository<StudentCourse> studentCoursesRepository, IRepository<Student> studentRepository)
    {
        _studentRepository = studentRepository;
        _studentGradeRepository = studentGradeRepository;
        _studentCoursesRepository = studentCoursesRepository;

        _errorObj = new ResponseObj();
        _logger = logger;
    }

    public async Task<StudentResponse> Create(Student student)
    {
        var response = new StudentResponse
        {
            IsSuccess = false,
            StudentId = 0,
            ErrorResponse = new ResponseObj()
        };
        try
        {
            if (student == null)
            {
                _errorObj.ErrorMessage = "Empty Data Object";
                _errorObj.TechMessage = "Empty Data Object";
                response.ErrorResponse = _errorObj;
                response.StudentId = -1;
                return response;

            }
            var retVal = await _studentRepository.Insert(student);

            if (retVal == null || retVal.Id < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ErrorResponse = _errorObj;
                return response;
            }


            response.IsSuccess = true;
            response.StudentId = retVal.Id;
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            response.ErrorResponse = _errorObj;

            return response;
        }

    }

    public async Task<StudentResponse> Delete(int Id)
    {
        var response = new StudentResponse
        {
            IsSuccess = false,
            StudentId = 0,
            ErrorResponse = new ResponseObj()
        };

        if (Id <= 0)
        {
            _errorObj.ErrorMessage = "Error Occurred! invalid identity";
            _errorObj.TechMessage = "Records could not be fetch because " +
                "Identity is less than or equals zero";
            response.ErrorResponse = _errorObj;
        }

        try
        {
            var retVal = _studentRepository.Delete(Id);

            if (!string.IsNullOrEmpty(retVal))
            {
                _errorObj.ErrorMessage = "Error Occurred! Could not Delete";
                _errorObj.TechMessage = "Records could not be Deleted recods ";
                response.ErrorResponse = _errorObj;
            }

            response.IsSuccess = true;
            return await Task.FromResult(response);
        }
        catch (Exception ex)
        {
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            response.ErrorResponse = _errorObj;
            return response;
        }
    }

    public async Task<Student> Fetch(int id)
    {
        var retVal =  new Student();
        try
        {
            //var query  = from s in _studentRepository.Table
            //            join sc in _studentCoursesRepository.Table on s.Id equals sc.StudentId
            //             where s.StudentCourses.Any()
            //             where s.Id == id && sc.StudentId == id
            //            orderby s.Id
            //            select s;
            // retVal = query.ToList();
               return await _studentRepository.getById(id);
            //return retVal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return retVal;
        }

    }

    public async Task<int> GetTotalCount()
    {
        var retVal = from u in _studentRepository.Table
                     select u;
        return await Task.FromResult(retVal.Count());

    }

    public async Task<IEnumerable<Grade>> List(int studentId = 0,
        int courseId = 0, string? courseCode = null, int pageNumber = 1, int pageSize = 5)
    {
        var retVal = Enumerable.Empty<Grade>();

        try
        {

            var ewe = from st in _studentGradeRepository.Table
                      join g in _studentGradeRepository.Table on st.Id equals g.StudentId
                      select g;
            var check = ewe.Count();

          var query =  _studentGradeRepository.Table;
            query = query.Include(g => g.Course);
            query = query.Include(g => g.Student);

            var queryCount = query.Count();
         
            if (courseId > 0 )
            {
               query = query.Where(x => x.Course.Id == courseId);
            }

            if (studentId> 0)
            {
                query = query.Where(x => x.Student.Id == studentId);
            }
           
            if (!string.IsNullOrEmpty(courseCode) || courseCode?.Length > 2)
            {
            query = query.Where(x => x.Course.CourseCode == courseCode);
                
            }

            int totalCount = queryCount;

            // Calculate the number of records to skip
            int skipCount = (pageNumber - 1) * pageSize;

            retVal = query.Skip(skipCount)
            .Take(pageSize)
            .ToList();

            return await Task.FromResult(retVal.ToList());

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return Enumerable.Empty<Grade>().ToList();
        }

    }




    public async Task<StudentResponse> Update(Student student)
    {
        var response = new StudentResponse
        {
            IsSuccess = false,
            StudentId = 0,
            ErrorResponse = new ResponseObj()
        };
        if (student == null)
        {
            _errorObj.ErrorMessage = "Error Occurred! invalid identity";
            _errorObj.TechMessage = "Records could not be fetch because " +
                "Identity is less than or equals zero";
            response.ErrorResponse = _errorObj;
            return response;
        }

        try
        {
            var retVal = await _studentRepository.Update(student);
            if (retVal == null || retVal.Id < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "could not be retrive record ";
                response.ErrorResponse = _errorObj;
            }
            response.IsSuccess = true;
            response.StudentId = retVal!.Id;
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            response.ErrorResponse = _errorObj;
            return response;
        }
    }



    //public IQueryable<Student> ApplySorting(IQueryable<Student> query, string sortBy, string sortDirection)
    //{
    //    if (!string.IsNullOrEmpty(sortBy))
    //    {
    //        // Determine the property to sort by using reflection
    //        var propertyInfo = typeof(Student).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
    //        if (propertyInfo != null)
    //        {
    //            if (string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase))
    //            {
    //                query = query.OrderByDescending(x => propertyInfo.GetValue(x, null));
    //            }
    //            else
    //            {
    //                query = query.OrderBy(x => propertyInfo.GetValue(x, null));
    //            }
    //        }
    //    }
    //    return query;
    //}

    //public IQueryable<Student> ApplyFiltering(IQueryable<Student> query, Dictionary<string, string> filters)
    //{
    //    foreach (var filter in filters)
    //    {
    //        var propertyInfo = typeof(Student).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
    //        if (propertyInfo != null)
    //        {
    //            query = query.Where(x => propertyInfo.GetValue(x, null).ToString().Contains(filter.Value, StringComparison.OrdinalIgnoreCase));
    //        }
    //    }
    //    return query;
    //}

    //public IQueryable<Student> ApplyPagination(IQueryable<Student> query, int pageNumber, int pageSize)
    //{
    //    return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    //}



}
