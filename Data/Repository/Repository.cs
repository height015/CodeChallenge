using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data.Repository;


public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;

        public Repository(AppDbContext requestDbContext)
        {
            _appDbContext = requestDbContext;
        }
        public IQueryable<T> Table => _appDbContext.Set<T>();

        public IQueryable<T> TableNoTracking => _appDbContext.Set<T>().AsNoTracking();

        public string Delete(int Id)
        {
            string message = string.Empty;
            try
            {
            var entity =  _appDbContext.Set<T>().Find(Id);
            _appDbContext.Set<T>().Remove(entity);
            _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
               
            }
            return message;

        }

        public async Task<IEnumerable<T>> Fetch()
        {
            return await _appDbContext.Set<T>().ToListAsync();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _appDbContext.Set<T>().Where(expression);
        }

        public async Task<T> getById(int id)
        {
            return await _appDbContext.Set<T>().FindAsync(id);
        }



        public async Task<T> Insert(T entity)
        {
            await _appDbContext.Set<T>().AddAsync(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IList<T>> Insert(IEnumerable<T> entities)
        {
            try
            {
                await _appDbContext.Set<T>().AddRangeAsync(entities);
                await _appDbContext.SaveChangesAsync();
                return entities.ToList();

            }
            catch (Exception ex)
            {

            return Enumerable.Empty<T>().ToList();
        }

        }
        public async Task<T> Update(T entity)
        {
            
            try
            {
            _appDbContext.Set<T>().Update(entity);
            await _appDbContext.SaveChangesAsync();
            return entity;
            }
            catch
            {
            throw; 
            }
            


        }

       
    }
