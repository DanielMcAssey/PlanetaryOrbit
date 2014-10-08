using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlanetaryOrbit.Core
{
    class SolarFlare
    {
        public float mPosition;
        public float mScale;
        public Color mColor;
        public string mTextureName;
        public Texture2D mTexture;

        public SolarFlare(float _position, float _scale, Color _color, string _textureName)
        {
            this.mPosition = _position;
            this.mScale = _scale;
            this.mColor = _color;
            this.mTextureName = _textureName;
        }
    }
}
