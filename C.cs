namespace Projectile
{
    static class C
    {   // klasa przechowująca stałe używane w programie
        // umieszczone tu dla wygodnego manipulowania danymi
        // bez przeszukiwania projektu

        // STAŁE DOTYCZĄCE CAŁEGO PROGRAMU
        public const int FPS = 60;
        public const float REFRESH_RATE = 1f / FPS;
        public const int DEFAULT_WIDTH = 1280;
        public const int DEFAULT_HEIGHT = 720;
        public const string AUTHOR_INFO = "                                        Piotr Chodań 180434, JPWP 2021\n" +
                                          "Temat projektu: Gra symulująca rzut paraboliczny \"Project: Projectile\"";

        // ścieżki dostępu
        public const string SAVE_PATH = "../../save/progress.bin";
        public const string MAINFONT_PATH = "../../fonts/BERNHC.ttf";
        public const string INFO_FONT_PATH = "../../fonts/consolab.ttf";
        public const string THEORY_FONT_PATH = "../../fonts/cmunrm.ttf";
        public const string CLICK_SOUND_PATH = "../../media/sounds/click1.wav";

        // STAŁE DOTYCZĄCE MENU GŁÓWNEGO
        // wymiary
        public const int MIN_X = 417;
        public const int MIN_Y = 235;

        // rozmiary czcionki
        public const int MAIN_TEXT_SIZE = 42;
        public const int NAME_TEXT_SIZE = 30;
        public const int CPLT_TEXT_SIZE = 72;
        public const int RST_TEXT_SIZE = 25;

        // grubość obramowania
        public const float DEF_BRD_THICKNESS = 6f;

        // marginesy
        public const int MINIATURE_TITLE_TOP_MARGIN = 25;
        public const int LEFT_MARGIN = 150;
        public const int TOP_MARGIN = 150;
        public const int TOP_RESET_MARGIN = 60;
        public const int V_SPACING = 50;
        public const int H_SPACING = 146;

        // komunikaty
        public const string DEFAULT_TITLE = "Project: Projectile";

        // ścieżki dostępu
        public const string MIN_EARTH_PATH = "../../media/images/earth_min.png";
        public const string MIN_MARS_PATH = "../../media/images/mars_min.png";
        public const string MIN_KEPLER_PATH = "../../media/images/kepler_22b_min.png";
        public const string MIN_MOON_PATH = "../../media/images/moon_min.png";
        public const string EARTH_PATH = "../../media/images/earth.png";
        public const string MARS_PATH = "../../media/images/mars.png";
        public const string KEPLER_PATH = "../../media/images/kepler_22b.png";
        public const string MOON_PATH = "../../media/images/moon.png";

        // STAŁE DOTYCZĄCE POZIOMU
        public const int THEORY_TEXT_SIZE = 25;
        public const int TITLE_TEXT_SIZE = 36;
        public const int CONST_TEXT_SIZE = 20;
        public const int FLIGHT_TEXT_SIZE = 25;
        public const int SHOT_TEXT_SIZE = 25;
        public const int STAGE_NUM_TEXT_SIZE = 30;
        public const int RESULT_TEXT_SIZE = 128;
        public const int RESULT_LAST_TEXT_SIZE = 62;
        public const int RESULT_BTN_TEXT_SIZE = 30;

        //grubość obramowania
        public const float PROJ_BRD_THICKNESS = 2f;
        public const float RESULT_BRD_THICKNESS = 3;
        public const float THEORY_BRD_THICKNESS = 3;

        // poziomy gruntu
        public const int MARS_BASE_ALT = 145;
        public const int EARTH_BASE_ALT = 115;
        public const int MOON_BASE_ALT = 115;
        public const int KEPLER_BASE_ALT = 115;

        // położenia procy i celu (X)
        public const int SLINGSHOT_X_EARTH = 140;
        public const int SLINGSHOT_X_MARS = 140;
        public const int SLINGSHOT_X_MOON = 140;
        public const int SLINGSHOT_X_KEPLER = 140;
        public const int TARGET_X_EARTH = 1140;
        public const int TARGET_X_MARS = 1140;
        public const int TARGET_X_MOON = 1140;
        public const int TARGET_X_KEPLER = 1140;

        // przyspieszenia grawitacyjne
        public const float G_EARTH = 9.81f;
        public const float G_MARS = 3.72f;
        public const float G_MOON = 1.62f;
        public const float G_KEPLER = 2.4f * G_EARTH; //PRZY ZAŁOZENIU, ŻE MA PODOBNY SKŁAD DO ZIEMI!

        // platformy
        public const int SLINGSHOT_PLATFORM_WIDTH = 20;
        public const int TARGET_PLATFORM_WIDTH = 10;

        // cięciwa
        public const float STRING_THICKNESS = 4;
        public const float STRING_DISTANCE = 0;

        // wymiary
        public const float RESULT_HEIGHT = 240;
        public const float RESULT_WIDTH = 426;
        public const float THEORY_HEIGHT = 600;
        public const float THEORY_WIDTH = 880;
        public const float RESULT_BTN_WIDTH = 140;
        public const float RESULT_BTN_HEIGHT = 45;
        public const int RST_X_RAD = 75;
        public const int RST_Y_RAD = 40;
        public const int THEORY_X_RAD = 75;
        public const int THEORY_Y_RAD = 40;
        public const int PROJECTILE_RAD = 5;

        // marginesy
        public const float RESULT_BTN_BOTTOM_MARGIN = 15;
        public const float RESULT_BTN_WIDTH_MARGIN = 26;
        public const float RESULT_TEXT_TOP_MARGIN = 30;
        public const int TITLE_TOP_MARGIN = 30;
        public const int STAGE_TOP_MARGIN = TITLE_TOP_MARGIN + TITLE_TEXT_SIZE + 15;
        public const int TOP_THEORY_BTN_MARGIN = 60;
        public const int LEFT_THEORY_BTN_MARGIN = DEFAULT_WIDTH-100;
        public const int RIGHT_RESET_MARGIN = 120;
        public const int CONST_TEXT_LEFT_MARGIN = 40;
        public const int CONST_TEXT_TOP_MARGIN = 22;
        public const int SHOT_TEXT_LEFT_MARGIN = 160;
        public const int SHOT_TEXT_TOP_MARGIN = 640;
        public const int FLIGHT_TEXT_LEFT_MARGIN = 790;
        public const int FLIGHT_TEXT_TOP_MARGIN = 22;
        public const int THEORY_TOP_MARGIN = 12;
        public const int THEORY_LEFT_MARGIN = 12;

        // stałe rozgrywki
        public const float DISTANCE_SCALE = 50; //[px/m]
        public const float ANGLE_CUTOFF = 15;
        public const float ANGLE_SAFE_K = 45;

        // komunikaty
        public const string SUCCESS_MAIN = "SUCCESS!";
        public const string SUCCESS_LAST_MAIN = "ALL STAGES\nCOMPLETED";
        public const string FAILURE_MAIN = "FAILURE";
        public const string SUCCESS_LEFT = "MAIN MENU";
        public const string SUCCESS_RIGHT = "NEXT STAGE";
        public const string FAILURE_LEFT = "MAIN MENU";
        public const string FAILURE_RIGHT = "TRY AGAIN";

        // ścieżki dostępu

        public const string STRING_SOUND_PATH = "../../media/sounds/rubber_stretch2.wav";
        public const string SHOT_SOUND_PATH = "../../media/sounds/shot.wav";
        public const string HIT_SOUND_PATH = "../../media/sounds/hit.wav";
        public const string MISS_SOUND_PATH = "../../media/sounds/miss.wav";
        public const string SLINGSHOT_PATH = "../../media/images/slingshot.png";
        public const string TARGET_PATH = "../../media/images/target.png";
        public const string THEORY_PATH = "../../media/images/theory.png";

        static C()
        { }
    }
}
