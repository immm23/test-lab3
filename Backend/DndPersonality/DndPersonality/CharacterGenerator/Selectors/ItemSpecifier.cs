using DndPersonality.DAL.Interfaces;
using System.Text.Json;

namespace DndPersonality.CharacterGenerator.Selectors
{
    public class ItemSpecifier<T> : Selector
    {
        public T Item { get; private set; }
        public ItemSpecifier(int id, T item, string name)
            : base(id, name)
        {
            Item = item;
        }

        public override void Select(object selection)
        {
            var stringifiedSelection = selection.ToString();
            if (stringifiedSelection is not null)
            {
                try
                {
                    var result = JsonSerializer.Deserialize<T>(stringifiedSelection);
                    Item = result ?? throw new ArgumentException("Empty or invalid selection provided");
                }
                catch
                {
                    throw new ArgumentException("Wrong, non readable selection provided");
                }
                
            }

            IsSelected = true;

        }

        public override object GetSelection()
        {
            if (!IsSelected || Item is null)
            {
                throw new InvalidOperationException("Can not provide selection from not selected selector");
            }

            return Item;
        }
    }
}
