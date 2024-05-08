using DndPersonality.Models.DALModels.Components;

namespace DndPersonality.DAL.Repositories
{
    public class LanguagesRepository : EntitiesRepository<Language>
    {
        public LanguagesRepository(Context context)
            : base(context) { }
    }
}
