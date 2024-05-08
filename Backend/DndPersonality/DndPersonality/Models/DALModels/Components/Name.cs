using DndPersonality.DAL.Interfaces;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels.Components
{
    public class Name : IEntity
    {
        public int Id { get; set; }
        public string RaceName { get; set; } = null!;

        [JsonIgnore] public Race? Race { get; set; }
    }
}
