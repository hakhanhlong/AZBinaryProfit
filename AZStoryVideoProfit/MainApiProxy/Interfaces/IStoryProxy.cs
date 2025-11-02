using AZStoryVideoProfit.MainApiProxy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.Interfaces
{
    public interface IStoryProxy
    {
        StoryIdeaResponseViewModel StoryIdea(StoryIdeaRequestViewModel requestViewModel);

        StorySettingResponseViewModel StorySetting(StorySettingRequestViewModel requestViewModel);

        StoryPremiseResponseViewModel StoryPremise(StoryPremiseRequestViewModel requestViewModel);

        StoryOutlineResponseViewModel StoryOutline(StoryOutlineRequestViewModel requestViewModel);

        StoryStartingResponseViewModel StoryStarting(StoryStartingRequestViewModel requestViewModel);

        StoryContinuationResponseViewModel StoryContinuation(StoryContinuationRequestViewModel requestViewModel);

    }
}
