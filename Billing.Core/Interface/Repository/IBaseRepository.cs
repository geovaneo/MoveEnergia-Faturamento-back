using System.Linq.Expressions;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Inclui a entidade fornecida no contexto do banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser incluída.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade incluída.</returns>
        Task<T> CreateAsync(T entity);

        /// <summary>
        /// Altera a entidade fornecida no contexto do banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser alterada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade alterada.</returns>
        Task<T> UpdateAsync(T entity);

        Task<T> Remove(int id);

        /// <summary>
        /// Exclui a entidade fornecida do contexto do banco de dados.
        /// </summary>
        /// <param name="entity">A entidade a ser excluída.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade excluída.</returns>
        Task<T> DeleteAsync(T entity);

        /// <summary>
        /// Lista todas as entidades do tipo especificado no contexto do banco de dados.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo uma lista de todas as entidades.</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Retorna a entidade do tipo especificado com o identificador fornecido.
        /// </summary>
        /// <param name="Id">O identificador da entidade a ser recuperada.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona, contendo a entidade com o identificador fornecido.</returns>
        Task<T> Get(int Id);

        /// <summary>
        /// Salva todas as alterações feitas no contexto do banco de dados.
        /// </summary>
        /// <returns>Uma tarefa que representa a operação assíncrona de salvar as alterações.</returns>
        Task SaveAsync();

    }
}
