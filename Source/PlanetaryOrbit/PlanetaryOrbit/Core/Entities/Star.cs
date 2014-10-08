using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using PlanetaryOrbit.Core;

namespace PlanetaryOrbit.Core
{
    class Star
    {
        public enum StarType
        {
            M,
            K,
            G,
            F,
            A,
            B,
            O,
            Undefined,
        };

        SolarFlare[] mSolarFlares =
        {
            new SolarFlare(-0.5f, 0.7f, new Color( 50,  25,  50), "flare1"),
            new SolarFlare( 0.3f, 0.4f, new Color(100, 255, 200), "flare1"),
            new SolarFlare( 1.2f, 1.0f, new Color(100,  50,  50), "flare1"),
            new SolarFlare( 1.5f, 1.5f, new Color( 50, 100,  50), "flare1"),

            new SolarFlare(-0.3f, 0.7f, new Color(200,  50,  50), "flare2"),
            new SolarFlare( 0.6f, 0.9f, new Color( 50, 100,  50), "flare2"),
            new SolarFlare( 0.7f, 0.4f, new Color( 50, 200, 200), "flare2"),

            new SolarFlare(-0.7f, 0.7f, new Color( 50, 100,  25), "flare3"),
            new SolarFlare( 0.0f, 0.6f, new Color( 25,  25,  25), "flare3"),
            new SolarFlare( 2.0f, 1.4f, new Color( 25,  50, 100), "flare3"),
        };

        public Vector3 mColour;
        public float mScale;
        public StarType mType;

        //Occlusion
        bool mOcclusionWait;
        OcclusionQuery mOcclusionQuery;
        bool mOcclusionQueryActive = false;
        float mOcclusionAlpha;

        //Solar Settings
        Effect mStarShader;
        Texture2D mGlowSprite;
        GraphicsDevice _obj_graphics;
        float mGlowSize = 400;
        

        public void SetType(StarType _type)
        {
            this.mType = _type;
            switch (this.mType)
            {
                case StarType.A:
                    this.mScale = 4;
                    this.mColour = new Vector3(0.5f, 1, 0.5f);
                    break;

                case StarType.B:
                    this.mScale = 6;
                    this.mColour = new Vector3(0.8f, 0.8f, 1.0f);
                    break;

                case StarType.F:
                    this.mScale = 1;
                    this.mColour = new Vector3(1, 1, 1);
                    break;

                case StarType.G: //Sun
                    this.mScale = 0.8f;
                    this.mColour = new Vector3(1, 1, 0.3f);
                    break;

                case StarType.K:
                    this.mScale = 0.7f;
                    this.mColour = new Vector3(1, 0.8f, 0.3f);
                    break;

                case StarType.M:
                    this.mScale = 0.5f;
                    this.mColour = new Vector3(1, 0.3f, 0.3f);
                    break;

                case StarType.O:
                    this.mScale = 10;
                    this.mColour = new Vector3(0.3f, 0.3f, 1);
                    break;

            }

            this.mGlowSize = 30 * this.mScale;
        }

        public Star(GraphicsDevice _graphics, ContentManager _content)
        {
            this.mOcclusionQuery = new OcclusionQuery(_graphics);
            this.mStarShader = _content.Load<Effect>("Core/Shaders/Special/Star");
            this.mGlowSprite = _content.Load<Texture2D>("Core/Textures/Extra/Star/glow");
            this._obj_graphics = _graphics;

            foreach (SolarFlare localFlare in this.mSolarFlares)
            {
                localFlare.mTexture = _content.Load<Texture2D>("Core/Textures/Extra/Flares/" + localFlare.mTextureName);
            }
        }

        public void Draw(BaseCamera _camera, bool _drawShaders)
        {
            if (this.mOcclusionQueryActive)
            {
                if (this.mOcclusionQuery.IsComplete)
                {
                    const float queryArea = 16 * 16;
                    if (this.mOcclusionQuery.PixelCount > 0)
                    {
                        this.mOcclusionAlpha = Math.Min(this.mOcclusionQuery.PixelCount / queryArea, 1);
                    }
                    else
                    {
                        this.mOcclusionAlpha = 0;
                    }
                }
                else
                {
                    this.mOcclusionWait = true;
                }
            }
            else
            {
                this.mOcclusionQuery.Begin();
                this.mOcclusionQueryActive = true;
                this.mOcclusionWait = false;
            }

            
        }
    }
}
