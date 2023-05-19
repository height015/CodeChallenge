using System;
using CodeChallenge.Domain;

namespace CodeChallenge.Contracts;

public interface IStudentCourseService
{
    Task<StudentCourse> Fetch(int id);

    Task<StudentCourseResponseObj> Create(StudentCourse studentcourse);

    Task<StudentCourseResponseObj> Update(StudentCourse grade);

    Task<StudentCourseResponseObj> Delete(int Id);

    Task<IEnumerable<StudentCourse>> List();
}

