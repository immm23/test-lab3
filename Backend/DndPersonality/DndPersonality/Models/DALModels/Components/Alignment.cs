using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Alignment
    {
        public int Id { get; set; }
        public AlignmentSide AlignmentSide { get; set; } = null!;
        public AlignmentStrength AlignmentStrength { get; set; } = null!;

        [JsonIgnore] public AlignmentSideId AlignmentSideId { get; set; }
        [JsonIgnore] public AlignmentStrengthId AlignmentStrengthId { get; set; }
        [JsonIgnore] public ICollection<Race>? Races { get; set; } = new List<Race>();
    }
}
