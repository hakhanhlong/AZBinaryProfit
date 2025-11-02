using AZStoryVideoProfit.MainApiProxy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.Interfaces
{
    public interface IStoryVideoProxy
    {
        StoryVideoResponseViewModel StoryVideoSceneGenerate(StoryVideoSceneRequestViewModel requestViewModel);
      
    }
}
