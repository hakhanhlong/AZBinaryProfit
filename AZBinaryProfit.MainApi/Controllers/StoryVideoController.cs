using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;
using AZBinaryProfit.MainApi.ViewModels;

namespace AZBinaryProfit.MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryVideoController : ControllerBase
    {
        private readonly ILogger<StoryVideoController> _logger;
        private readonly Kernel _Kernel;
        private AppSettings appSettings;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public StoryVideoController(
            ILogger<StoryVideoController> logger,
            Kernel kernel,
            IOptions<AppSettings> options)
        {
            _logger = logger;
            _Kernel = kernel;
            //_ITextGenerationService = kernel.GetRequiredService<ITextGenerationService>();
            _IChatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

            appSettings = options.Value;

        }

        [HttpPost]
        [Route("StoryVideoSceneGenerate")]
        public async Task<IActionResult> StoryVideoSceneGenerate([FromBody] StoryVideoSceneRequestViewModel request)
        {
            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "StoryVideo", "StoryVideoScene", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "StoryVideo", "StoryVideoScene", "config.json");

            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

            // Read and parse config.json
            var configJson = System.IO.File.ReadAllText(configFilePath);
            var configOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var config = PromptTemplateConfig.FromJson(configJson);


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);

            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ResponseSchema = typeof(StoryVideoSceneResponseViewModel),
                ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 32000,
                Temperature = 0.0,
                //Temperature = 0.7,
                //TopP = 0.9,
                //TopK = 1
            })
            {
                ["genre"] = request.Genre,
                ["num_scenes"] = request.Num_Scenes,
                ["story"] = request.Story,
                ["scene_per_second"] = request.Scene_Per_Second
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();



            return new JsonResult(
            new
            {
                Data = JsonConvert.DeserializeObject<StoryVideoSceneResponseViewModel>(responseData),
                Info = new
                {
                    TotalTokenCount = metadata!["TotalTokenCount"],
                    PromptTokenCount = metadata!["PromptTokenCount"],
                    CandidatesTokenCount = metadata!["CandidatesTokenCount"],
                    CurrentCandidateTokenCount = metadata!["CurrentCandidateTokenCount"]
                }
            });


        }
    }
}
