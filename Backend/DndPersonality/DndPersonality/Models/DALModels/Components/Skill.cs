using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.System;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Skill : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore] public ICollection<SkillClass>? SkillClasses { get; set; } = new List<SkillClass>();
        [JsonIgnore] public ICollection<CharacterClass>? Classes { get; set; } = new List<CharacterClass>();

    }
}
