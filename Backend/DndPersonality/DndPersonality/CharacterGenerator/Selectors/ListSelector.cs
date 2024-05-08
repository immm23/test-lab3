using DndPersonality.DAL.Interfaces;

namespace DndPersonality.CharacterGenerator.Selectors
{
    public class ListSelector<T> : Selector where T : class, IEntity
    {
        public int Amount { get; private set; }
        public List<T> Items { get; private set; }
        public ListSelector(int id, List<T> items, int amount, string name)
            : base(id, name)
        {
            Items = items;
            Amount = amount;
        }

        public override void Select(object selection)
        {
            if (selection is not List<int> ||
                ((List<int>)selection).Count != Amount)
            {
                throw new ArgumentException($"Wrong selection provided, {nameof(selection)}");
            }

            List<int> selected = (List<int>)selection;
            Items = Items.Where(p => selected.Contains(p.Id)).ToList();

            IsSelected = true;
        }

        public override object GetSelection()
        {
            if (!IsSelected)
            {
                throw new InvalidOperationException("Can not provide selection from not selected selector");
            }

            return Items;
        }
    }
}
