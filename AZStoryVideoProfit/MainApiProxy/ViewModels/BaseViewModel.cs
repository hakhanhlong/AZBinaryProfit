using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.ViewModels
{
    public class BaseViewModel
    {
        public InfoViewModel Info { get; set; }
    }

    public class InfoViewModel
    {
        public int TotalTokenCount { get; set; }
        public int PromptTokenCount { get; set; }
        public int CandidatesTokenCount { get; set; }
        public int CurrentCandidateTokenCount { get; set; }

        public object Analyze {  get; set; }
    }
}
