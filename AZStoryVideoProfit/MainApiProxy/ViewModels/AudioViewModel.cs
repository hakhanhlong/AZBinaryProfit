using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.ViewModels
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

    public class AudioScriptResponseViewModel: BaseViewModel
    {
        public string Data { get; set; }
    }
}
