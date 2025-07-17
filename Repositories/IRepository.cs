namespace StatSanctum.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T dto);
        Task<T> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
        Task<T> UpdateAsync(T dto);
    }
}
