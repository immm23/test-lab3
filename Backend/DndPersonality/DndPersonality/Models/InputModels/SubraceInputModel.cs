using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.InputModels.Components;

namespace DndPersonality.Models.InputModels
{
    public class SubraceInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; 
        public RaceBonusInputModel RaceBonus { get; set; } = null!;
        public string SpecialAbility { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImagePath { get; set; }
        public int RaceId { get; set; }
    }
}
