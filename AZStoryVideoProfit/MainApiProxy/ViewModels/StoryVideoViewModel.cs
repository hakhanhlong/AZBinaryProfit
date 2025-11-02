using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AZStoryVideoProfit.MainApiProxy.ViewModels
{
    public class StoryVideoSceneRequestViewModel
    {

        public int Num_Scenes { get; set; }


        /*
         *  "children's story", câu chuyện thiếu nhi
            "adventure story", câu chuyện phiêu lưu
            "fairy tale", truyện cổ tích
            "sci-fi story", truyện khoa học viễn tưởng
            "fantasy story", câu chuyện tưởng tượng
            "mystery story", câu chuyện bí ẩn
            "fable", truyện ngụ ngôn
         */
        public string Genre { get; set; }

        /*
         * e.g., Một câu chuyện thiếu nhi vui nhộn về một chú robot vụng về cố gắng nướng bánh mừng sinh nhật người sáng tạo ra nó trong một căn bếp tương lai
         */
        public string Story { get; set; }

        public int Scene_Per_Second { get; set; }

    }


    public class StoryVideoResponseViewModel : BaseViewModel
    {        
        public StoryVideoSceneResponseViewModel Data { get; set; }

    }

    public class StoryVideoSceneResponseViewModel
    {

        public string Name { get; set; }
        public List<StoryVideoSceneItem> Scenes { get; set; }

    }

    public class StoryVideoSceneItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Narration { get; set; }
    }

}
