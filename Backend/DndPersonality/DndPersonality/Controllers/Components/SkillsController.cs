using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Components
{
    [ApiController]
    public class SkillsController : EntitiesController<Skill, Skill>
    {
        public SkillsController(IEntitiesRepository<Skill> repository,
            IMapper mapper)
            : base(repository, mapper) { }
    }
}
