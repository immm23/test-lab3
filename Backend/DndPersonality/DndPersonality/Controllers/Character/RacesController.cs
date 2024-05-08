using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels;
using DndPersonality.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Character
{
    [ApiController]
    public class RacesController : EntitiesController<RaceInputModel, Race>
    {
        public RacesController(IEntitiesRepository<Race> repository,
            IMapper mapper)
            : base(repository, mapper) {}
    }
}
