using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class AlignmentStrength
    {
        public AlignmentStrengthId AlignmentStrengthId { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore] public ICollection<Alignment> Alignments { get; set; } = new List<Alignment>();
    }
}
