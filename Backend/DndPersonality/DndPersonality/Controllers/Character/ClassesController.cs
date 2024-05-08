using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Character
{
    [ApiController]
    public class ClassesController : EntitiesController<CharacterClassInputModel, CharacterClass>
    { 
        public ClassesController(IEntitiesRepository<CharacterClass> repository,
            IMapper mapper)
            : base(repository, mapper) { }
    }
}
