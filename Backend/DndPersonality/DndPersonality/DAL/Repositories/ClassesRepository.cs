using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Components;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Repositories
{
    public class ClassesRepository : EntitiesRepository<CharacterClass>
    {
        public ClassesRepository(Context context) 
            : base(context) { }

        public override async Task AddAsync(CharacterClass entity)
        {
            entity.WeaponTypes = await RetrieveWeaponTypesFromContextAsync(entity.WeaponTypes);
            entity.ArmorTypes = await RetrieveArmorTypesFromContextAsync(entity.ArmorTypes);
            entity.Tools = await RetrieveChildListFromContextAsync(entity.Tools);
            entity.Skills = await RetrieveChildListFromContextAsync(entity.Skills);
            entity.StartItems = await RetrieveChildListFromContextAsync(entity.StartItems);
            entity.WeaponSelectors = await RetrieveWeaponSelectorsFromContextAsync(entity.WeaponSelectors);

            await Context.Classes.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(CharacterClass entity)
        {
            var dbEntity = Context.Classes.Include(p => p.Skills)
                .Include(p => p.ArmorTypes)
                .Include(p => p.WeaponTypes)
                .Include(p => p.Tools)
                .Include(p => p.WeaponSelectors)
                    .ThenInclude(p => p.Weapons)
                        .ThenInclude(p => p.WeaponType)
                .Include(p => p.WeaponSelectors)
                    .ThenInclude(p => p.Weapons)
                        .ThenInclude(p => p.DamageType)
                .Include(p => p.PrimaryAbility)
                .Include(p => p.SavingThrow)
                .Include(p => p.StartItems)
                .Include(p => p.HitPoints)
                    .ThenInclude(p => p.Ability)
                .First(p => p.Id == entity.Id);

            dbEntity.WeaponTypes = await RetrieveWeaponTypesFromContextAsync(entity.WeaponTypes);
            dbEntity.ArmorTypes = await RetrieveArmorTypesFromContextAsync(entity.ArmorTypes);
            dbEntity.Skills = await RetrieveChildListFromContextAsync(entity.Skills);
            dbEntity.Tools = await RetrieveChildListFromContextAsync(entity.Tools);
            dbEntity.StartItems = await RetrieveChildListFromContextAsync(entity.StartItems);
            dbEntity.WeaponSelectors = await RetrieveWeaponSelectorsFromContextAsync(entity.WeaponSelectors);

            dbEntity.Update(entity);

            await Context.SaveChangesAsync(); 
        }

        public override async Task<IEnumerable<CharacterClass>> AllAsync()
        {
            return await Context.Classes
                .Include(p => p.Skills).AsNoTracking()
                .Include(p => p.ArmorTypes).AsNoTracking()
                .Include(p => p.WeaponTypes).AsNoTracking()
                .Include(p => p.Tools).AsNoTracking()
                .Include(p => p.WeaponSelectors)
                    .ThenInclude(p => p.Weapons)
                        .ThenInclude(p => p.WeaponType).AsNoTracking()
                .Include(p => p.WeaponSelectors)
                    .ThenInclude(p => p.Weapons)
                        .ThenInclude(p => p.DamageType).AsNoTracking()
                .Include(p => p.PrimaryAbility).AsNoTracking()
                .Include(p => p.SavingThrow).AsNoTracking()
                .Include(p => p.StartItems).AsNoTracking()
                .Include(p => p.HitPoints)
                    .ThenInclude(p => p.Ability).AsNoTracking()
                .ToListAsync();
        }

        private async Task<List<WeaponType>> RetrieveWeaponTypesFromContextAsync(List<WeaponType> inputWeaponTypes)
        {
            List<WeaponType> outputWeaponTypes = new List<WeaponType>();
            var weaponTypesSet = Context.Set<WeaponType>();

            foreach (var weaponType in inputWeaponTypes)
            {
                var dbWeaponType =
                    await weaponTypesSet.FirstOrDefaultAsync(p => p.WeaponTypeId == weaponType.WeaponTypeId);

                if (dbWeaponType is not null)
                {
                    outputWeaponTypes.Add(dbWeaponType);
                }
            }

            return outputWeaponTypes;
        }

        private async Task<List<ArmorType>> RetrieveArmorTypesFromContextAsync(List<ArmorType> inputArmorTypes)
        {
            List<ArmorType> outputArmorTypes = new List<ArmorType>();
            var armorTypesSet = Context.Set<ArmorType>();

            foreach (var weaponType in inputArmorTypes)
            {
                var dbArmorType =
                    await armorTypesSet.FirstOrDefaultAsync(p => p.ArmorTypeId == weaponType.ArmorTypeId);

                if (dbArmorType is not null)
                {
                    outputArmorTypes.Add(dbArmorType);
                }
            }

            return outputArmorTypes;
        }

        private async Task<List<WeaponSelector>> RetrieveWeaponSelectorsFromContextAsync(
            List<WeaponSelector> inputWeaponSelectors)
        {
            List<WeaponSelector> outputWeaponSelectors = new List<WeaponSelector>();
            var weaponsSelectorsSet = Context.Set<WeaponSelector>();
            var weaponsSet = Context.Set<Weapon>()
                .Include(p => p.WeaponType)
                .Include(p => p.DamageType);

            foreach (var weaponSelector in inputWeaponSelectors)
            {
                var newWeaponSelector = new WeaponSelector();
                foreach (var weapon in weaponSelector.Weapons)
                {
                    var dbWeapon = await weaponsSet.FirstOrDefaultAsync(p => p.Id == weapon.Id);

                    if (dbWeapon is not null)
                    {
                        newWeaponSelector.Weapons.Add(dbWeapon);
                    }

                }

                outputWeaponSelectors.Add(newWeaponSelector);
            }

            return outputWeaponSelectors;
        }
    }
}
