using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.Components;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Components
{
    [ApiController]
    public class ArmorsController : EntitiesController<ArmorInputModel, Armor>
    {
        public ArmorsController(IEntitiesRepository<Armor> repository,
            IMapper mapper)
            : base(repository, mapper) { }
    }
}
