using System.Text.Json.Serialization;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.System;

namespace DndPersonality.Models.DALModels.Components
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore] public ICollection<CharacterClass>? Classes { get; set; } = new List<CharacterClass>();

        [JsonIgnore] public ICollection<ItemClass>? ItemClasses { get; set; } = new List<ItemClass>();
    }
}
