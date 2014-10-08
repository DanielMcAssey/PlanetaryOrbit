using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSystem
{
    public class GameSettings
    {
        //General Configuration
        public const string CONFIG_GAME_BUILD = "0.6.3";
        public const string CONFIG_GAME_BUILD_TYPE = "Development";
        public const string CONFIG_GAME_NAME = "PlanetaryOrbit - " + CONFIG_GAME_BUILD_TYPE + " Build (" + CONFIG_GAME_BUILD + ")";
        public const string CONFIG_GAME_NAME_CLEAN = "PlanetaryOrbit";

        //Debug Settings
        public const string CONFIG_DEBUG_JIRA = "jira.blackholedev.net";
        public const string CONFIG_DEBUG_JIRA_METHOD = "POST";
        public const string CONFIG_DEBUG_JIRA_KEY = "pAG7zeZ16AY1Ht3924QsC1rup8T3x8pC";

        //Directory Mappings
        public const string CONFIG_CONTENT_ROOT = "Content";
        public const string CONFIG_DATA_DIR_SHADERS = "Core/Shaders/";
        public const string CONFIG_DATA_DIR_GUI = "System/UI/";
        public const string CONFIG_DATA_DIR_SETTINGS = "System/Config/";
        public const string CONFIG_DATA_DIR_GUI_FONTS = CONFIG_DATA_DIR_GUI + "Fonts/";
        public const string CONFIG_DATA_DIR_GUI_BG = CONFIG_DATA_DIR_GUI + "Backgrounds/";

        //Default Assets
        public const string ASSET_CONFIG_GUI_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_GUI";
        public const string ASSET_CONFIG_HUD_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_HUD";
        public const string ASSET_CONFIG_DEBUG_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_DEBUG";
        public const string ASSET_CONFIG_PLANET_FONT = CONFIG_DATA_DIR_GUI_FONTS + "font_PLANET";
        public const string ASSET_CONFIG_BLANK_BG = CONFIG_DATA_DIR_GUI_BG + "texture_BLANK";
        public const string ASSET_CONFIG_DEFAULT_BG = CONFIG_DATA_DIR_GUI_BG + "bg";
        public const string ASSET_CONFIG_MSGBOX_BG = CONFIG_DATA_DIR_GUI_BG + "msgbox";

        //Settings
        public const string ASSET_CONFIG_SETTINGS_FILE = "base.config";
        public const string ASSET_CONFIG_SYSTEMS_FILE = "systems.stor";

        //Default Strings
        public const string ASSET_CONFIG_MSG_LOADING = "-LOADING-";
        public const string ASSET_CONFIG_MSG_BOX_USAGE = "\n'Enter' - Yes\n'Escape' - No";

        //Physics
        public const double PHYSICS_GRAVITY_CONSTANT = 6.67384E-11;
        public const double PHYSICS_GAUSS_GRAVITY_CONSTANT = 0.01720209895;
        public const double PHYSICS_SPEED_OF_LIGHT = 299792458.0;
        public const double PHYSICS_SPEED_OF_LIGHT_SQUARED = PHYSICS_SPEED_OF_LIGHT * PHYSICS_SPEED_OF_LIGHT;
        public const double PHYSICS_EPSILON = 0.40909281391860255966307657332412;
        public static double PHYSICS_EPSILON_CE = Math.Cos(PHYSICS_EPSILON);
        public static double PHYSICS_EPSILON_SE = Math.Sin(PHYSICS_EPSILON);
        public static long[] PHYSICS_BS_CONSTANTS = { 1, 2, 3, 5, 8, 13 };
        public const double PHYSICS_KM_TO_AU = 6.684587122671E-9;
        public const double PHYSICS_SOLAR_MASS = 1.98892000114E+30;
    }
}
