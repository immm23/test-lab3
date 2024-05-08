using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.Subrace;
using DndPersonality.Models.DALModels;

namespace DndPersonality.Models.InputModels.Components
{
    public class RaceBonusInputModel
    {
        public AbilityId AbilityId { get; set; }
        public int Amount { get; set; }
    }
}
