using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Components
{
    [ApiController]
    public class ItemsController : EntitiesController<Item, Item>
    {
        public ItemsController(IEntitiesRepository<Item> repository,
            IMapper mapper)
            : base(repository, mapper) { }
    }
}
