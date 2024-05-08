using DndPersonality.Models.DALModels;
using DndPersonality.Models.DALModels.Subrace;

namespace DndPersonality.CharacterGenerator
{
    public class Origin
    {
        public Race Race { get; set; } = null!;
        public CharacterClass CharacterClass { get; set; } = null!;
        public Subrace? Subrace { get; set; }

        public Origin(Race race, CharacterClass characterClass, Subrace? subrace = null)
        {
            Race = race;
            CharacterClass = characterClass;
            Subrace = subrace;
        }
        public Origin()
        {
                
        }
    }
}
