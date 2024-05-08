using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.InputModels.Components
{
    public class WeaponInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public WeaponTypeId WeaponTypeId { get; set; }
        public DamageTypeId DamageTypeId { get; set; }
        public string Damage { get; set; } = null!;
        public int Price { get; set; }
    }
}
