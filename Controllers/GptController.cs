using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;
using OpenAI_API.Models;

namespace Finals.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GptController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetAIBasedResult(string SearchText)
    {
        string APIKey = "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";
        string answer = string.Empty;

        var api = new OpenAIAPI(APIKey);

        var chat = api.Chat.CreateConversation();
        chat.Model = Model.GPT4_Turbo;
        chat.RequestParameters.Temperature = 0.5f;
        chat.RequestParameters.MaxTokens = 60;

        chat.AppendSystemMessage(
            "Provide only the high-end price for the car. Without currency sign. Just a single number.");

        chat.AppendUserInput("BMW X1 SUV?");
        chat.AppendExampleChatbotOutput("34000");

        chat.AppendUserInput(SearchText);
        
        string response = await chat.GetResponseFromChatbotAsync();

        return Ok(response);
    }
}