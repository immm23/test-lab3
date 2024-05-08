using DndPersonality.CharacterGenerator;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.InputModels.CharaterGenerator;
using Microsoft.EntityFrameworkCore;

namespace DndPersonality.DAL.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        
        private readonly Context _context;

        public CharacterRepository(Context context)
        {
            _context= context;
        }

        public async Task<Origin> ComposeOriginAsync(OriginInputModel originInputModel)
        {
            var race = await _context.Races
                .Include(p => p.Names)
                .Include(p => p.RaceBonus)
                    .ThenInclude(p => p.Ability)
                .FirstOrDefaultAsync(p => p.Id == originInputModel.RaceId);
            var characterClass = await _context.Classes
                .Include(p => p.HitPoints)
                .Include(p => p.ArmorTypes)
                .Include(p => p.WeaponTypes)
                .Include(p => p.StartItems)
                .Include(p => p.Tools)
                .Include(p => p.Skills)
                .Include(p => p.WeaponSelectors)
                    .ThenInclude(p => p.Weapons)
                        .ThenInclude(p => p.WeaponType)
                .Include(p => p.WeaponSelectors)
                    .ThenInclude(p => p.Weapons)
                        .ThenInclude(p => p.DamageType)
                .Include(p => p.PrimaryAbility)
                .Include(p => p.SavingThrow)
                .FirstOrDefaultAsync(p => p.Id == originInputModel.ClassId);
            var subrace = await _context.Subraces
                .Include(p => p.RaceBonus)
                    .ThenInclude(p => p.Ability)
                .FirstOrDefaultAsync(p => p.Id == originInputModel.RaceId);
            
            if(race is not null && characterClass is not null)
            {
                return new Origin(race, characterClass, subrace);
            }
            else
            {
                var notFoundObject = nameof(race) == null ? nameof(race) : nameof(characterClass);
                throw new KeyNotFoundException($"Object was not found in database, " +
                    $"{notFoundObject}, with origin {originInputModel}");
            }
        }
    }
}
