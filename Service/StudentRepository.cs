using System;
using System.Reflection;
using CodeChallenge.Contracts;
using CodeChallenge.Data;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Helpers;

namespace CodeChallenge.Service;

public class StudentRepository : IStudentService
{

    private readonly IRepository<Student> _studentRepository;
    private readonly ILogger<StudentRepository> _logger;
    private ResponseObj _errorObj;

    public StudentRepository(AppDbContext appDbContext, ILogger<StudentRepository> logger)
    {
        _studentRepository = new Repository<Student>(appDbContext);
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
        var retVal = new Student();
        try
        {
            retVal = await _studentRepository.getById(id);
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
        var retVal = from u in _studentRepository.Table
                     select u;
        return await Task.FromResult(retVal.Count());

    }

    public async Task<IQueryable<Student>> List()
    {
        var retVal = Enumerable.Empty<Student>();

        try
        {

            retVal = from c in _studentRepository.Table
                     where c != null
                     select c;

            return await Task.FromResult(retVal.AsQueryable());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return Enumerable.Empty<Student>().AsQueryable();
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
