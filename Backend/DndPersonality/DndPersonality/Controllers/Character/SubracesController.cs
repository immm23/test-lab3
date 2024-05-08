using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Subrace;
using DndPersonality.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Character
{
    [ApiController]
    public class SubracesController : EntitiesController<SubraceInputModel, Subrace>
    {
        public SubracesController(IEntitiesRepository<Subrace> repository,
            IMapper mapper)
            : base(repository, mapper){ }
    }
}
