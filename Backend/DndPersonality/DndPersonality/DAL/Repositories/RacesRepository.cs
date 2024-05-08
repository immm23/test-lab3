using DndPersonality.Models.DALModels;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Repositories
{
    public class RacesRepository : EntitiesRepository<Race>
    {
        public RacesRepository(Context context) 
            : base(context) { }

        public override async Task AddAsync(Race entity)
        {
            entity.NativeLanguages = await RetrieveChildListFromContextAsync(entity.NativeLanguages);

            await Context.Races.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Race entity)
        {
            var dbEntity = Context.Races
                .Include(p => p.NativeLanguages)
                .Include(p => p.Alignment)
                .Include(p => p.Alignment)
                    .ThenInclude(p => p.AlignmentSide)
                .Include(p => p.Alignment)
                    .ThenInclude(p => p.AlignmentStrength)
                .Include(p => p.RaceBonus)
                .Include(p => p.Names)
                .First(p => p.Id == entity.Id);

            dbEntity.NativeLanguages = await RetrieveChildListFromContextAsync(entity.NativeLanguages);
            dbEntity.Update(entity);

            await Context.SaveChangesAsync();
        }

        public override async Task<IEnumerable<Race>> AllAsync()
        {
            return await Context.Races
                .Include(p => p.RaceBonus)
                .ThenInclude(p => p.Ability).AsNoTracking()
                .Include(p => p.Alignment)
                    .ThenInclude(p => p.AlignmentStrength).AsNoTracking()
                .Include(p => p.Alignment)
                    .ThenInclude(p => p.AlignmentSide).AsNoTracking()
                .Include(p => p.Names).AsNoTracking()
                .Include(p => p.NativeLanguages).AsNoTracking()
                .ToListAsync();
        }
    }
}
