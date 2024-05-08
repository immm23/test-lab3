using AutoMapper;
using DndPersonality.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntitiesController<TInputModel, TDalModel> : ControllerBase
        where TInputModel : class
        where TDalModel : class, IEntity
    {
        protected readonly IEntitiesRepository<TDalModel> Repository;
        protected readonly IMapper Mapper;

        public EntitiesController(IEntitiesRepository<TDalModel> repository,
            IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        [Route("new")]
        public virtual async Task<IActionResult> Create(TInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(500, ModelState);
            }

            var entity = Mapper.Map<TDalModel>(inputModel);
            await Repository.AddAsync(entity);
            return CreatedAtAction(nameof(Create), entity);
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public virtual async Task<IActionResult> Update(TInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(500, ModelState);
            }

            var entity = Mapper.Map<TDalModel>(inputModel);

            try
            {
                await Repository.UpdateAsync(entity);
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("delete/{id:int}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Repository.RemoveAsync(id);
                return Ok();
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }

        }

        [HttpGet]
        [Route("all")]
        public virtual async Task<IActionResult> GetAll()
        {
            var entities = await Repository.AllAsync();
            return Ok(entities);
        }
    }
}
