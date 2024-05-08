using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.System;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Weapon : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public WeaponType WeaponType { get; set; } = null!;
        public DamageType DamageType { get; set; } = null!;
        public string Damage { get; set; } = null!;
        public int Price { get; set; }

        [JsonIgnore] public DamageTypeId DamageTypeId { get; set; }
        [JsonIgnore] public WeaponTypeId WeaponTypeId { get; set; }
        [JsonIgnore] public ICollection<WeaponSelector>? WeaponSelectors { get; set; } =
            new List<WeaponSelector>();
        [JsonIgnore] public ICollection<WeaponSelectorWeapon>? WeaponSelectorWeapons { get; set; } = 
            new List<WeaponSelectorWeapon>();
    }
}
