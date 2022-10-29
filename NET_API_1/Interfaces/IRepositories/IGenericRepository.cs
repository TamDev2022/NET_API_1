using NET_API_1.Utils;

namespace NET_API_1.Interfaces.IRepositories
{
    public interface IGenericRepository<T>
    {
        public void Insert(T entity);
        public void Delete(T entity);
        public void Update(T entity);
        public Task<PaginatedList<T>> GetListAsync(int PageNumber, int PageSize);

    }
}
