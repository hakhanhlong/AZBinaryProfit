namespace AZBinaryProfit.MainApi.ViewModels
{
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



}
