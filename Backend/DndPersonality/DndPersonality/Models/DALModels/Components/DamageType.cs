using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class DamageType
    {
        public string Name { get; set; } = null!; 
        public DamageTypeId DamageTypeId { get; set; }

        [JsonIgnore] public List<Weapon>? Weapons { get; set; } = new List<Weapon>();
    }
}
