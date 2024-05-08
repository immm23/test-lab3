using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Components
{
    [ApiController]
    public class LanguagesController : EntitiesController<Language, Language>
    {
        public LanguagesController(IEntitiesRepository<Language> repository,
            IMapper mapper)
            : base(repository, mapper) { }
    }
}
