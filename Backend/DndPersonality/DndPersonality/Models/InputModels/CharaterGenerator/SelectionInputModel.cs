namespace DndPersonality.Models.InputModels.CharaterGenerator
{
    public class SelectionInputModel
    {
        public List<ItemSpecifierInputModel> SpecifiedItems { get; set; } =
    new List<ItemSpecifierInputModel>();
        public List<ListSelectorInputModel> ListSelectedItems { get; set; } =
            new List<ListSelectorInputModel>();
    }
}
