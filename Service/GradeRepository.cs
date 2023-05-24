using CodeChallenge.Contracts;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Service;

public class GradeRepository : IGradeService
{
    private readonly IRepository<Grade> _gradeRepository;
    private readonly IRepository<Courses> _courseRepository;
    private readonly IRepository<StudentCourse> _studentCoursesRepository;

    private readonly ILogger<GradeRepository> _logger;
    private ResponseObj _errorObj;

    public GradeRepository(IRepository<Grade> gradeRepository,
        IRepository<StudentCourse> studentCoursesRepository, IRepository<Courses> courseRepository, ILogger<GradeRepository> logger)
    {
        _gradeRepository = gradeRepository;
        _courseRepository = courseRepository;
        _studentCoursesRepository = studentCoursesRepository;
         _errorObj = new ResponseObj();
        _logger = logger;
    }

    public async Task<GradeResponseObj> Create(Grade grade)
    {
        var response = new GradeResponseObj
        {
            IsSuccessful = false,
            GradeId = 0,
            ResponseError = new ResponseObj()
        };
        try
        {
            if (grade == null)
            {
                _errorObj.ErrorMessage = "Empty Data Object";
                _errorObj.TechMessage = "Empty Data Object";
                response.ResponseError = _errorObj;
                response.GradeId = -1;
                return response;

            }


            var retVal = await _gradeRepository.Insert(grade);

            if (retVal == null || retVal.Id < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ResponseError = _errorObj;
                return response;
            }
            response.IsSuccessful = true;
            response.GradeId = retVal.Id;
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            response.ResponseError = _errorObj;

            return response;
        }

    }

    public async Task<GradeResponseObj> Delete(int Id)
    {
        var response = new GradeResponseObj
        {
            IsSuccessful = false,
            GradeId = 0,
            ResponseError = new ResponseObj()
        };

        if (Id <= 0)
        {
            _errorObj.ErrorMessage = "Error Occurred! invalid identity";
            _errorObj.TechMessage = "Records could not be fetch because " +
                "Identity is less than or equals zero";
            response.ResponseError = _errorObj;
        }

        try
        {
            var retVal = _gradeRepository.Delete(Id);

            if (!string.IsNullOrEmpty(retVal))
            {
                _errorObj.ErrorMessage = "Error Occurred! Could not Delete";
                _errorObj.TechMessage = "Records could not be Deleted recods ";
                response.ResponseError = _errorObj;
            }

            response.IsSuccessful = true;
            return await Task.FromResult(response);
        }
        catch (Exception ex)
        {
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            response.ResponseError = _errorObj;
            return response;
        }
    }

    public async Task<Grade> Fetch(int id)
    {
        var retVal = new Grade();
        try
        {
            retVal = await _gradeRepository.getById(id);
            return retVal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return retVal;
        }

    }

    public async Task<Grade> GetGradeByStudentId(int studentId)
    {
        var retVal = new Grade();
        try
        {
            var query = from g in _gradeRepository.Table
                       where g.StudentId == studentId
                       select  g;
            query = query.Include(g => g.Course);
            query = query.Include(g => g.Student);

            retVal = await query.FirstOrDefaultAsync();

            return retVal;

            //var query = from g in _gradeRepository.Table
            //            join c in _courseRepository.Table on g.StudentId equals studentId
            //            select g;

            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return retVal;
        }

    }

    public async Task<int> GetTotalCount()
    {
        var retVal = from u in _gradeRepository.Table
                     select u;
        return await Task.FromResult(retVal.Count());

    }

    public async Task<IEnumerable<Grade>> List(int studentId = 0, int courseId = 0,
        string? courseCode = null, int pageNumber = 1, int pageSize = 5)
    {
        var retVal = Enumerable.Empty<Grade>();

        try
        {
            var query = from g in _gradeRepository.Table
                      join c in _courseRepository.Table on g.CourseId equals c.Id
                      select g;
            var queryCount = query.Count();

            if (courseId > 0)
            {
                query = query.Where(x => x.Course.Id == courseId);
            }

            if (studentId > 0)
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
            return retVal;
        }

    }

    public async Task<GradeResponseObj> Update(Grade grade)
    {
        var response = new GradeResponseObj
        {
            IsSuccessful = false,
            GradeId = 0,
            ResponseError = new ResponseObj()
        };
        if (grade == null)
        {
            _errorObj.ErrorMessage = "Error Occurred! invalid identity";
            _errorObj.TechMessage = "Records could not be fetch because " +
                "Identity is less than or equals zero";
            response.ResponseError = _errorObj;
            return response;
        }

        try
        {
            var retVal = await _gradeRepository.Update(grade);
            if (retVal == null || retVal.Id < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "could not be retrive record ";
                response.ResponseError = _errorObj;
            }
            response.IsSuccessful = true;
            response.GradeId = retVal!.Id;
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            _errorObj.ErrorMessage = "Unknown Error Occurred!";
            _errorObj.TechMessage = ex.GetBaseException().Message;
            response.ResponseError = _errorObj;
            return response;
        }
    }


}
