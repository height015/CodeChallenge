using System;
using CodeChallenge.Domain;

namespace CodeChallenge.Contracts;

public interface IGradeService
{
    Task<Grade> Fetch(int id);

    Task<Grade> GetGradeByStudentId(int studentId);

    Task<GradeResponseObj> Create(Grade grade);

    Task<GradeResponseObj> Update(Grade grade);

    Task<GradeResponseObj> Delete(int Id);

    Task<IEnumerable<Grade>> List(int studentId = 0, int courseId = 0,
        string? courseCode = null, int pageNumber = 1, int pageSize = 5);
}

