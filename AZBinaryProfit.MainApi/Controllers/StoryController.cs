using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using AZBinaryProfit.MainApi.ViewModels;
using Microsoft.SemanticKernel.Connectors.Google;
using System.Text.Json;
using Newtonsoft.Json;
using AZBinaryProfit.MainApi.Helpers;

namespace AZBinaryProfit.MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly ILogger<StoryController> _logger;
        private readonly Kernel _Kernel;
        private AppSettings appSettings;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public StoryController(
            ILogger<StoryController> logger,
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
        [Route("StoryIdea")]
        public async Task<IActionResult> StoryIdea([FromBody] StoryIdeaRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryIdea", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryIdea", "config.json");

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
                ResponseSchema = typeof(StoryIdeaResponseViewModel),
                ResponseMimeType = "application/json",
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
                ["idea_number"] = request.IdeaNumber,
                ["topic"] = request.Topic,
                ["language"] = request.Language
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();


            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = JsonConvert.DeserializeObject<StoryIdeaResponseViewModel>(responseData),
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
        [Route("StorySetting")]
        public async Task<IActionResult> StorySetting([FromBody] StorySettingRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StorySetting", "skprompt.txt");
            var settings = System.IO.File.ReadAllText(promptFilePath);
            settings = settings.Replace("{{$story}}", request.Story);
            settings = settings.Replace("{{$language}}", request.Language);
            settings = settings.Replace("{{$persona}}", request.Persona);
            settings = settings.Replace("{{$story_setting}}", request.Story_Setting);
            settings = settings.Replace("{{$character_input}}", request.Character);
            settings = settings.Replace("{{$plot_element}}", request.Plot_Elements);
            settings = settings.Replace("{{$writing_style}}", request.Story_Writing_Style);
            settings = settings.Replace("{{$story_tone}}", request.Story_Tone);
            settings = settings.Replace("{{$narrative_pov}}", request.Narrative_Pov);
            settings = settings.Replace("{{$audience_age_group}}", request.Audience_Age_Group);
            settings = settings.Replace("{{$content_rating}}", request.Content_Rating);
            settings = settings.Replace("{{$ending_preference}}", request.Ending_Preference);



            promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryGuideline", "skprompt.txt");
            string guidelines = System.IO.File.ReadAllText(promptFilePath);
            guidelines = guidelines.Replace("{{$number_words}}", $"{request.NumberWords * request.NumberPages}");
            guidelines = guidelines.Replace("{{$pages}}", $"{request.NumberPages}");


            return new JsonResult(new StorySettingResponseViewModel
            {
                Settings = settings,
                Guidelines = guidelines,
            });

        }


        [HttpPost]
        [Route("StoryPremise")]
        public async Task<IActionResult> StoryPremise([FromBody] StoryPremiseRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryPremise", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryPremise", "config.json");

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
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4000,
                Temperature = 1,
                TopK = 0,
                TopP = 0.7

            })
            {
                ["persona"] = request.Persona,
                ["story_setting"] = request.StorySetting,
                ["character_input"] = request.Character,
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responsePremise = response.GetValue<string>();

            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = responsePremise,
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
        [Route("StoryOutline")]
        public async Task<IActionResult> StoryOutline([FromBody] StoryOutlineRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryOutline", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryOutline", "config.json");

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
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4000,
                Temperature = 1,
                TopK = 0,
                TopP = 0.7

            })
            {
                ["persona"] = request.Persona,
                ["premise"] = request.Premise
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responsePremise = response.GetValue<string>();

            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = responsePremise,
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
        [Route("StoryStarting")]
        public async Task<IActionResult> StoryStarting([FromBody] StoryStartingRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryStarting", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryStarting", "config.json");

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
                //ResponseSchema = typeof(StoryGenerateResponseViewModel),
                //ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 1,
                TopK = 0,
                TopP = 0.7

            })
            {
                ["persona"] = request.Persona,
                ["premise"] = request.Premise,
                ["outline"] = request.Outline,
                ["guidelines"] = request.Guidelines,
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responsePremise = response.GetValue<string>();

            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = responsePremise,
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
        [Route("StoryContinuation")]
        public async Task<IActionResult> StoryContinuation([FromBody] StoryContinuationRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryContinuation", "skprompt.txt");
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Story", "StoryContinuation", "config.json");

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
                //ResponseSchema = typeof(StoryGenerateResponseViewModel),
                //ResponseMimeType = "application/json",
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 1,
                TopK = 0,
                TopP = 0.7

            })
            {
                ["persona"] = request.Persona,
                ["premise"] = request.Premise,
                ["outline"] = request.Outline,
                ["guidelines"] = request.Guidelines,
                ["story"] = request.StoryStarting,
                ["chunkedtext"] = ""
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseContinuation = response.GetValue<string>();




            int count = 1;
            string story_text = request.StoryStarting + $"\n\n --count {count}-- \n\n" + responseContinuation;

            
            int target_words =  request.Number_Pages * request.Number_Words; //pages * words
            while (!responseContinuation.Contains("IAMDONE") && StringHelper.CountWords(story_text) < target_words)
            {

                int remaining_words = target_words - StringHelper.CountWords(story_text);
                int chunk_words = Math.Min(request.Number_Words, remaining_words);

                promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);
                kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
                {
                    //ResponseSchema = typeof(StoryGenerateResponseViewModel),
                    //ResponseMimeType = "application/json",
                    ThinkingConfig = new GeminiThinkingConfig
                    {
                        ThinkingBudget = 0
                    },
                    MaxTokens = 8192,
                    Temperature = 1,
                    TopK = 0,
                    TopP = 0.7

                })
                {
                    ["persona"] = request.Persona,
                    ["premise"] = request.Premise,
                    ["outline"] = request.Outline,
                    ["story"] = story_text,
                    ["guidelines"] = request.Guidelines,
                    ["chunkedtext"] = ""
                };


                // Querying the prompt function
                response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
                responseContinuation = response.GetValue<string>();
                

                System.Diagnostics.Debug.WriteLine($"Run number:{count}");
                count++;

                story_text += $"\n\n --count {count}-- \n\n" + responseContinuation;
            }



            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = story_text,
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
