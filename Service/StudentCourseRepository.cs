using System;
using System.Diagnostics;
using CodeChallenge.Contracts;
using CodeChallenge.Data;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Helpers;

namespace CodeChallenge.Service
{
	public class StudentCourseRepository : IStudentCourseService
    {
        private readonly IRepository<StudentCourse> _studentCourseRepository;
        private readonly ILogger<StudentCourseRepository> _logger;
        private ResponseObj _errorObj;

        public StudentCourseRepository(AppDbContext appDbContext, ILogger<StudentCourseRepository> logger)
        {
            _studentCourseRepository = new Repository<StudentCourse>(appDbContext);
            _errorObj = new ResponseObj();
            _logger = logger;
        }

        public async Task<StudentCourseResponseObj> Create(StudentCourse studentcourse)
        {
            var response = new StudentCourseResponseObj
            {
                IsSuccessful = false,
                StudentCourseId = 0,
                ResponseError = new ResponseObj()
            };
            try
            {
                if (studentcourse == null)
                {
                    _errorObj.ErrorMessage = "Empty Data Object";
                    _errorObj.TechMessage = "Empty Data Object";
                    response.ResponseError = _errorObj;
                    response.StudentCourseId = -1;
                    return response;
                }


                var retVal = await _studentCourseRepository.Insert(studentcourse);

                if (retVal == null || retVal.Id < 1)
                {
                    _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                    _errorObj.TechMessage = "Record counld not be created";
                    response.ResponseError = _errorObj;
                    return response;
                }


                response.IsSuccessful = true;
                response.StudentCourseId = retVal.Id;
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

        public async Task<StudentCourseResponseObj> Delete(int Id)
        {
            var response = new StudentCourseResponseObj
            {
                IsSuccessful = false,
                StudentCourseId = 0,
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
                var retVal = _studentCourseRepository.Delete(Id);

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

        public async Task<StudentCourse> Fetch(int id)
        {
            var response = new StudentCourseResponseObj
            {
                IsSuccessful = false,
                StudentCourseId = 0,
                ResponseError = new ResponseObj()
            };

            var retVal = new StudentCourse();
            try
            {
                retVal = await _studentCourseRepository.getById(id);
                return retVal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return retVal;
            }


        }

        public async Task<IEnumerable<StudentCourse>> List()
        {
            var response = new StudentCourseResponseObj
            {
                IsSuccessful = false,
                StudentCourseId = 0,
                ResponseError = new ResponseObj()
            };


            var retVal = Enumerable.Empty<StudentCourse>();

            try
            {

                retVal = from c in _studentCourseRepository.Table
                         where c != null
                         select c;

                return await Task.FromResult(retVal.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
                return retVal;
            }
        }

        public async Task<StudentCourseResponseObj> Update(StudentCourse studentCourse)
        {
            var response = new StudentCourseResponseObj
            {
                IsSuccessful = false,
                StudentCourseId = 0,
                ResponseError = new ResponseObj()
            };


            if (studentCourse == null)
            {
                _errorObj.ErrorMessage = "Error Occurred! invalid identity";
                _errorObj.TechMessage = "Records could not be fetch because " +
                    "Identity is less than or equals zero";
                response.ResponseError = _errorObj;
                return response;
            }

            try
            {
                var retVal = await _studentCourseRepository.Update(studentCourse);
                if (retVal == null || retVal.Id < 1)
                {
                    _errorObj.ErrorMessage = "Error Occurred! Please try again later";
                    _errorObj.TechMessage = "could not be retrive record ";
                    response.ResponseError = _errorObj;
                }
                response.IsSuccessful = true;
                response.StudentCourseId = retVal!.Id;
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
}

