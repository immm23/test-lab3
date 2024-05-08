using DndPersonality.DAL.Interfaces;

namespace DndPersonality.CharacterGenerator.Selectors
{
    public abstract class Selector
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        protected bool IsSelected = false;

        public Selector(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract void Select(object selection);
        public abstract object GetSelection();
    }
}
