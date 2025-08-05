using System.Linq.Expressions;

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

        // ✅ NOUVELLES MÉTHODES AJOUTÉES POUR VOTRE PRODUCTSERVICE

        /// <summary>
        /// Récupère des éléments avec un filtre
        /// </summary>
        /// <param name="filter">Expression de filtre</param>
        /// <returns>Éléments filtrés</returns>
        Task<IEnumerable<T>> GetWithFilterAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Récupère des éléments avec filtre et relations (includes)
        /// </summary>
        /// <param name="filter">Expression de filtre</param>
        /// <param name="includes">Relations à inclure</param>
        /// <returns>Éléments avec relations</returns>
        Task<IEnumerable<T>> GetWithFilterAndIncludesAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Récupère tous les éléments avec des relations (includes)
        /// </summary>
        /// <param name="includes">Relations à inclure</param>
        /// <returns>Éléments avec relations</returns>
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Récupère un élément par ID avec des relations (includes)
        /// </summary>
        /// <param name="id">ID de l'élément</param>
        /// <param name="includes">Relations à inclure</param>
        /// <returns>Élément avec relations</returns>
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Compte les éléments avec un filtre
        /// </summary>
        /// <param name="filter">Expression de filtre</param>
        /// <returns>Nombre d'éléments</returns>
        Task<int> CountWithFilterAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Récupère des éléments paginés avec filtre
        /// </summary>
        /// <param name="filter">Expression de filtre</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="orderBy">Expression de tri</param>
        /// <param name="descending">Tri descendant</param>
        /// <returns>Éléments paginés</returns>
        Task<IEnumerable<T>> GetPagedWithFilterAsync<TKey>(
            Expression<Func<T, bool>> filter,
            int page,
            int pageSize,
            Expression<Func<T, TKey>> orderBy,
            bool descending = false);

        /// <summary>
        /// Calcule la somme d'une propriété avec filtre
        /// </summary>
        /// <typeparam name="TResult">Type du résultat</typeparam>
        /// <param name="filter">Expression de filtre</param>
        /// <param name="selector">Sélecteur de propriété</param>
        /// <returns>Somme calculée</returns>
        Task<decimal> SumWithFilterAsync(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, decimal>> selector);

        /// <summary>
        /// Calcule la moyenne d'une propriété décimale avec filtre
        /// </summary>
        /// <param name="filter">Expression de filtre</param>
        /// <param name="selector">Sélecteur de propriété</param>
        /// <returns>Moyenne calculée</returns>
        Task<decimal> AverageDecimalWithFilterAsync(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, decimal>> selector);
    }
}