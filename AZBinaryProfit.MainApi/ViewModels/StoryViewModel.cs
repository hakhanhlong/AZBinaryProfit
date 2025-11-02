namespace AZBinaryProfit.MainApi.ViewModels
{




    public class StoryIdeaRequestViewModel
    {
        public int IdeaNumber { get; set; }
        public string Topic { get; set; }
        public string Language { get; set; }
    }


    public class StoryIdeaResponseViewModel
    {
        public List<IdeaStoryItemViewModel> IdeaStories { get; set; } = new();
    }

    public class IdeaStoryItemViewModel
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
    }


    public class StorySettingRequestViewModel
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

        public int NumberWords {  get; set; }
        public int NumberPages { get; set; }

        public string Persona {  get; set; }
        public string Language {  get; set; }
    }

    public class StorySettingResponseViewModel
    {
        public string Settings { get; set; }
        public string Guidelines { get; set; }
    }





    public class StoryPremiseRequestViewModel
    {
        public string Persona { get; set; }
        public string StorySetting { get; set; }
        public string Character { get; set; }
    }


    public class StoryOutlineRequestViewModel
    {
        public string Persona { get; set; }
        public string Premise { get; set; }        
    }

    public class StoryStartingRequestViewModel
    {
        public string Persona { get; set; }
        public string Premise { get; set; }

        public string Outline { get; set; }

        public string Guidelines { get; set; }
    }


    public class StoryContinuationRequestViewModel
    {
        public string Persona { get; set; }
        public string Premise { get; set; }

        public string Outline { get; set; }

        public string Guidelines { get; set; }

        public string StoryStarting { get; set; }        

        public int Number_Pages { get; set; }
        public int Number_Words { get; set; }
    }

}
