using System;
namespace CodeChallenge.Data.Repository;

public interface IRepository<T> where T : class
{
    Task<T> getById(int id);

    Task<T> Insert(T entity);

    Task<IList<T>> Insert(IEnumerable<T> entities);

    Task<T> Update(T entity);

    Task<IEnumerable<T>> Fetch();

    string Delete(int Id);

    IQueryable<T> Table { get; }

    IQueryable<T> TableNoTracking { get; }

}

