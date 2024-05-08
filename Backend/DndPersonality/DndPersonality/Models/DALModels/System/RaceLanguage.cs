using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.Models.DALModels.System
{
    public class RaceLanguage
    {
        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!; 

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;
    }
}
