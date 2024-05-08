using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class RaceBonus
    {
        public int Id { get; set; }
        public Ability Ability { get; set; } = null!;
        public int Amount { get; set; }

        [JsonIgnore] public AbilityId AbilityId { get; set; }
        [JsonIgnore] public Race? Race { get; set; }
        [JsonIgnore] public Subrace.Subrace? Subrace { get; set; }
    }
}
