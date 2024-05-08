using DndPersonality.Models.DALModels.Components;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Repositories
{
    public class ArmorsRepository : EntitiesRepository<Armor>
    {
        public ArmorsRepository(Context context)
            : base(context) { }

        public override async Task<IEnumerable<Armor>> AllAsync()
        {
            return await Context.Armors
                .Include(p => p.ArmorType).AsNoTracking()
                .Include(p => p.BonusAbility).AsNoTracking()
                .ToListAsync();
        }
    }
}
