namespace DndPersonality.Models.InputModels.CharaterGenerator
{
    public class CharacterInputModel
    {
        public OriginInputModel Origin { get; set; } = null!;
        public SelectionInputModel Selections { get; set; } = null!;
        public bool GenerateImage { get; set; } = false;
        public bool GenerateStory { get; set; } = false;
    }
}
