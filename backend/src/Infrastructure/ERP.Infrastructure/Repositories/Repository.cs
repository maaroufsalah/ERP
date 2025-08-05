using ERP.Application.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ERP.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ErpDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<Repository<T>> _logger;

        public Repository(ErpDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        // ✅ ================= MÉTHODES CRUD DE BASE =================

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                _logger.LogDebug("Récupération de tous les éléments de type {EntityType}", typeof(T).Name);
                return await _dbSet.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de tous les éléments de type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogDebug("Récupération de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                _logger.LogDebug("Création d'un nouvel élément de type {EntityType}", typeof(T).Name);

                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Élément {EntityType} créé avec succès", typeof(T).Name);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'élément {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            try
            {
                _logger.LogDebug("Mise à jour d'un élément de type {EntityType}", typeof(T).Name);

                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Élément {EntityType} mis à jour avec succès", typeof(T).Name);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de l'élément {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogDebug("Suppression de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);

                var entity = await GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogWarning("Tentative de suppression d'un élément {EntityType} inexistant avec l'ID: {Id}", typeof(T).Name, id);
                    return false;
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Élément {EntityType} avec l'ID {Id} supprimé avec succès", typeof(T).Name, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);
                throw;
            }
        }

        // ✅ ================= MÉTHODES D'OPTIMISATION =================

        public async Task<T?> GetByIdAsNoTrackingAsync(int id)
        {
            try
            {
                _logger.LogDebug("Récupération sans tracking de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération sans tracking de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _dbSet.AsNoTracking().AnyAsync(e => EF.Property<int>(e, "Id") == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'existence de l'élément {EntityType} avec l'ID: {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<int> CountAsync()
        {
            try
            {
                return await _dbSet.AsNoTracking().CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des éléments {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                _logger.LogDebug("Récupération paginée des éléments {EntityType}: page {Page}, taille {PageSize}", typeof(T).Name, page, pageSize);

                return await _dbSet
                    .AsNoTracking()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération paginée des éléments {EntityType}", typeof(T).Name);
                throw;
            }
        }

        // ✅ ================= MÉTHODES DE GESTION DU CONTEXTE =================

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde des changements");
                throw;
            }
        }

        public void Attach(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                _context.Attach(entity);
                _logger.LogDebug("Entité {EntityType} attachée au contexte", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'attachement de l'entité {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public void Detach(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                _context.Entry(entity).State = EntityState.Detached;
                _logger.LogDebug("Entité {EntityType} détachée du contexte", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du détachement de l'entité {EntityType}", typeof(T).Name);
                throw;
            }
        }

        // ✅ ================= MÉTHODES AVANCÉES =================

        public async Task<T> UpdatePartialAsync(T entity, params string[] propertiesToUpdate)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                _context.Attach(entity);
                var entry = _context.Entry(entity);

                foreach (var property in propertiesToUpdate)
                {
                    entry.Property(property).IsModified = true;
                }

                await _context.SaveChangesAsync();

                _logger.LogDebug("Mise à jour partielle de l'entité {EntityType} effectuée", typeof(T).Name);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour partielle de l'entité {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null || !entities.Any())
                    throw new ArgumentException("La collection d'entités ne peut pas être vide", nameof(entities));

                _logger.LogDebug("Création en lot de {Count} entités de type {EntityType}", entities.Count(), typeof(T).Name);

                await _dbSet.AddRangeAsync(entities);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Création en lot de {Count} entités {EntityType} effectuée avec succès", entities.Count(), typeof(T).Name);
                return entities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création en lot des entités {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null || !entities.Any())
                    return true;

                _logger.LogDebug("Suppression en lot de {Count} entités de type {EntityType}", entities.Count(), typeof(T).Name);

                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();

                _logger.LogDebug("Suppression en lot de {Count} entités {EntityType} effectuée avec succès", entities.Count(), typeof(T).Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression en lot des entités {EntityType}", typeof(T).Name);
                throw;
            }
        }

        // ✅ ================= NOUVELLES MÉTHODES AJOUTÉES =================

        public async Task<IEnumerable<T>> GetWithFilterAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                _logger.LogDebug("Récupération des éléments {EntityType} avec filtre", typeof(T).Name);
                return await _dbSet.AsNoTracking().Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des éléments {EntityType} avec filtre", typeof(T).Name);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetWithFilterAndIncludesAsync(
            Expression<Func<T, bool>> filter,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                _logger.LogDebug("Récupération des éléments {EntityType} avec filtre et relations", typeof(T).Name);

                IQueryable<T> query = _dbSet.AsNoTracking();

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query.Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des éléments {EntityType} avec filtre et relations", typeof(T).Name);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                _logger.LogDebug("Récupération de tous les éléments {EntityType} avec relations", typeof(T).Name);

                IQueryable<T> query = _dbSet.AsNoTracking();

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de tous les éléments {EntityType} avec relations", typeof(T).Name);
                throw;
            }
        }

        public async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                _logger.LogDebug("Récupération de l'élément {EntityType} avec l'ID: {Id} et relations", typeof(T).Name, id);

                IQueryable<T> query = _dbSet.AsNoTracking();

                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'élément {EntityType} avec l'ID: {Id} et relations", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<int> CountWithFilterAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                _logger.LogDebug("Comptage des éléments {EntityType} avec filtre", typeof(T).Name);
                return await _dbSet.AsNoTracking().CountAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du comptage des éléments {EntityType} avec filtre", typeof(T).Name);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetPagedWithFilterAsync<TKey>(
            Expression<Func<T, bool>> filter,
            int page,
            int pageSize,
            Expression<Func<T, TKey>> orderBy,
            bool descending = false)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;

                _logger.LogDebug("Récupération paginée des éléments {EntityType} avec filtre: page {Page}, taille {PageSize}", typeof(T).Name, page, pageSize);

                IQueryable<T> query = _dbSet.AsNoTracking().Where(filter);

                query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

                return await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération paginée des éléments {EntityType} avec filtre", typeof(T).Name);
                throw;
            }
        }

        public async Task<decimal> SumWithFilterAsync(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, decimal>> selector)
        {
            try
            {
                _logger.LogDebug("Calcul de la somme des éléments {EntityType} avec filtre", typeof(T).Name);
                return await _dbSet.AsNoTracking().Where(filter).SumAsync(selector);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de la somme des éléments {EntityType} avec filtre", typeof(T).Name);
                throw;
            }
        }

        public async Task<decimal> AverageDecimalWithFilterAsync(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, decimal>> selector)
        {
            try
            {
                _logger.LogDebug("Calcul de la moyenne des éléments {EntityType} avec filtre", typeof(T).Name);
                var items = await _dbSet.AsNoTracking().Where(filter).ToListAsync();
                return items.Any() ? items.Average(selector.Compile()) : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de la moyenne des éléments {EntityType} avec filtre", typeof(T).Name);
                throw;
            }
        }
    }
}