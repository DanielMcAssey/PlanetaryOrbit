using System;
using System.Collections.Generic;
using System.Linq;
using POSystem;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using PlanetaryOrbit.Core;
using PlanetaryOrbit.Core.Screen;

namespace PlanetaryOrbit
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _obj_graphics;
        ConfigManager _obj_config;
        ScreenManager _obj_screenmanager;

        public Game()
        {
            Logger.log(Log_Type.INFO, "Initialize Game");
            this._obj_config = new ConfigManager();

            this.Window.Title = GameSettings.CONFIG_GAME_NAME;
            this.Content.RootDirectory = GameSettings.CONFIG_CONTENT_ROOT;
            IsFixedTimeStep = true;

            Logger.log(Log_Type.INFO, "Initialize Graphics and Sound");

            try
            {
                this._obj_graphics = new GraphicsDeviceManager(this);
            }
            catch (Exception ex)
            {
                Logger.log(Log_Type.ERROR, "FATAL (GPU): " + ex.Message);
            }

            Logger.log(Log_Type.INFO, "Game Configuration");
            try
            {
                this._obj_graphics.PreferredBackBufferWidth = this._obj_config.CoreSettings.VIDEO_RES_WIDTH;
                this._obj_graphics.PreferredBackBufferHeight = this._obj_config.CoreSettings.VIDEO_RES_HEIGHT;
                this._obj_graphics.IsFullScreen = this._obj_config.CoreSettings.VIDEO_FULLSCREEN;
                this._obj_graphics.PreferMultiSampling = this._obj_config.CoreSettings.VIDEO_ANTIALIASING;
                this._obj_graphics.SynchronizeWithVerticalRetrace = this._obj_config.CoreSettings.VIDEO_VSYNC;
                this._obj_graphics.PreferredDepthStencilFormat = (DepthFormat)this._obj_config.CoreSettings.VIDEO_DEPTH_STENCIL_BUFFER;
                this._obj_graphics.ApplyChanges();
            }
            catch (Exception ex)
            {
                Logger.log(Log_Type.INFO, "Creating New Configuration File - " + ex.Message);
                this._obj_config.CreateNewSettings();
                this._obj_graphics.PreferredBackBufferWidth = this._obj_config.CoreSettings.VIDEO_RES_WIDTH;
                this._obj_graphics.PreferredBackBufferHeight = this._obj_config.CoreSettings.VIDEO_RES_HEIGHT;
                this._obj_graphics.IsFullScreen = this._obj_config.CoreSettings.VIDEO_FULLSCREEN;
                this._obj_graphics.PreferMultiSampling = this._obj_config.CoreSettings.VIDEO_ANTIALIASING;
                this._obj_graphics.SynchronizeWithVerticalRetrace = this._obj_config.CoreSettings.VIDEO_VSYNC;
                this._obj_graphics.PreferredDepthStencilFormat = (DepthFormat)this._obj_config.CoreSettings.VIDEO_DEPTH_STENCIL_BUFFER;
                this._obj_graphics.ApplyChanges();
            }
            Logger.log(Log_Type.INFO, "Game Configuration [Complete]");
            Logger.log(Log_Type.INFO, "Initialize Graphics and Sound [Complete]");
            Logger.log(Log_Type.INFO, "Initialize Screen Manager");
            this._obj_screenmanager = new ScreenManager(this, this._obj_config, this._obj_graphics);
            this.Components.Add(this._obj_screenmanager);
            this._obj_screenmanager.addScreen(new ScreenBG(), null);
            this._obj_screenmanager.addScreen(new ScreenMenuRoot(), PlayerIndex.One);
            Logger.log(Log_Type.INFO, "Initialize Screen Manager [Complete]");
            Logger.log(Log_Type.INFO, "Initialize Game [Complete]");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
    }
}
