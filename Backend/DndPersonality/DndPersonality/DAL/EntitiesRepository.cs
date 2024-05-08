using DndPersonality.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL
{
    public abstract class EntitiesRepository<T> : IEntitiesRepository<T> where T : class, IEntity
    {
        protected readonly Context Context;
        public EntitiesRepository(Context context) 
        { 
            Context = context;
        }

        public virtual async Task AddAsync(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(int id)
        {
            var entity = await Context.Set<T>().FirstAsync(p => p.Id == id);
            Context.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> AllAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        protected async Task<List<TChild>> RetrieveChildListFromContextAsync<TChild>(
            List<TChild> inputChildren) where TChild : class, IEntity
        {
            List<TChild> outputChildren = new List<TChild>();
            var contextSet = Context.Set<TChild>();

            foreach (var child in inputChildren)
            {
                var dbChild = await contextSet.FirstOrDefaultAsync(p => p.Id == child.Id);
                outputChildren.Add(dbChild ?? child);
            }

            return outputChildren;
        }
    }
}
