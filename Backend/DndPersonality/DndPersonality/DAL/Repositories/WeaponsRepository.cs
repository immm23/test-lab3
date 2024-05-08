using DndPersonality.Models.DALModels.Components;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Repositories
{
    public class WeaponsRepository : EntitiesRepository<Weapon>
    {
        public WeaponsRepository(Context context)
            : base(context) { }

        public override async Task<IEnumerable<Weapon>> AllAsync()
        {
            return await Context.Weapons
                .Include(p => p.DamageType).AsNoTracking()
                .Include(p => p.WeaponType).AsNoTracking()
                .ToListAsync();
        }
    }
}
