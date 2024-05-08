using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.DALModels.System
{
    public class ToolClass
    {
        public int ToolId { get; set; }
        public Tool Tool { get; set; } = null!;

        public int ClassId { get; set; }
        public CharacterClass CharacterClass { get; set; } = null!;
    }
}
