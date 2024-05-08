using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels.System;
using System.Text.Json.Serialization;

namespace DndPersonality.Models.DALModels
{
    public class Race : IEntity, IUpdatable<Race>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Name> Names { get; set; } = new List<Name>();
        public int RaceBonusId { get; set; }
        public RaceBonus RaceBonus { get; set; } = null!;
        public string AgeDescription { get; set; } = null!;
        public Alignment Alignment { get; set; } = null!;
        public string SizeDescription { get; set; } = null!;
        public string? DarkVision { get; set; }
        public int Speed { get; set; }
        public string Description { get; set; } = null!;
        public List<Language> NativeLanguages { get; set; } = new List<Language>();
        public int AdditionalLanguages { get; set; }
        public string? ToolsBonus { get; set; }
        public string? AdditionalBonus { get; set; }
        public string ImagePath { get; set; } = null!;
        public List<Subrace.Subrace>? Subraces { get; set; } = new List<Subrace.Subrace>();

        [JsonIgnore] public ICollection<RaceLanguage>? RaceLanguages { get; set; } =
            new List<RaceLanguage>();

        public void Update(Race entity)
        {
            Name = entity.Name;
            Names = entity.Names;
            SizeDescription = entity.SizeDescription;
            DarkVision = entity.DarkVision;
            Speed = entity.Speed;
            AgeDescription = entity.AgeDescription;
            Alignment = entity.Alignment;
            RaceBonus = entity.RaceBonus;
            Description = entity.Description;
            AdditionalLanguages = entity.AdditionalLanguages;
            ToolsBonus = entity.ToolsBonus;
            AdditionalBonus = entity.AdditionalBonus;
        }
    }
}
