using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class WeaponType
    {
        public WeaponTypeId WeaponTypeId { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore] public ICollection<Weapon> Weapons { get; set; } = new List<Weapon>();
        [JsonIgnore] public ICollection<CharacterClass> Classes { get; set; } = new List<CharacterClass>();
    }
}
