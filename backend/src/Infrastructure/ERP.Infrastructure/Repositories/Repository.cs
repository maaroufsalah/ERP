using ERP.Application.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

                // Optimisation: utiliser AsNoTracking pour de meilleures performances en lecture seule
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

                // Pour les opérations de lecture seule, on peut utiliser AsNoTracking
                // Mais pour les mises à jour potentielles, on garde le tracking
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

        /// <summary>
        /// Récupère un élément par ID sans tracking (pour lecture seule)
        /// </summary>
        /// <param name="id">ID de l'élément</param>
        /// <returns>L'élément sans tracking</returns>
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

        /// <summary>
        /// Vérifie si un élément existe
        /// </summary>
        /// <param name="id">ID de l'élément</param>
        /// <returns>True si l'élément existe</returns>
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

        /// <summary>
        /// Compte le nombre total d'éléments
        /// </summary>
        /// <returns>Nombre d'éléments</returns>
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

        /// <summary>
        /// Récupère les éléments avec pagination
        /// </summary>
        /// <param name="page">Numéro de page (commence à 1)</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Éléments paginés</returns>
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

        /// <summary>
        /// Sauvegarde les changements explicitement
        /// </summary>
        /// <returns>Nombre d'entités affectées</returns>
        public async Task<int> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                _logger.LogDebug("Sauvegarde effectuée: {AffectedEntities} entités affectées", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde des changements");
                throw;
            }
        }

        /// <summary>
        /// Attache une entité au contexte (utile pour les mises à jour partielles)
        /// </summary>
        /// <param name="entity">Entité à attacher</param>
        public void Attach(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                _dbSet.Attach(entity);
                _logger.LogDebug("Entité {EntityType} attachée au contexte", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'attachement de l'entité {EntityType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Détache une entité du contexte
        /// </summary>
        /// <param name="entity">Entité à détacher</param>
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

        /// <summary>
        /// Met à jour partiellement une entité (uniquement les propriétés modifiées)
        /// </summary>
        /// <param name="entity">Entité avec les nouvelles valeurs</param>
        /// <param name="propertiesToUpdate">Propriétés à mettre à jour</param>
        public async Task<T> UpdatePartialAsync(T entity, params string[] propertiesToUpdate)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                _dbSet.Attach(entity);
                var entry = _context.Entry(entity);

                // Marquer seulement les propriétés spécifiées comme modifiées
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

        /// <summary>
        /// Crée plusieurs entités en une seule transaction
        /// </summary>
        /// <param name="entities">Entités à créer</param>
        /// <returns>Entités créées</returns>
        public async Task<IEnumerable<T>> CreateRangeAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null || !entities.Any())
                    throw new ArgumentException("La liste d'entités ne peut pas être vide", nameof(entities));

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

        /// <summary>
        /// Supprime plusieurs entités en une seule transaction
        /// </summary>
        /// <param name="entities">Entités à supprimer</param>
        /// <returns>True si la suppression a réussi</returns>
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

        // ✅ ================= MÉTHODES UTILITAIRES SUPPLÉMENTAIRES =================

        /// <summary>
        /// Récupère les premiers N éléments
        /// </summary>
        /// <param name="count">Nombre d'éléments à récupérer</param>
        /// <returns>Les N premiers éléments</returns>
        public async Task<IEnumerable<T>> GetTopAsync(int count)
        {
            try
            {
                _logger.LogDebug("Récupération des {Count} premiers éléments de type {EntityType}", count, typeof(T).Name);
                return await _dbSet.AsNoTracking().Take(count).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des premiers éléments {EntityType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Exécute une requête SQL brute
        /// </summary>
        /// <param name="sql">Requête SQL</param>
        /// <param name="parameters">Paramètres</param>
        /// <returns>Résultats de la requête</returns>
        public async Task<IEnumerable<T>> ExecuteSqlQueryAsync(string sql, params object[] parameters)
        {
            try
            {
                _logger.LogDebug("Exécution d'une requête SQL brute pour {EntityType}", typeof(T).Name);
                return await _dbSet.FromSqlRaw(sql, parameters).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'exécution de la requête SQL pour {EntityType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Recharge une entité depuis la base de données
        /// </summary>
        /// <param name="entity">Entité à recharger</param>
        /// <returns>Entité rechargée</returns>
        public async Task<T> ReloadAsync(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                await _context.Entry(entity).ReloadAsync();
                _logger.LogDebug("Entité {EntityType} rechargée depuis la base de données", typeof(T).Name);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du rechargement de l'entité {EntityType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Obtient l'état de tracking d'une entité
        /// </summary>
        /// <param name="entity">Entité</param>
        /// <returns>État de l'entité</returns>
        public EntityState GetEntityState(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "L'entité ne peut pas être nulle");

                return _context.Entry(entity).State;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'état de l'entité {EntityType}", typeof(T).Name);
                throw;
            }
        }

        /// <summary>
        /// Libère les ressources
        /// </summary>
        public void Dispose()
        {
            _context?.Dispose();
        }

        /// <summary>
        /// Libération asynchrone des ressources
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
    }
}