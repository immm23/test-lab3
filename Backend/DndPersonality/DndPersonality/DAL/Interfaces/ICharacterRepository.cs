using DndPersonality.CharacterGenerator;
using DndPersonality.Models.InputModels.CharaterGenerator;

namespace DndPersonality.DAL.Interfaces
{
    public interface ICharacterRepository
    {
        public Task<Origin> ComposeOriginAsync(OriginInputModel originInputModel);
    }
}
