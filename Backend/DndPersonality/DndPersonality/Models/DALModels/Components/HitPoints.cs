using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class HitPoints
    {
        public int Id { get; set; }
        public int Default { get; set; }
        public Ability Ability { get; set; } = null!;

        [JsonIgnore] public AbilityId AbilityId { get; set; }
        [JsonIgnore] public CharacterClass? Class { get; set; }
    }
}
