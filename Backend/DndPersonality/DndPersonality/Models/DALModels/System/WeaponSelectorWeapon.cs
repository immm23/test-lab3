using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.DALModels.System
{
    public class WeaponSelectorWeapon
    {
        public int WeaponSelectorId { get; set; }
        public WeaponSelector WeaponSelector { get; set; } = null!;

        public int WeaponId { get; set; }
        public Weapon Weapon { get; set; } = null!;
    }
}
