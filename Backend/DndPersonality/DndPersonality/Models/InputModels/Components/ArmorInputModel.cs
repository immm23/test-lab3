using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.InputModels.Components
{
    public class ArmorInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int BasePoints { get; set; }
        public AbilityId? BonusAbilityId { get; set; }
        public ArmorTypeId ArmorTypeId { get; set; }
        public int Price { get; set; }
    }
}
