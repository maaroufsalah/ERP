namespace ERP.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // ✅ Méthodes CRUD de base
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);

        // ✅ Méthodes d'optimisation et performance
        Task<T?> GetByIdAsNoTrackingAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);

        // ✅ Méthodes de gestion du contexte
        Task<int> SaveChangesAsync();
        void Attach(T entity);
        void Detach(T entity);

        // ✅ Méthodes avancées
        Task<T> UpdatePartialAsync(T entity, params string[] propertiesToUpdate);
        Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities);
        Task<bool> DeleteRangeAsync(IEnumerable<T> entities);
    }
}