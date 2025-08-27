using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Data.Context;
using MoveEnergia.Billing.Core.Interface.Repository;

namespace MoveEnergia.Billing.Data.Repository
{
    public abstract class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        protected DbSet<T> EntitySet => _context.Set<T>();

        /// <summary>
        /// Altera a entidade fornecida no contexto do banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser alterada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade alterada.</returns>
        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await Task.FromResult(entity);
        }
        /// <summary>
        /// Exclui a entidade fornecida do contexto do banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser excluída.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade excluída.</returns>
        public async Task<T> DeleteAsync(T entity)
        {
            //_context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await Task.FromResult(entity);
        }
        public async Task<T> Remove(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return null;
            return entity;
        }

        /// <summary>
        /// Inclui a entidade fornecida no contexto do banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser incluída.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade incluída.</returns>

        public async Task<T> CreateAsync(T entity)
        {
            var entidadeCriada = await _context.AddAsync(entity);

            if (entidadeCriada == null)
                return null;

            return await Task.FromResult(entity);
        }

        /// <summary>
        /// Lista todas as entidades do tipo especificado no contexto do banco de dados.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo uma lista de todas as entidades.</returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Salva todas as alterações feitas no contexto do banco de dados.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona de salvar as alterações.</returns>
        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.InnerException?.Message ?? ex.Message);
            }
        }

        /// <summary>
        /// Retorna a entidade do tipo especificado com o identificador fornecido.
        /// </summary>
        /// <param name="Id">O identificador da entidade a ser recuperada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade com o identificador fornecido.</returns>
        public async Task<T> Get(int Id) => await _context.Set<T>().FindAsync(Id);

    }
}
