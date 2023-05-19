using System;
using CodeChallenge.Domain;

namespace CodeChallenge.Contracts;

public interface IGradeService
{
    Task<Grade> Fetch(int id);

    Task<GradeResponseObj> Create(Grade grade);

    Task<GradeResponseObj> Update(Grade grade);

    Task<GradeResponseObj> Delete(int Id);

    Task<IEnumerable<Grade>> List();
}

