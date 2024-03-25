using Finals.Interfaces;
using OpenAI_API;
using OpenAI_API.Models;

namespace Finals.Services;

public class GptService : IGptService
{
    private readonly string _apiKey;

    public GptService(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>("OPENAI_API_KEY"); 
    }

    public async Task<decimal> GetCarPriceAsync(string model)
    {
        var api = new OpenAIAPI(_apiKey);

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.GPT4_Turbo;
        chat.RequestParameters.Temperature = 0.5f;
        chat.RequestParameters.MaxTokens = 60;

        chat.AppendSystemMessage(
            "Provide only the high-end price for the car. Without currency sign. Just a single number.");

        chat.AppendUserInput(model);

        string response = await chat.GetResponseFromChatbotAsync();

        return decimal.Parse(response);
    }
}