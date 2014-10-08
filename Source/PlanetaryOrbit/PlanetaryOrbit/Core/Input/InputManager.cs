using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POSystem;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PlanetaryOrbit.Core
{
    public class InputManager
    {
        Dictionary<string, InputHelper> mInputs = new Dictionary<string, InputHelper>();

        public MouseState CurrentMouseState;
        public MouseState PreviousMouseState;
        public MouseState OriginalMouseState;

        public InputManager(ref ConfigManager _config)
        {
            Logger.log(Log_Type.INFO, "Initialize Input");
            try
            {
                //Add Keyboard Input
                this.AddKeyboardInput("NAV_UP", _config.CoreSettings.INPUT_GENERAL_UP, true);
                this.AddKeyboardInput("NAV_DOWN", _config.CoreSettings.INPUT_GENERAL_DOWN, true);
                this.AddKeyboardInput("NAV_LEFT", _config.CoreSettings.INPUT_GENERAL_LEFT, true);
                this.AddKeyboardInput("NAV_RIGHT", _config.CoreSettings.INPUT_GENERAL_RIGHT, true);

                this.AddKeyboardInput("NAV_SELECT", _config.CoreSettings.INPUT_GENERAL_SELECT, true);
                this.AddKeyboardInput("NAV_CANCEL", _config.CoreSettings.INPUT_GENERAL_CANCEL, true);

                this.AddKeyboardInput("GLOBAL_START", _config.CoreSettings.INPUT_GENERAL_START, false);
            }
            catch (Exception ex)
            {
                Logger.log(Log_Type.ERROR, "FATAL: " + ex.Message);
            }

            Logger.log(Log_Type.INFO, "Initialize Input [Complete]");
        }

        public InputHelper NewInput(string action)
        {
            if (mInputs.ContainsKey(action) == false)
            {
                mInputs.Add(action, new InputHelper());
            }

            return mInputs[action];
        }

        public void startUpdate()
        {
            CurrentMouseState = Mouse.GetState();
            InputHelper.startUpdate();
        }

        public void endUpdate()
        {
            PreviousMouseState = CurrentMouseState;
            InputHelper.endUpdate();
        }

        public void resetAllInput()
        {
            mInputs.Clear();
        }

        public bool IsPressed(string action, PlayerIndex? player)
        {
            if (mInputs.ContainsKey(action) == false)
                return false;

            return mInputs[action].IsPressed(player);
        }

        public bool[] GetMouseButtons()
        {
            bool[] _buttons = new bool[3];

            if (this.CurrentMouseState.LeftButton == ButtonState.Pressed)
                _buttons[0] = true;
            else
                _buttons[0] = false;

            if (this.CurrentMouseState.MiddleButton == ButtonState.Pressed)
                _buttons[1] = true;
            else
                _buttons[1] = false;

            if (this.CurrentMouseState.RightButton == ButtonState.Pressed)
                _buttons[2] = true;
            else
                _buttons[2] = false;

            return _buttons;
        }

        public void AddGamePadInput(string action, Buttons buttonPressed, bool isReleased)
        {
            NewInput(action).AddGamepadInput(buttonPressed, isReleased);
        }

        public void AddKeyboardInput(string action, Keys keyPressed, bool isReleased)
        {
            NewInput(action).AddKeyboardInput(keyPressed, isReleased);
        }
    }
}
