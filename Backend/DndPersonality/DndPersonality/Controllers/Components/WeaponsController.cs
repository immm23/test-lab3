using AutoMapper;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.Components;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Components
{
    [ApiController]
    public class WeaponsController : EntitiesController<WeaponInputModel, Weapon>
    {
       public WeaponsController(IEntitiesRepository<Weapon> repository,
           IMapper mapper)
            : base(repository, mapper) { }
    }
}
