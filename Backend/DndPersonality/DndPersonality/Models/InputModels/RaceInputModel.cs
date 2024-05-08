using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.Components;

namespace DndPersonality.Models.InputModels
{
    public class RaceInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Name> Names { get; set; } = new List<Name>();
        public RaceBonusInputModel? RaceBonus { get; set; }
        public string AgeDescription { get; set; } = null!;
        public AlignmentInputModel Alignment { get; set; } = null!;
        public string SizeDescription { get; set; } = null!;
        public string? DarkVision { get; set; }
        public int Speed { get; set; }
        public string Description { get; set; } = null!;
        public List<Language> NativeLanguages { get; set; } = new List<Language>();
        public int AdditionalLanguages { get; set; }
        public string? ToolsBonus { get; set; }
        public string? AdditionalBonus { get; set; }
        public string? ImagePath { get; set; }
    }
}
