using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Subrace;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Repositories
{
    public class SubracesRepository : EntitiesRepository<Subrace>
    {
        public SubracesRepository(Context context) 
            : base(context) { }

        public override async Task AddAsync(Subrace entity)
        {
            await Context.Subraces.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public override async Task<IEnumerable<Subrace>> AllAsync()
        {
            return await Context.Subraces
                .Include(p => p.RaceBonus)
                    .ThenInclude(p => p.Ability).AsNoTracking()
                .ToListAsync();
        }

        public override async Task UpdateAsync(Subrace entity)
        {
            var dbEntity = Context.Subraces.Include(p => p.RaceBonus).First(p => p.Id == entity.Id);
            dbEntity.Update(entity);
            await Context.SaveChangesAsync();
        }

    }
}
