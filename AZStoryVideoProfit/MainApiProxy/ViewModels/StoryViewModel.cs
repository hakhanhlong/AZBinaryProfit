using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.ViewModels
{
    public class StoryIdeaRequestViewModel
    {
        public int IdeaNumber { get; set; }
        public string Topic { get; set; }
        public string Language {  get; set; }
    }



    public class StoryIdeaResponseViewModel: BaseViewModel
    {
        public StoryIdeaViewModel Data { get; set; }
      
    }

    public class StoryIdeaViewModel
    {
        public List<StoryIdeaItemViewModel> IdeaStories { get; set; }
    }



    public class StoryIdeaItemViewModel
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
}
