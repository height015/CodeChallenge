using System;
using CodeChallenge.Domain;
using CodeChallenge.Models;

namespace CodeChallenge.Contracts;

public interface IStudentService
{
    Task<Student> Fetch(int id);

    Task<StudentResponse> Create(Student student);

    Task<StudentResponse> Update(Student grade);

    Task<StudentResponse> Delete(int Id);

    Task<IEnumerable<Grade>> List(int studentId = 0,
        int courseId = 0, string courseCode = null, int pageNumber = 1, int pageSize =5);
}

