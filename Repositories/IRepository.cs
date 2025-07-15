namespace StatSanctum.Repositories
{
    public interface IRepository<TDto, TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Create(TDto dto);
        Task<TEntity> GetById(int id);
        Task DeleteById(int id);
        Task<TEntity> Update(int id, TDto dto);
    }
}
