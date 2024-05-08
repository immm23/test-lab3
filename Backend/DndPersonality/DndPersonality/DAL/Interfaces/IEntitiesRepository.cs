using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Interfaces
{
    public interface IEntitiesRepository<T>
    {
        public Task AddAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task RemoveAsync(int id);
        public Task<IEnumerable<T>> AllAsync();
    }
}
