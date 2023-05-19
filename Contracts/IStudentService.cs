using System;
using CodeChallenge.Domain;

namespace CodeChallenge.Contracts;

public interface IStudentService
{
    Task<Student> Fetch(int id);

    Task<StudentResponse> Create(Student student);

    Task<StudentResponse> Update(Student grade);

    Task<StudentResponse> Delete(int Id);

    Task<IEnumerable<Student>> List();
}

