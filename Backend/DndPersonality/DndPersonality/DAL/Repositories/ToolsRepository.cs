using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.DAL.Repositories
{
    public class ToolsRepository : EntitiesRepository<Tool>
    {
        public ToolsRepository(Context context) 
            : base(context) { }
    }
}
