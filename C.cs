namespace Projectile
{
    static class C
    {
        public const uint DEFAULT_WIDTH = 1280;
        public const uint DEFAULT_HEIGHT = 720;
        public const uint MIN_X = 417;
        public const uint MIN_Y = 235;

        public const uint MAIN_TEXT_SIZE = 42;
        public const uint NAME_TEXT_SIZE = 30;
        public const uint TITLE_TEXT_SIZE = 36;
        public const uint RST_TEXT_SIZE = 25;
        public const uint CONST_TEXT_SIZE = 20;
        public const uint FLIGHT_TEXT_SIZE = 25;
        public const uint SHOT_TEXT_SIZE = 25;
        public const uint STAGE_NUM_TEXT_SIZE = 30;
        public const uint RESULT_TEXT_SIZE = 30;
        public const uint RESULT_BTN_TEXT_SIZE = 30;

        public const float DEF_BRD_THICKNESS = 6f;
        public const float PROJ_BRD_THICKNESS = 2f;

        public const uint MARS_BASE_ALT = 145;
        public const uint EARTH_BASE_ALT = 115;
        public const uint MOON_BASE_ALT = 115;
        public const uint KEPLER_BASE_ALT = 115;

        public const uint SLINGSHOT_X_EARTH = 140;
        public const uint SLINGSHOT_X_MARS = 140;
        public const uint SLINGSHOT_X_MOON = 140;
        public const uint SLINGSHOT_X_KEPLER = 140;
        public const uint TARGET_X_EARTH = 1140;
        public const uint TARGET_X_MARS = 1140;
        public const uint TARGET_X_MOON = 1140;
        public const uint TARGET_X_KEPLER = 1140;

        public const float G_EARTH = 9.81f;
        public const float G_MARS = 3.72f;
        public const float G_MOON = 1.62f;
        public const float G_KEPLER = 2.4f * G_EARTH; //PRZY ZAŁOZENIU, ŻE MA PODOBNY SKŁAD DO ZIEMI!

        public const uint SLINGSHOT_PLATFORM_WIDTH = 20;
        public const uint TARGET_PLATFORM_WIDTH = 10;
        public const float STRING_THICKNESS = 4;
        public const float STRING_DISTANCE = 0;

        public const float RESULT_HEIGHT = 240;
        public const float RESULT_WIDTH = 426;
        public const float RESULT_BRD_THICKNESS = 3;
        public const float RESULT_BTN_WIDTH = 54;
        public const float RESULT_BTN_HEIGHT = 30;
        public const float RESULT_BTN_BOTTOM_MARGIN = 15;
        public const float RESULT_BTN_WIDTH_MARGIN = 26;

        public const float THEORY_HEIGHT = 620;
        public const float THEORY_WIDTH = 910;
        public const float THEORY_BRD_THICKNESS = 3;

        public const uint TITLE_TOP_MARGIN = 20;
        public const uint STAGE_TOP_MARGIN = TITLE_TOP_MARGIN + TITLE_TEXT_SIZE + 15;
        public const uint LEFT_MARGIN = 150;
        public const uint TOP_MARGIN = 150;
        public const uint TOP_RESET_MARGIN = 60;
        public const uint TOP_THEORY_BTN_MARGIN = 60;
        public const uint LEFT_THEORY_BTN_MARGIN = DEFAULT_WIDTH-100;
        public const uint RIGHT_RESET_MARGIN = 120;
        public const uint CONST_TEXT_LEFT_MARGIN = 40;
        public const uint CONST_TEXT_TOP_MARGIN = 22;
        public const uint SHOT_TEXT_LEFT_MARGIN = 160;
        public const uint SHOT_TEXT_TOP_MARGIN = 640;
        public const uint FLIGHT_TEXT_LEFT_MARGIN = 820;
        public const uint FLIGHT_TEXT_TOP_MARGIN = 71;
        public const uint V_SPACING = 50;
        public const uint H_SPACING = 146;

        public const uint RST_X_RAD = 75;
        public const uint RST_Y_RAD = 40;
        public const uint THEORY_X_RAD = 75;
        public const uint THEORY_Y_RAD = 40;
        public const uint PROJECTILE_RAD = 5;

        public const float DISTANCE_SCALE = 50; //[px/m]
        public const float ANGLE_CUTOFF = 15;
        public const float ANGLE_SAFE_K = 15;

        public const string SUCCESS_MAIN = "SUCCESS!";
        public const string SUCCESS_LAST_MAIN = "ALL STAGES COMPLETED";
        public const string FAILURE_MAIN = "FAILURE";
        public const string SUCCESS_LEFT = "MENU";
        public const string SUCCESS_RIGHT = "NEXT STAGE";
        public const string FAILURE_LEFT = "MENU";
        public const string FAILURE_RIGHT = "TRY AGAIN";

        public const string DEFAULT_TITLE = "Project: Projectile";
        public const string MIN_EARTH_PATH = "../../media/images/earth_min.png";
        public const string MIN_MARS_PATH = "../../media/images/mars_min.png";
        public const string MIN_KEPLER_PATH = "../../media/images/kepler_22b_min.png";
        public const string MIN_MOON_PATH = "../../media/images/moon_min.png";
        public const string EARTH_PATH = "../../media/images/earth.png";
        public const string MARS_PATH = "../../media/images/mars.png";
        public const string KEPLER_PATH = "../../media/images/kepler_22b.png";
        public const string MOON_PATH = "../../media/images/moon.png";
        public const string MAINFONT_PATH = "../../fonts/BERNHC.ttf";
        public const string FONT_PATH = "../../fonts/consolab.ttf";
        public const string CLICK_SOUND_PATH = "../../media/sounds/click1.wav";
        public const string STRING_SOUND_PATH = "../../media/sounds/rubber_stretch.wav";
        public const string SHOT_SOUND_PATH = "../../media/sounds/shot.wav";
        public const string SLINGSHOT_PATH = "../../media/images/slingshot.png";
        public const string TARGET_PATH = "../../media/images/target.png";

        static C()
        { }
    }
}
