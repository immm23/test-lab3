using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Ability
    {
        public AbilityId AbilityId { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore] public ICollection<RaceBonus>? RaceBonuses { get; set; } = new List<RaceBonus>();
        [JsonIgnore] public ICollection<CharacterClass>? Classes { get; set; } = new List<CharacterClass>();
        [JsonIgnore]public ICollection<Armor>? Armors { get; set; } = new List<Armor>();
        [JsonIgnore] public ICollection<HitPoints>? HitPoints { get; set; } = new List<HitPoints>();

    }
}
