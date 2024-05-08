using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.DAL.Repositories
{
    public class ItemsRepository : EntitiesRepository<Item>
    {
        public ItemsRepository(Context context) : base(context) { }
    }
}
