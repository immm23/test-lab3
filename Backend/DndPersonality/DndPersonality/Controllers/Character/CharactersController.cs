using DndPersonality.CharacterGenerator;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.CharaterGenerator;
using Microsoft.AspNetCore.Mvc;

namespace DndPersonality.Controllers.Character
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {

        private readonly ICharacterRepository _characterRepository;
        private readonly IOpenAIService _openAIService;
        private readonly IEntitiesRepository<Language> _languageRepository;

        public CharactersController(ICharacterRepository characterRepository,
            IEntitiesRepository<Language> languageRepository,
            IOpenAIService openAIService)
        {
            _characterRepository = characterRepository;
            _languageRepository = languageRepository;
            _openAIService = openAIService;
        }

        [HttpPost]
        [Route("constuctors/generate/")]
        public async Task<IActionResult> GenerateContructor(OriginInputModel originInputModel)
        {
            try
            {
                var origin = await _characterRepository.ComposeOriginAsync(originInputModel);
                var languages = await _languageRepository.AllAsync();
                Constructor constructor = new(origin, languages.ToList());

                var selectors = constructor.Selectors.Cast<object>().ToList();

                return Ok(selectors);
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch(Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateCharacter(CharacterInputModel characterInputModel)
        {
            try
            {
                var origin = await _characterRepository.ComposeOriginAsync(characterInputModel.Origin);
                var languages = await _languageRepository.AllAsync();
                Constructor constructor = new(origin, languages.ToList());

                var validatedSelectors = constructor.ValidateSelectionModel(characterInputModel.Selections);
                CharacterGenerator.Character character = new(origin, validatedSelectors);
                
                if (characterInputModel.GenerateStory is true)
                {
                    character.GeneratedStory = await _openAIService.GenerateCharacterStory(origin, character);
                }

                if (characterInputModel.GenerateImage is true)
                {
                    character.GeneratedImageUrl = await _openAIService.GenerateCharacterImage(origin,character);
                }

                return Ok(character);

            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
