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
    public class AudioController : ControllerBase
    {
        private readonly ILogger<AudioController> _logger;
        private readonly Kernel _Kernel;
        private AppSettings appSettings;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public AudioController(
            ILogger<AudioController> logger,
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
        [Route("AudioScript")]
        public async Task<IActionResult> AudioScript([FromBody] AudioScriptRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Audio", "AudioScript", "skprompt.txt");


            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);
            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.7,
                TopK = 1,
                TopP = 0.9

            })
            {
                ["title"] = request.Title,
                ["story"] = request.Story,
                ["narrator_persona"] = request.NarratorPersona,
                ["content_instruction"] = request.ContentInstruction,
                ["style"] = request.Style,
                ["tts_instruction"] = request.TTSInstruction,
                ["language"] = request.Language,
                ["script_length"] = request.ScriptLength,
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();


            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = responseData,
                Info = new
                {
                    TotalTokenCount = metadata!["TotalTokenCount"],
                    PromptTokenCount = metadata!["PromptTokenCount"],
                    CandidatesTokenCount = metadata!["CandidatesTokenCount"],
                    CurrentCandidateTokenCount = metadata!["CurrentCandidateTokenCount"]
                }
            });

        }

        [HttpPost]
        [Route("SEOMetadata")]
        public async Task<IActionResult> SEOMetadata([FromBody] AudioScript_SEOMetadataRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Audio", "SEOMetadata", "skprompt.txt");


            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);
            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.7,
                TopK = 1,
                TopP = 0.9

            })
            {
                ["topic"] = request.Topic,
                ["script"] = request.Script
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();


            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = responseData,
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
