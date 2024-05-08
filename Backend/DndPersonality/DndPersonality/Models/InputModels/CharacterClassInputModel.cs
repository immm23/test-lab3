using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.Components;

namespace DndPersonality.Models.InputModels
{
    public class CharacterClassInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ProficiencyBonus { get; set; }
        public AbilityId PrimaryAbilityId { get; set; }
        public AbilityId SavingThrowId { get; set; }
        public string HitDice { get; set; } = null!;
        public HitPointsInputModel HitPoints { get; set; } = null!;
        public List<ArmorTypeId> ArmorTypes { get; set; } = new List<ArmorTypeId>();
        public List<WeaponTypeId> WeaponTypes { get; set; } = new List<WeaponTypeId>();
        public List<Tool> Tools { get; set; } = new List<Tool>();
        public List<Skill>? Skills { get; set; } = new List<Skill>();
        public int SelectedSkills { get; set; }
        public string AdditionalAbilities { get; set; } = null!;
        public List<WeaponSelectorInputModel> WeaponSelectors { get; set; } =
            new List<WeaponSelectorInputModel>();
        public List<Item> StartItems { get; set; } = new List<Item>();
        public string? ImagePath { get; set; }
    }
}
