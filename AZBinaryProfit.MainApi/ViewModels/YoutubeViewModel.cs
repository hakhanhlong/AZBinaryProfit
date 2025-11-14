using System.ComponentModel;

namespace AZBinaryProfit.MainApi.ViewModels
{


    public class YoutubeIdeaRequestViewModel
    {
        public int Idea_Number { get; set; }
        public string Topic { get; set; }
    }


    public class YoutubeIdeaResponseViewModel
    {
        public List<YoutubeIdeaStoryItemViewModel> YoutubeIdeas { get; set; } = new();
    }

    public class YoutubeIdeaStoryItemViewModel
    {
        public string Name { get; set; }
        public string Story { get; set; }
        public string Genre { get; set; }
        public string Story_Setting { get; set; }
        public string Character { get; set; }
        public string Plot_Elements { get; set; }
        public string Story_Writing_Style { get; set; }

        public string Story_Tone { get; set; }
        public string Narrative_Pov { get; set; }
        public string Audience_Age_Group { get; set; }
        public string Content_Rating { get; set; }

        public string Ending_Preference { get; set; }
        public string Guidelines { get; set; }

        public List<string> MainPoints { get; set; }
    }


    public class YoutubeShortVideoScriptRequest
    {
        public string story { get; set; }
        //public string target_audience { get; set; }
        public int duration_seconds { get; set; }

        //[Description("Energetic, Professional, Casual, Humorous, Dramatic, Educational, Entertaining, Inspirational")]
        //public string tone_style { get; set; }

        [Description("Question, Statistic, Challenge, Tutorial, Transformation, Trend, Story, Controversy")]
        public string hook_type { get; set; }


        /*
         * "Question": "Start with an intriguing question that stops the scroll",
            "Statistic": "Begin with a surprising statistic or fact",
            "Challenge": "Present a challenge or dare to the viewer",
            "Tutorial": "Jump straight into a quick how-to or life hack",
            "Transformation": "Show a before/after or transformation hook",
            "Trend": "Leverage a current trend or sound",
            "Story": "Start with a captivating micro-story",
            "Controversy": "Present a controversial or surprising statement"
         */
        public string hook_instructions { get; set; }


        [Description("Tutorial/How-to, Life Hack, Entertainment, Educational, Trend, Story, Challenge, Review, News")]
        public string content_type { get; set; }

        // - Include caption suggestions for accessibility
        public bool include_captions { get; set; }

        // - Include text overlay positions and timing
        public bool include_text_overlay { get; set; }

        //- Include sound effect suggestions
        public bool include_sound_effects { get; set; }

        //- Include vertical framing notes for optimal composition
        public bool vertical_framing_notes { get; set; }


    }

    public class YoutubeShortVideoScriptNarrationRequest
    {
        public string shorts_script { get; set; }
        public int duration_seconds { get; set; }

        [Description("Energetic, Professional, Casual, Humorous, Dramatic, Educational, Entertaining, Inspirational")]
        public string tone_style { get; set; }
    }


    public class YoutubeGenerateTitleRequestViewModel
    {

        public string MainPoint {  get; set; }
        public string ToneStyle {  get; set; }
        public string TargetAudience {  get; set; }
        public string UseCase {  get; set; }
        public int NumTitles { get; set; }

    }

    public class YoutubeGenerateTitleResponseViewModel
    {
        public List<string> Title { get; set; }          

    }

    public class YoutubeGenerateDescriptionRequestViewModel
    {
        public string MainPoint { get; set; }
        public string ToneStyle { get; set; }
        public string TargetAudience { get; set; }
        public string UseCase { get; set; }
        //public string Language { get; set; }
        //public string Primary_Keywords {  get; set; }
        //public string Secondary_Keywords { get; set; }
        public string Seo_Goals {  get; set; }

    }


    public class YoutubeGenerateThumbnail_ThumbnailConceptRequestViewModel
    {
        public int NumConcepts { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string TargetAudience { get; set; }
        public string ContentType { get; set; }        
        public string Style_Preference { get; set; }

    }


    public class YoutubeGenerateThumbnail_ThumbnailImagePromptRequestViewModel
    {        
        public string Concept_Description { get; set; }
        public string Style_Preference { get; set; }
        public string Aspect_Ratio { get; set; }
        public string Image_Style { get; set; }
        public string Image_Focus { get; set; }

        public string Text_Style { get; set; }

    }



    public class YoutubeGenerateStoryVideoScriptRequestViewModel
    {

        public string story { get; set; }
        public string target_audience { get; set; }

        [Description("Professional, Casual, Humorous, Educational, Entertaining, Inspirational")]
        public string tone_style { get; set; }

        [Description("How-to/Tutorial, Vlog, Review, Educational, Entertainment, News")]
        public string use_case { get; set; }


        [Description(
            "\"Problem-Solution\": \"Structure the script to first present a problem, then provide a solution.\",\r\n       " +
            "\"Before-After-Bridge\": \"Structure the script to show the before state, the transformation process, and the after state.\",\r\n        " +
            "\"Hook-Problem-Solution-Call to Action\": \"Start with a hook, present the problem, provide the solution, and end with a call to action.\",\r\n       " +
            "\"Compare and Contrast\": \"Structure the script to compare and contrast different options or approaches.\",\r\n      " +
            "\"Step-by-Step Tutorial\": \"Break down the content into clear, sequential steps.\",\r\n      " +
            "\"Case Study\": \"Present a real-world example or case study to illustrate the main points.\",\r\n  " +
            "\"Interview Format\": \"Structure the script as an interview with questions and answers.\",\r\n" +
            "\"Review Format\": \"Structure the script as a review with pros, cons, and a final verdict.\",\r\n " +
            "\"Vlog Format\": \"Structure the script as a personal video blog with a conversational tone.\",\r\n " +
            "\"Educational Format\": \"Structure the script to teach a concept with examples and explanations.\",\r\n " +
            "\"Entertainment Format\": \"Structure the script to entertain while delivering the main message.\""
            )]
        public string script_structure { get; set; }
        public string script_structure_desc { get; set; }

        [Description(
            "Include a hook at the beginning to grab attention." +
            "End with a strong call to action." +
            "Include prompts for viewer engagement (e.g., questions, polls)." +
            "Include suggested timestamps for key sections." +
            "Include visual cues and transitions.")]
        public string additional_elements { get; set; }


        /*
         * Question Hook:Start with a thought-provoking question to engage viewers immediately
         * Story Hook: Begin with a brief, relevant story or anecdote
         * Statistic Hook: Open with an interesting statistic or fact
         * Controversy Hook: Present a controversial statement or opinion to spark interest
         * Promise Hook: Make a promise about what viewers will learn or gain
         * Scenario Hook:Describe a scenario or situation viewers can relate to
         * Mystery Hook: Create a sense of mystery or intrigue
         * Quote Hook:Start with a relevant quote from an expert or notable figure
         */
        public string hooks { get; set; }

        /*
         * Comment Prompt:Ask viewers to share their experiences or opinions in the comments
         * Poll Suggestion: Suggest creating a poll for viewers to vote on
         * Question for Comments: Pose a specific question for viewers to answer in the comments
         * Challenge: Challenge viewers to try something and report back
         * Tag Friends: Encourage viewers to tag friends who might benefit from the content
         * Share Request: Ask viewers to share the video with others who might find it helpful
         * Community Post: Mention creating a community post to continue the discussion
         * Live Stream Teaser: Tease an upcoming live stream on the same topic
         */
        public string community_interactions { get; set; }


        [Description("Include a hook at the beginning to grab attention.")]
        public bool include_hook { get; set; }
        [Description("End with a strong call to action.")]
        public bool include_cta { get; set; }
        [Description("Include prompts for viewer engagement (e.g., questions, polls)")]
        public bool include_engagement { get; set; }
        [Description("Include suggested timestamps for key sections.")]
        public bool include_timestamps { get; set; }
        [Description("Include visual cues and transitions.")]
        public bool include_visual_cues { get; set; }

        public string language {  get; set; }
        public int duration_seconds {  get; set; }

    }



}
