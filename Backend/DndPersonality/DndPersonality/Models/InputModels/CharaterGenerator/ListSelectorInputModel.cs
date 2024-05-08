namespace DndPersonality.Models.InputModels.CharaterGenerator
{
    public class ListSelectorInputModel
    {
        public int Id { get; set; }
        public List<int> Selections { get; set; } = new List<int>();
    }
}
