using System;
using CodeChallenge.Domain;

namespace CodeChallenge.Contracts;

public interface ICoursesService
{
    Task<Courses> Fetch(int id);

    Task<CoursesResponseObj> Create(Courses courses);

    Task<CoursesResponseObj> Update(Courses courses);

    Task<CoursesResponseObj> Delete(int Id);

    Task<IEnumerable<Courses>> List();
}

