using System.Text.Json.Serialization;
using DndPersonality.Models.DALModels.System;

namespace DndPersonality.Models.DALModels.Components
{
    public class WeaponSelector
    {
        public int Id { get; set; }
        public List<Weapon> Weapons { get; set; } = new List<Weapon>();

        [JsonIgnore] public CharacterClass? Class { get; set; }
        [JsonIgnore] public ICollection<WeaponSelectorWeapon>? WeaponSelectorWeapons { get; set; } =
            new List<WeaponSelectorWeapon>();
    }
}
