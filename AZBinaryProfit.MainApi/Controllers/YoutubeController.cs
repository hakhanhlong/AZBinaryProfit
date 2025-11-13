using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Newtonsoft.Json;
using System.Text.Json;
using AZBinaryProfit.MainApi.ViewModels;
using System.Linq;

namespace AZBinaryProfit.MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubeController : ControllerBase
    {
        private readonly ILogger<YoutubeController> _logger;
        private readonly Kernel _Kernel;
        private AppSettings appSettings;
        //private readonly ITextGenerationService _ITextGenerationService;
        private readonly IChatCompletionService _IChatCompletionService;
        public YoutubeController(
            ILogger<YoutubeController> logger,
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
        [Route("YoutubeIdea")]
        public async Task<IActionResult> YoutubeIdea([FromBody] YoutubeIdeaRequestViewModel request)
        {


            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Youtube", "GenerateIdea", "skprompt.txt");
            


            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

  


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);
            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ResponseSchema = typeof(YoutubeIdeaResponseViewModel),
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
                ["idea_number"] = request.Idea_Number,
                ["topic"] = request.Topic
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();


            var metadata = response.Metadata;

            return new JsonResult(new
            {
                Data = JsonConvert.DeserializeObject<YoutubeIdeaResponseViewModel>(responseData),
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
        [Route("YoutubeShortVideoScript")]
        public async Task<IActionResult> YoutubeShortVideoScript([FromBody] YoutubeShortVideoScriptRequest request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts","Youtube", "GenerateShortVideoScript", "skprompt.txt");
            

            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);



            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);



            string required_elements = "";
            if (request.include_captions)
                required_elements += "- Include caption suggestions for accessibility.\n\n";
            if (request.include_text_overlay)
                required_elements += "- Include text overlay positions and timing.\n\n";
            if (request.include_sound_effects)
                required_elements += "- Include sound effect suggestions.\n\n";
            if (request.vertical_framing_notes)
                required_elements += "- Include vertical framing notes for optimal composition.\n\n";



            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 8192,
                Temperature = 0.7,
                TopP = 0.9,
                TopK = 1
            })
            {
                ["story"] = request.story,                               
                ["duration_seconds"] = request.duration_seconds,
                ["hook_type"] = request.hook_type,
                ["hook_instructions"] = request.hook_instructions,
                ["required_elements"] = required_elements
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();



            return new JsonResult(
            new
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
        [Route("YoutubeShortVideoScriptNarration")]
        public async Task<IActionResult> YoutubeShortVideoScriptNarration([FromBody] YoutubeShortVideoScriptNarrationRequest request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Youtube", "GenerateShortVideoScriptNarration", "skprompt.txt");
            

            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

            


            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);

            float Words_Per_Second = 3.5f;
            float Narration_Padding = 0.5f;

            int Target_Words = (int)((Convert.ToInt32(request.duration_seconds) - Narration_Padding) * Words_Per_Second);


            string[] scenes = request.shorts_script.Split("\n\n");



            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ThinkingConfig = new GeminiThinkingConfig
                {
                    ThinkingBudget = 0
                },
                MaxTokens = 4096,
                Temperature = 0.7,
                TopP = 0.9,
                TopK = 1
            })
            {
                ["shorts_script"] = request.shorts_script,
                ["duration_seconds"] = request.shorts_script,
                ["tone_style"] = request.tone_style,
                ["target_words"] = Target_Words,
                ["words_per_second"] = Words_Per_Second
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);

            var metadata = response.Metadata;
            //var tokenUsage = metadata!["Usage"] as ChatTokenUsage;

            var responseData = response.GetValue<string>();



            return new JsonResult(
            new
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
        [Route("GenerateTitle")]
        public async Task<IActionResult> GenerateTitle([FromBody] YoutubeGenerateTitleRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Youtube", "GenerateTitle", "skprompt.txt");
            
            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

            
            // Create the function with both prompt and config
            //var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent, config.ExecutionSettings["default"]);
            var promptFunctionFromPrompt = _Kernel.CreateFunctionFromPrompt(promptContent);
            var kernelArguments = new KernelArguments(new GeminiPromptExecutionSettings
            {
                ResponseSchema = typeof(YoutubeGenerateTitleResponseViewModel),
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
                ["main_points"] = request.MainPoint,
                ["num_titles"] = request.NumTitles,
                ["target_audience"] = request.TargetAudience,
                ["tone_style"] = request.ToneStyle,
                ["use_case"] = request.UseCase,                
            };

            // Querying the prompt function
            var response = await promptFunctionFromPrompt.InvokeAsync(_Kernel, kernelArguments);
            var responseData = response.GetValue<string>();


            var metadata = response.Metadata;

            var titles = JsonConvert.DeserializeObject<YoutubeGenerateTitleResponseViewModel>(responseData);
            var listAnalyze = new List<object>();
            foreach ( var title in titles.Title)
            {
                listAnalyze.Add(new
                {
                    Title = title,
                    Analyze = AnalyzeTitle(title)
                });
            }

            return new JsonResult(new
            {
                Data = titles,
                Info = new
                {
                    TotalTokenCount = metadata!["TotalTokenCount"],
                    PromptTokenCount = metadata!["PromptTokenCount"],
                    CandidatesTokenCount = metadata!["CandidatesTokenCount"],
                    CurrentCandidateTokenCount = metadata!["CurrentCandidateTokenCount"],
                    Analyze = listAnalyze
                }
            });

        }

        private object AnalyzeTitle(string Title)
        {
            int char_count = Title.Length;
            bool optimal_length = char_count >= 50 && char_count <= 60;


            string[] clickbait_phrases = [
                "shocking", "you won't believe", "gone wrong", "gone sexual",
                "free v-bucks", "free robux", "100%", "gone viral", "viral",
                "you need to see this", "wait till the end", "at 3am", "3am",
                "don't watch this", "watch till the end", "gone too far",
                "insane", "unbelievable", "mind-blowing", "life-changing",
                "secret", "hidden", "revealed", "exposed", "leaked",
                "never before seen", "first time ever", "world's first",
                "no one knows", "experts hate this", "doctors hate this",
                "this will change your life", "this will blow your mind",
                "you've been doing it wrong", "the truth about", "the real reason",
                "what they don't want you to know", "what they're hiding",
                "what they don't tell you", "what you need to know",
                "what you should know", "what you must know", "what you must see",
                "what you must watch", "what you must do", "what you must have",
                "what you must buy", "what you must try", "what you must avoid",
                "what you must stop doing", "what you must start doing",
                "what you must change", "what you must learn", "what you must understand",
                "what you must realize", "what you must accept", "what you must believe",
                "what you must know about", "what you must see about", "what you must watch about",
                "what you must do about", "what you must have about", "what you must buy about",
                "what you must try about", "what you must avoid about", "what you must stop doing about",
                "what you must start doing about", "what you must change about", "what you must learn about",
                "what you must understand about", "what you must realize about", "what you must accept about",
                "what you must believe about", "what you must know about", "what you must see about",
                "what you must watch about", "what you must do about", "what you must have about",
                "what you must buy about", "what you must try about", "what you must avoid about",
                "what you must stop doing about", "what you must start doing about", "what you must change about",
                "what you must learn about", "what you must understand about", "what you must realize about",
                "what you must accept about", "what you must believe about"
            ];


            int clickbait_score = 0;
            List<string> detected_phrases = new List<string>();
            foreach(var phrase in clickbait_phrases)
            {
                if (phrase.ToLower().Contains(Title.ToLower()))
                {
                    clickbait_score += 1;
                    detected_phrases.Add(phrase);
                }
           
            }

            bool is_clickbait = clickbait_score > 0;

            bool hasNumber = Title.Any(char.IsDigit);
            bool hasQuestion = Title.Contains("?");
            bool hasColon = Title.Contains(":");
            bool hasBrackets = Title.Contains("[") || Title.Contains("]") || Title.Contains("(") || Title.Contains(")");

            // Initialize the score (Equivalent to: seo_score = 0)
            int seoScore = 0;

            // All subsequent checks use the same structure, adding points if the condition is true.

            // Equivalent to: if optimal_length: seo_score += 3
            if (optimal_length)
            {
                seoScore += 3;
            }

            // Equivalent to: if has_number: seo_score += 1
            if (hasNumber)
            {
                seoScore += 1;
            }

            // Equivalent to: if has_question: seo_score += 1
            if (hasQuestion)
            {
                seoScore += 1;
            }

            // Equivalent to: if has_colon: seo_score += 1
            if (hasColon)
            {
                seoScore += 1;
            }

            // Equivalent to: if has_brackets: seo_score += 1
            if (hasBrackets)
            {
                seoScore += 1;
            }

            // Equivalent to: if not is_clickbait: seo_score += 2
            if (!is_clickbait)
            {
                seoScore += 2;
            }


            return new
            {
                char_count = char_count,
                optimal_length = optimal_length,
                is_clickbait = is_clickbait,
                clickbait_score = clickbait_score,
                seo_score = seoScore,
                has_number = hasNumber,
                has_question = hasQuestion,
                has_colon = hasColon,
                has_brackets = hasBrackets,
            };
        }



        [HttpPost]
        [Route("GenerateDescription")]
        public async Task<IActionResult> GenerateDescription([FromBody] YoutubeGenerateDescriptionRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Youtube", "GenerateDescription", "skprompt.txt");

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
                ["main_points"] = request.MainPoint,                
                ["target_audience"] = request.TargetAudience,
                ["tone_style"] = request.ToneStyle,
                ["use_case"] = request.UseCase,
                ["primary_keywords"] = request.UseCase,
                ["secondary_keywords"] = request.UseCase,
                ["seo_goals"] = request.UseCase,
                ["timecode_status"] = "YES"
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
        [Route("GenerateThumbnail_ThumbnailConcepts")]
        public async Task<IActionResult> GenerateThumbnail_ThumbnailConcepts([FromBody] YoutubeGenerateThumbnail_ThumbnailConceptRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Youtube", "GenerateThumbnail", "ThumbnailConcepts", "skprompt.txt");

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
                ["num_concepts"] = request.NumConcepts,
                ["video_title"] = request.VideoTitle,
                ["video_description"] = request.VideoDescription,
                ["target_audience"] = request.TargetAudience,
                ["content_type"] = request.ContentType,
                ["style_preference"] = request.Style_Preference,
                ["host_element"] = "YES"
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
        [Route("GenerateThumbnail_ThumbnailImagePrompt")]
        public async Task<IActionResult> GenerateThumbnail_ThumbnailImagePrompt([FromBody] YoutubeGenerateThumbnail_ThumbnailImagePromptRequestViewModel request)
        {

            var promptFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Youtube", "GenerateThumbnail", "ThumbnailDesign", "skprompt.txt");

            // Read prompt content
            var promptContent = System.IO.File.ReadAllText(promptFilePath);

            promptContent = promptContent.Replace("{{$concept_description}}", request.Concept_Description);
            promptContent = promptContent.Replace("{{$style_preference}}", request.Style_Preference);
            promptContent = promptContent.Replace("{{$aspect_ratio}}", request.Aspect_Ratio);
            promptContent = promptContent.Replace("{{$image_style}}", request.Image_Style);
            promptContent = promptContent.Replace("{{$image_focus}}", request.Image_Focus);
            promptContent = promptContent.Replace("{{$text_style}}", request.Text_Style);
            



            return new JsonResult(new
            {
                Data = promptContent,
                Info = new
                {
                    TotalTokenCount = 0,
                    PromptTokenCount = 0,
                    CandidatesTokenCount = 0,
                    CurrentCandidateTokenCount = 0
                }
            });

        }



    }
}
