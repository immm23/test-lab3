using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.System;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Language : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore] public ICollection<Race>? Races { get; set; } = new List<Race>();

        [JsonIgnore] public ICollection<RaceLanguage>? RaceLanguages { get; set; } = new List<RaceLanguage>();
    }
}
