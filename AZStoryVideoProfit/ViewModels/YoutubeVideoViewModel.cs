using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.ViewModels
{

    public class YoutubeShortStoryVideoViewModel
    {
        public string Name {  get; set; }
        public List<YoutubeShortStoryVideo_Section_ViewModel> Sections { get; set; }
        public string CaptionSuggestionsforAccessibility {  get; set; }
    }


    public class YoutubeShortStoryVideo_Section_ViewModel
    {
        public string Name { get; set; }

        public List<YoutubeShortStoryVideo_Scene_ViewModel> Scenes {  get; set; }

    }
  

    public class YoutubeShortStoryVideo_Scene_ViewModel
    {
        public List<YoutubeShortStoryVideo_VisualInstruction_ViewModel> VisualInstructions {  get; set; }
        public List<YoutubeShortStoryVideo_TextOverlay_ViewModel> TextOverlays { get; set; }
        public List<YoutubeShortStoryVideo_AudioVoiceover_ViewModel> AudioVoiceover { get; set; }

        public string CameraAnglesFramingNotes {  get; set; }
        public string Timing { get; set; }
    }

    public class YoutubeShortStoryVideo_VisualInstruction_ViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }

    public class YoutubeShortStoryVideo_TextOverlay_ViewModel
    {
        public string VisualId { get; set; }
        public string Timing { get; set; }
        public string Description { get; set; }
    }

    public class YoutubeShortStoryVideo_AudioVoiceover_ViewModel
    {
        public string VisualId { get; set; }
        public string Timing { get; set; }
        public string Description { get; set; }
    }


}
