namespace DndPersonality.DAL.Interfaces
{
    public interface IUpdatable<in T> where T : class, IEntity
    {
        public void Update(T type);
    }
}
