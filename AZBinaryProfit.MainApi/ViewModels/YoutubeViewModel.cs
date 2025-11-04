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
}
