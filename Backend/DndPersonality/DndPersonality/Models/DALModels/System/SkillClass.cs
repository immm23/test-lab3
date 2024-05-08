using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.DALModels.System
{
    public class SkillClass
    {
        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        public int ClassId { get; set; }
        public CharacterClass CharacterClass { get; set; } = null!;
    }
}
