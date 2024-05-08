using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class ArmorType
    {
        public string Name { get; set; } = null!;
        public ArmorTypeId ArmorTypeId { get; set; }

        [JsonIgnore] public ICollection<Armor> Armors { get; set; } = new List<Armor>();
        [JsonIgnore] public ICollection<CharacterClass> Classes { get; set; } = new List<CharacterClass>();
    }
}
