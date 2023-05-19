using CodeChallenge.Contracts;
using CodeChallenge.Data;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Helpers;

namespace CodeChallenge.Service;

public class CoursesRepository : ICoursesService
{
    private readonly IRepository<Courses> _courseRepository;
    private readonly ILogger<CoursesRepository> _logger;
    private ResponseObj _errorObj;

    public CoursesRepository(AppDbContext appDbContext, ILogger<CoursesRepository> logger)
    {
        _courseRepository = new Repository<Courses>(appDbContext);
        _errorObj = new ResponseObj();
        _logger = logger;
    }

    public async Task<CoursesResponseObj> Create(Courses courses)
    {
        var response = new CoursesResponseObj
        {
            IsSuccessful = false,
            CourseId = 0,
            ResponseError = new ResponseObj()
        };
        try
        {
            if (courses == null)
            {
                _errorObj.ErrorMessage = "Empty Data Object";
                _errorObj.TechMessage = "Empty Data Object";
                response.ResponseError = _errorObj;
                response.CourseId = -1;
                return response;

            }


            var retVal = await _courseRepository.Insert(courses);

            if (retVal == null || retVal.Id < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "Record counld not be created";
                response.ResponseError = _errorObj;
                return response;
            }


            response.IsSuccessful = true;
            response.CourseId = retVal.Id;
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

    public async Task<CoursesResponseObj> Delete(int Id)
    {
        var response = new CoursesResponseObj
        {
            IsSuccessful = false,
            CourseId = 0,
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
            var retVal = _courseRepository.Delete(Id);

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

    public async Task<Courses> Fetch(int id)
    {
        var retVal = new Courses();
        try
        {
            retVal = await _courseRepository.getById(id);
            return retVal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return retVal;
        }

    }

    public async Task<int> GetTotalCount()
    {
        var retVal = from u in _courseRepository.Table
                     select u;
        return await Task.FromResult(retVal.Count());

    }

    public async Task<IEnumerable<Courses>> List()
    {
        var retVal = Enumerable.Empty<Courses>();

        try
        {

            retVal = from c in _courseRepository.Table
                     where c != null
                     select c;

            return await Task.FromResult(retVal.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return retVal;
        }

    }

    public async Task<CoursesResponseObj> Update(Courses courses)
    {
        var response = new CoursesResponseObj
        {
            IsSuccessful = false,
            CourseId = 0,
            ResponseError = new ResponseObj()
        };
        if (courses == null)
        {
            _errorObj.ErrorMessage = "Error Occurred! invalid identity";
            _errorObj.TechMessage = "Records could not be fetch because " +
                "Identity is less than or equals zero";
            response.ResponseError = _errorObj;
            return response;
        }

        try
        {
            var retVal = await _courseRepository.Update(courses);
            if (retVal == null || retVal.Id < 1)
            {
                _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                _errorObj.TechMessage = "could not be retrive record ";
                response.ResponseError = _errorObj;
            }
            response.IsSuccessful = true;
            response.CourseId = retVal!.Id;
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

