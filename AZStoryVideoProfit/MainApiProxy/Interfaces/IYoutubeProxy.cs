using AZStoryVideoProfit.MainApiProxy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.Interfaces
{
    public interface IYoutubeProxy
    {
        YoutubeGenerateTitleResponseViewModel GenerateTitle(YoutubeGenerateTitleRequestViewModel requestViewModel);

        YoutubeGenerateDescriptionResponseViewModel GenerateDescription(YoutubeGenerateDescriptionRequestViewModel requestViewModel);

        YoutubeGenerateThumbnail_ThumbnailConceptResponseViewModel GenerateThumbnailConcept(YoutubeGenerateThumbnail_ThumbnailConceptRequestViewModel requestViewModel);

        YoutubeGenerateThumbnail_ThumbnailImagePromptResponseViewModel GenerateThumbnail_ImagePrompt(YoutubeGenerateThumbnail_ThumbnailImagePromptRequestViewModel requestViewModel);

    }
}
