using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DndPersonality.Controllers.Character;
using DndPersonality.DAL.Interfaces;
using DndPersonality.Models.DALModels.Components;
using DndPersonality.Models.InputModels.CharaterGenerator;
using DndPersonality.CharacterGenerator;
using DndPersonality;


public class CharactersControllerTests
{
    private readonly Mock<ICharacterRepository> _mockCharacterRepository = new Mock<ICharacterRepository>();
    private readonly Mock<IOpenAIService> _mockOpenAIService = new Mock<IOpenAIService>();
    private readonly Mock<IEntitiesRepository<Language>> _mockLanguageRepository = new Mock<IEntitiesRepository<Language>>();
    private readonly CharactersController _controller;

    public CharactersControllerTests()
    {
        _controller = new CharactersController(
            _mockCharacterRepository.Object,
            _mockLanguageRepository.Object,
            _mockOpenAIService.Object);
    }

    [Fact]
    public async Task GenerateConstructor_ReturnsOk_WithValidData()
    {
        var originInputModel = new OriginInputModel(); // Populate as necessary
        var origin = new Origin(); // Assume this is a valid return object
        var languages = new List<Language> { new Language(), new Language() };

        _mockCharacterRepository.Setup(x => x.ComposeOriginAsync(originInputModel)).ReturnsAsync(origin);
        _mockLanguageRepository.Setup(x => x.AllAsync()).ReturnsAsync(languages);

        var result = await _controller.GenerateContructor(originInputModel) as OkObjectResult;

        Assert.NotNull(result);
        Assert.IsType<List<object>>(result.Value);
        Assert.Equal(200, result.StatusCode);
        _mockCharacterRepository.Verify(x => x.ComposeOriginAsync(It.IsAny<OriginInputModel>()), Times.Once);
    }

    [Fact]
    public async Task GenerateConstructor_ThrowsKeyNotFoundException()
    {
        var originInputModel = new OriginInputModel();
        _mockCharacterRepository.Setup(x => x.ComposeOriginAsync(originInputModel))
            .ThrowsAsync(new KeyNotFoundException("Origin not found"));

        var result = await _controller.GenerateContructor(originInputModel) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal("Origin not found", result.Value);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task GenerateCharacter_ReturnsOk_WhenStoryAndImageGenerated()
    {
        var characterInputModel = new CharacterInputModel { GenerateStory = true, GenerateImage = true, Selections = new() };
        var origin = new Origin(new DndPersonality.Models.DALModels.Race(), new DndPersonality.Models.DALModels.CharacterClass());
        var languages = new List<Language> { new Language(), new Language() };

        _mockCharacterRepository.Setup(x => x.ComposeOriginAsync(It.IsAny<OriginInputModel>())).ReturnsAsync(origin);
        _mockLanguageRepository.Setup(x => x.AllAsync()).ReturnsAsync(languages);
        _mockOpenAIService.Setup(x => x.GenerateCharacterStory(origin, It.IsAny<Character>())).ReturnsAsync("Generated Story");
        _mockOpenAIService.Setup(x => x.GenerateCharacterImage(origin, It.IsAny<Character>())).ReturnsAsync("http://image.url");

        var result = await _controller.GenerateCharacter(characterInputModel) as OkObjectResult;

        Assert.NotNull(result);
        Assert.IsType<Character>(result.Value);
        Assert.Equal(200, result.StatusCode);
        _mockCharacterRepository.Verify(x => x.ComposeOriginAsync(It.IsAny<OriginInputModel>()), Times.Once);
        _mockOpenAIService.Verify(x => x.GenerateCharacterStory(origin, It.IsAny<Character>()), Times.Once);
        _mockOpenAIService.Verify(x => x.GenerateCharacterImage(origin, It.IsAny<Character>()), Times.Once);
    }

}
