using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.DAL.Repositories
{
    public class SkillsRepository : EntitiesRepository<Skill>
    {
        public SkillsRepository(Context context)
            : base(context) { }
    }
}
