using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.System;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Tool : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }

        [JsonIgnore] public ICollection<CharacterClass>? Classes { get; set; } = new List<CharacterClass>();
        [JsonIgnore] public ICollection<ToolClass>? ToolClasses { get; set; } = new List<ToolClass>();
    }
}
