using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace POSystem
{
    public class SystemSettings
    {
        public enum ControlType
        {
            _KEYBOARD,
            _GAMEPAD
        }

        //Game Settings
        public bool GAME_TRUESCALE = false;

        //Video Settings
        public int VIDEO_RES_WIDTH = 1280;
        public int VIDEO_RES_HEIGHT = 768;
        public bool VIDEO_FULLSCREEN = false;
        public float VIDEO_BRIGHTNESS = 0.5f;
        public float VIDEO_CONTRAST = 0.5f;
        public bool VIDEO_ANTIALIASING = true;
        public bool VIDEO_VSYNC = false;
        public int VIDEO_DEPTH_STENCIL_BUFFER = 3;
        public bool VIDEO_OCULUS_ENABLED = false;

        //Audio Settings
        public float AUDIO_MAIN_VOLUME = 1.0f;
        public float AUDIO_MUSIC_VOLUME = 1.0f;
        public float AUDIO_SFX_VOLUME = 1.0f;

        //Simulation Settings
        public bool SIM_ENABLED = false;
        public string SIM_PLANET = "sun";
        public int SIM_SPEED = 400;

        //Current Control Scheme
        public int CURRENT_CONTROL_TYPE = (int)ControlType._KEYBOARD;

        //Controls
        public Keys INPUT_GENERAL_UP = Keys.Up;
        public Keys INPUT_GENERAL_DOWN = Keys.Down;
        public Keys INPUT_GENERAL_LEFT = Keys.Left;
        public Keys INPUT_GENERAL_RIGHT = Keys.Right;
        public Keys INPUT_GENERAL_SELECT = Keys.Enter;
        public Keys INPUT_GENERAL_CANCEL = Keys.Escape;
        public Keys INPUT_GENERAL_START = Keys.Enter;
    }
}
