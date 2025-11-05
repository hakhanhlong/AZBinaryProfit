namespace AZBinaryProfit.MainApi.ViewModels
{
    public class AudioScriptRequestViewModel
    {

        
        public string Title { get; set; }
        public string Story { get; set; }
        public string NarratorPersona { get; set; }        
        public string ContentInstruction { get; set; }
        public string Style { get; set; }
        public string TTSInstruction { get; set; }
        public string Language { get; set; }
        public int ScriptLength { get; set; }

    }
}
