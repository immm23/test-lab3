using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.DALModels.System
{
    public class ItemClass
    {
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public int ClassId { get; set; }
        public CharacterClass CharacterClass { get; set; } = null!;
    }
}
