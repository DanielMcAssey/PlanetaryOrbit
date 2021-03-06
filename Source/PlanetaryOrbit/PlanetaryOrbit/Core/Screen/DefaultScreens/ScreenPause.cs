﻿using Microsoft.Xna.Framework;
using POSystem;

namespace PlanetaryOrbit.Core.Screen
{
    class ScreenPause : BaseGUIScreen
    {
        //----------------CLASS CONSTANTS---------------------------------------------------------
        public const float DEFAULT_ALPHA = 2.0f / 3.0f;
        public const int DEFAULT_PADDING_H = 32;
        public const int DEFAULT_PADDING_V = 16;
        public static readonly Color DEFAULT_COLOUR = Color.White;

        //----------------CLASS MEMBERS-----------------------------------------------------------
        protected float _message_alpha;

        //-----------------CONSTRUCTOR----------------------------------------------------------------

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScreenPause()
            : base("PAUSED", Color.White, false, null, false, 1f)
        {
            this._is_popup = true;
            this._message_alpha = DEFAULT_ALPHA;
        }

        public override void loadContent()
        {
            // Create our menu entries.
            MenuItemBasic tentryresume = new MenuItemBasic("RESUME", this.GlobalContentManager);
            MenuItemBasic tentryquit = new MenuItemBasic("EXIT", this.GlobalContentManager);

            // Hook up menu event handlers.
            tentryresume.OnSelected += DefaultTriggerMenuBack;
            tentryquit.OnSelected += EventTriggerGoToMain;

            // Add entries to the menu.
            this._list_menuitems.Add(tentryresume);
            this._list_menuitems.Add(tentryquit);

            this.BGColour = Color.Black;
            base.loadContent();
        }

        public override void render()
        {
            this.ScreenManager.fadeBackBuffer(this.CurrentTransitionAlpha * this._message_alpha);
            base.render();
        }

        //-----------------EVENT HANDLER DELEGATES---------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EventTriggerGoToMain(object sender, EventPlayer e)
        {
            const string tmessage = "Are you sure you want to exit the simulation?";
            ScreenMsgBox tmsgboxconfirmquit = new ScreenMsgBox(GameSettings.ASSET_CONFIG_MSGBOX_BG, tmessage);
            tmsgboxconfirmquit.onAccepted += EventTriggerConfirmGoToMain;
            ScreenManager.addScreen(tmsgboxconfirmquit, ControllingPlayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EventTriggerConfirmGoToMain(object sender, EventPlayer e)
        {
            //Trigger the Loading Screen to load our Background Menu and Overlay it with our Menu Screen
            ScreenLoading.Load(ScreenManager, this.ControllingPlayer, null, new ScreenBG(), new ScreenMenuRoot());
        }
    }
}
