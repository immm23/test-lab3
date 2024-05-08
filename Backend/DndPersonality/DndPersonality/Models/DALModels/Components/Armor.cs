using DndPersonality.DAL.Interfaces;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Armor : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BasePoints { get; set; }
        public Ability BonusAbility { get; set; } = null!;
        public ArmorType ArmorType { get; set; } = null!;
        public int Price { get; set; }

        [JsonIgnore] public AbilityId BonusAbilityId { get; set; }
        [JsonIgnore] public ArmorTypeId ArmorTypeId { get; set; }
    }
}
