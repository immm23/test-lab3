using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Subrace
{
    public class Subrace: IEntity, IUpdatable<Subrace>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int RaceBonusId { get; set; }
        public RaceBonus RaceBonus { get; set; } = null!;
        public string SpecialAbility { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int RaceId { get; set; }
        public string ImagePath { get; set; } = null!;

        [JsonIgnore] public Race? Race { get; set; }

        public void Update(Subrace entity)
        {
            Name = entity.Name;
            RaceBonus = entity.RaceBonus;
            SpecialAbility= entity.SpecialAbility;
            Description = entity.Description;
        }
    }
}