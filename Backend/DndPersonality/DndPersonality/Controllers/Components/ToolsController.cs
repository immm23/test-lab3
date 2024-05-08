using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Components
{
    [ApiController]
    public class ToolsController : EntitiesController<Tool, Tool>
    {
        public ToolsController(IEntitiesRepository<Tool> repository,
            IMapper mapper)
            : base(repository, mapper) { }
    }
}
