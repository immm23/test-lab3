using AutoMapper;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.InputModels.Components;
using DndPersonality.Models.InputModels;
using System.Configuration;
using DndPersonality.Models.DALModels.Subrace;

namespace DndPersonality.Models
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Language, Language>();
            CreateMap<Skill, Skill>();
            CreateMap<WeaponInputModel, Weapon>();
            CreateMap<ArmorInputModel, Armor>();
            CreateMap<Tool, Tool>();
            CreateMap<CharacterClassInputModel, CharacterClass>();
            CreateMap<AlignmentSideId, AlignmentSide>()
                .ForMember(dest => dest.AlignmentSideId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Name, opt => opt.Ignore());
            CreateMap<AlignmentStrengthId, AlignmentStrength>()
                .ForMember(dest => dest.AlignmentStrengthId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Name, opt => opt.Ignore());
            CreateMap<AbilityId, Ability>()
                .ForMember(dest => dest.AbilityId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Name, opt => opt.Ignore());
            CreateMap<ArmorTypeId, ArmorType>()
                .ForMember(dest => dest.ArmorTypeId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Name, opt => opt.Ignore());
            CreateMap<WeaponTypeId, WeaponType>()
                .ForMember(dest => dest.WeaponTypeId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Name, opt => opt.Ignore());

            CreateMap<HitPointsInputModel, HitPoints>();
            CreateMap<WeaponSelectorInputModel, WeaponSelector>();

            CreateMap<RaceInputModel, Race>();
            CreateMap<RaceBonusInputModel, RaceBonus>();
            CreateMap<AlignmentInputModel, Alignment>();

            CreateMap<SubraceInputModel, Subrace>();
        }
    }
}
