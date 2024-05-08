using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.System;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels
{
    public class CharacterClass: IEntity, IUpdatable<CharacterClass>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int ProficiencyBonus { get; set; }
        public Ability PrimaryAbility { get; set; } = null!;
        public Ability SavingThrow { get; set; } = null!;
        public string HitDice { get; set; } = null!;
        public HitPoints HitPoints { get; set; } = null!;
        public List<ArmorType> ArmorTypes { get; set; } = new List<ArmorType>();
        public List<WeaponType> WeaponTypes { get; set; } = new List<WeaponType>();
        public List<Tool> Tools { get; set; } = new List<Tool>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public int SelectedSkills { get; set; }
        public string? AdditionalAbilities { get; set; }
        public List<WeaponSelector> WeaponSelectors { get; set; } = new List<WeaponSelector>();
        public List<Item> StartItems { get; set; } = new List<Item>();
        public string ImagePath { get; set; } = null!;

        [JsonIgnore] public ICollection<ItemClass>? ItemClasses { get; set; } = new List<ItemClass>();
        [JsonIgnore] public AbilityId PrimaryAbilityId { get; set; }
        [JsonIgnore] public ICollection<SkillClass>? SkillClasses { get; set; } = new List<SkillClass>();
        [JsonIgnore] public AbilityId SavingThrowId { get; set; }
        [JsonIgnore] public ICollection<ToolClass>? ToolClasses { get; set; } = new List<ToolClass>();


        public void Update(CharacterClass entity)
        {
            Name  = entity.Name;
            Description = entity.Description;
            ProficiencyBonus = entity.ProficiencyBonus;
            PrimaryAbilityId = entity.PrimaryAbilityId;
            SavingThrowId = entity.SavingThrowId;
            HitDice = entity.HitDice;
            HitPoints.AbilityId = entity.HitPoints.AbilityId;
            HitPoints.Default = entity.HitPoints.Default;
            SelectedSkills = entity.SelectedSkills;
            AdditionalAbilities = entity.AdditionalAbilities;
        }
    }
}
