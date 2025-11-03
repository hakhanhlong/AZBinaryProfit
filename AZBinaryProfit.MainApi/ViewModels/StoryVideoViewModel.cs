namespace AZBinaryProfit.MainApi.ViewModels
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

        public string VideoStyle {  get; set; }

        public string Video_AspectRatio {  get; set; }

    }


    public class StoryVideoSceneResponseViewModel
    {

        public string Name { get; set; }
        public List<StoryVideoSceneCharacter> Characters { get; set; }
        public List<StoryVideoSceneItem> Scenes { get; set; }

    }

    public class StoryVideoSceneItem
    {
        //public string Type {  get; set; }

        //public string Camera_Position { get; set; }
        //public string Camera_Motion_Primary { get; set; }
        //public string Motion_path { get; set; }
        //public string Tilt { get; set; }
        //public string Stabilization { get; set; }

        //public string Depth_Of_Field { get; set; }

        public string Description { get; set; }
        public string Narration { get; set; }
        public List<string> CharacterId {  get; set; }

        public StoryVideoScene_Background Background {  get; set; }


        //public StoryVideoScene_Scene Scene {  get; set; }
        //public StoryVideoSceneColor_Palette Color_Palette {  get; set; }
        public StoryVideoSceneVisual_Rules Visual_Style { get; set; }

        public List<string> Nagative_Prompt { get; set; }

        //public StoryVideoSceneCinematography Cinematography {  get; set; }

        //public StoryVideoScene_Background Background { get; set; }


    }

    public class StoryVideoSceneCharacter
    {
        public string CharacterId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Role { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }

    }
    public class StoryVideoSceneVisual_Rules
    {
        public string Style { get; set; }
        public List<string> Bans { get; set; }
        public List<string> Continuity { get; set; }       

    }

    public class StoryVideoSceneColor_Palette
    {
        public string Grade { get; set; }
        public List<string> Primary { get; set; }
        public List<string> Accents { get; set; }

    }

    public class StoryVideoSceneCinematography
    {
        public string Camera { get; set; }
        public string Lens { get; set; }
        public string Shutter { get; set; }

        public string iso { get; set; }
        public string White_Balance { get; set; }

        public string Exposure_notes { get; set; }

        public string Composition { get; set; }

    }


    public class StoryVideoScene_Scene
    {
        public string Time_Of_Day { get; set; }
        public string Lighting { get; set; }
        public string Atmosphere { get; set; }
       
    }

    public class StoryVideoScene_Background
    {
        //public string Id { get; set; }
        //public string Name { get; set; }
        public string Settings { get; set; }

        //public string Scenery { get; set; }
        //public string Props { get; set; }
        //public string Lighting { get; set; }

    }



}
