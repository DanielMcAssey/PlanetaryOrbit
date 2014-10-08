using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlanetaryOrbit.Core.Screen
{
    class ScreenSceneSelector : BaseGUIScreen
    {
        protected List<PeopleCredited> mCreditList;
        protected int mMaxTextLength;
        protected float mMaxWidth;
        protected float mTextScale;

        public ScreenSceneSelector()
            : base("Select Scene File", Color.White, false, null, false, 1f)
        {
        }

        public override void loadContent()
        {
            List<SceneFileMin> allScenes = this.ScreenManager.ConfigManager.GetAllScenes();
            for (int i = 0; i < allScenes.Count; i++)
            {
                MenuItemBasic menuItem = new MenuItemBasic(allScenes[i].FILE_NAME, this.GlobalContentManager);
                if (!allScenes[i].FILE_EXISTS) //Change color
                {
                    menuItem.NonSelectedColour = Color.Red;
                    menuItem.SelectedColour = Color.HotPink;
                }
                menuItem.OnSelected += EventTriggerGoToGame;
                this._list_menuitems.Add(menuItem);
            }

            base.loadContent();
        }

        public override void update()
        {
            if(this.GlobalInput.IsPressed("NAV_CANCEL", this.ControllingPlayer)) //If player presses cancel button (Escape/B)
                this.exitScreen(); //Exit the screen.

            base.update(); 
        }

        public override void render()
        {
            base.render();
        }

        //---------------EVENT HANDLERS-------------------------------------------------------------

        /// <summary>
        /// Event Handler to Go to character select screen.
        /// </summary>
        void EventTriggerGoToGame(object sender, EventPlayer e)
        {

            ScreenLoading.Load(this.ScreenManager, "LOADING", true, this.ControllingPlayer, new ScreenGame());
        }
    }
}
