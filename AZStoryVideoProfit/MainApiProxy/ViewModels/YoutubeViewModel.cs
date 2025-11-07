using System.Collections.Generic;
using System.ComponentModel;

namespace AZStoryVideoProfit.MainApiProxy.ViewModels
{

    public class YoutubeIdeaRequestViewModel
    {
        public int Idea_Number { get; set; }
        public string Topic { get; set; }
    }


    public class YoutubeIdeaResponseViewModel : BaseViewModel
    {
        public YoutubeIdeaResponseModel Data { get; set; }
    }

    public class YoutubeIdeaResponseModel
    {
        public List<YoutubeIdeaStoryItemViewModel> YoutubeIdeas { get; set; }
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

    public class YoutubeShortVideoScriptResponse: BaseViewModel
    {
        public string Data { get; set; }
    }

    public class YoutubeGenerateTitleRequestViewModel
    {

        public string MainPoint {  get; set; }
        public string ToneStyle {  get; set; }
        public string TargetAudience {  get; set; }
        public string UseCase {  get; set; }
        public int NumTitles { get; set; }

    }

    public class YoutubeGenerateTitleResponseViewModel: BaseViewModel
    {

        public YoutubeGenerateTitleViewModel Data { get; set; }       

    }

    public class YoutubeGenerateTitleViewModel
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
        //public string Primary_Keywords { get; set; }
        //public string Secondary_Keywords { get; set; }
        public string Seo_Goals { get; set; }

    }

    public class YoutubeGenerateDescriptionResponseViewModel : BaseViewModel
    {
        public string Data { get; set; }
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

    public class YoutubeGenerateThumbnail_ThumbnailConceptResponseViewModel : BaseViewModel
    {
        public string Data { get; set; }
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

    public class YoutubeGenerateThumbnail_ThumbnailImagePromptResponseViewModel
    {
        public string Data { get; set; }
    }
}
