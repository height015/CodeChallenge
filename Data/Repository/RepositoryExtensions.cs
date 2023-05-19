using System;
using System.Reflection;

namespace CodeChallenge.Data.Repository;

public static class RepositoryExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, string sortDirection)
    {
        if (!string.IsNullOrEmpty(sortBy))
        {
            var propertyInfo = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                if (string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => propertyInfo.GetValue(x, null));
                }
                else
                {
                    query = query.OrderBy(x => propertyInfo.GetValue(x, null));
                }
            }
        }
        return query;
    }

    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, Dictionary<string, string> filters)
    {
        foreach (var filter in filters)
        {
            var propertyInfo = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                query = query.Where(x => propertyInfo.GetValue(x, null).ToString().Contains(filter.Value, StringComparison.OrdinalIgnoreCase));
            }
        }
        return query;
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}
