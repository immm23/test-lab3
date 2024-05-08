using DndPersonality.CharacterGenerator;
using Newtonsoft.Json;
using System.Text;

namespace DndPersonality;

public interface IOpenAIService
{
    Task<string> GenerateCharacterImage(Origin origin, Character character);
    Task<string> GenerateCharacterStory(Origin origin, Character character);
}
public class OpenAIService : IOpenAIService
{
    private readonly string _apiKey;

    public OpenAIService(IConfiguration configuration)
	{
        _apiKey = configuration["OpenAPIKey"] ?? throw new ArgumentNullException();
    }

    public async Task<string> GenerateCharacterImage(Origin origin, Character character)
    {
        string url = "https://api.openai.com/v1/images/generations";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);

            var requestData = new
            {
                model = "dall-e-3",
                prompt = GeneratePromtForImageGeneration(origin, character),
                n = 1,
                size = "1024x1024"
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            { 
                var responseAsString = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(responseAsString);
                return json.data?[0]?.url;
            }
            else
            {
                return "";
            }
        }
    }

    public async Task<string> GenerateCharacterStory(Origin origin, Character character)
    {
        string url = "https://api.openai.com/v1/chat/completions";
        string systemPrompt = "You are a helpful dnd assistant. You goal is to provide user with background strory." +
            "You will recive brief description about origin, abilities, age and name and other details. Your ONLY task" +
            "is to create only background story, trying to explain background. You can not ask any details," +
            "you can not answer anything except of story. Reply only with story. Limit is 500 symbols";
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);

            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt},
                    new { role = "user", content = GeneratePromtForImageGeneration(origin, character) },
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseAsString =  await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(responseAsString);
                return json.choices?[0]?.message?.content;
            }
            else
            {
                return "";
            }
        }
    }

    private string GeneratePromtForImageGeneration(Origin origin,Character character)
    {
        var basicString = GenerateCommonCharacterDetailsString(origin, character);
        var story = character.GeneratedStory == null ? "" : character.GeneratedStory;

        if (story is not null)
        {
            basicString += "his backgroud story sounds as following:" + story;
        }

        return basicString;
    }

    private string GenerateCommonCharacterDetailsString(Origin origin, Character character)
    {
        return $"dnd character " +
            $"of {origin.Race.Name} race," +
            $"of {origin.CharacterClass.Name} class," +
            $"is called {character.Name}," +
            $"{character.Height} cm tall, " +
            $"weight {character.Weight} kg, " +
            $"{character.Age} years old, " +
            $"has {character.Alignment.AlignmentSide.Name}-{character.Alignment.AlignmentStrength.Name} alignment," +
            $"he has {string.Join(", ", character.Weapons.Select(p => p.Name))} as weapons," +
            $"and {string.Join(", ", character.StartItems.Select(p => p.Name))} as start items.";
    }
}
