using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlanetaryOrbit.Core.Models
{
    public struct ShaderPair
    {
        public enumShaderType Type;
        public Effect Shader;
    }

    public enum enumTextureType
    {
        MAP_COLOUR = 0,
        MAP_BUMP,
        MAP_SPECULAR,
        MAP_NORMAL,
        MAP_LIGHTS,
        MAP_CLOUD,
        MAP_WATER,
        FLAT_COLOUR,
        LAYER_SPECIAL1,
        LAYER_SPECIAL2,
        LAYER_SPECIAL3
    }

    public enum enumShaderType
    {
        TEXTURE = 0,
        FLAT,
        NORMAL,
        BUMP,
        EARTH,
        SUN
    }

    public abstract class BaseModel
    {
        protected Model mModel;

        protected Vector3 mPos;
        protected Vector3 mRot;
        protected Vector3 mScale;
        protected float mRadius;

        protected enumShaderType mShaderType;
        protected Dictionary<string, ShaderPair> mShaders;

        protected bool enableLighting;
        protected Vector3 shaderLightDirection, shaderLightColour;
        protected float shaderSpecularIntensity, shaderAmbientIntensity, shaderDiffuseIntensity;
        protected Color shaderSpecularColour, shaderAmbientColour, shaderDiffuseColour;

        protected Dictionary<enumTextureType, Texture2D> mTextures;
        protected Dictionary<enumTextureType, TextureCube> mTexturesCube;

        protected Matrix matWorld;
        protected Matrix[] matTransforms;

        protected BoundingSphere bsObject;

        protected TimeSpan mElapsedTime;

        #region "Properties"
        public Vector3 Position
        {
            get { return this.mPos; }
            set { this.mPos = value; }
        }

        public Vector3 Rotation
        {
            get { return this.mRot; }
            set { this.mRot = value; }
        }

        public BoundingSphere CurrentBounds
        {
            get { return this.bsObject.Transform(Matrix.CreateScale(this.mScale) * Matrix.CreateTranslation(this.mPos)); }
        }

        public Vector3 LightDirection
        {
            get { return this.shaderLightDirection; }
            set { this.shaderLightDirection = value; }
        }

        public float SpecularIntensity
        {
            get { return this.shaderSpecularIntensity; }
            set { this.shaderSpecularIntensity = value; }
        }

        public float AmbientIntensity
        {
            get { return this.shaderAmbientIntensity; }
            set { this.shaderAmbientIntensity = value; }
        }

        public Color AmbientColor
        {
            get { return this.shaderAmbientColour; }
            set { this.shaderAmbientColour = value; }
        }

        public Matrix World
        {
            get { return this.matWorld; }
        }
        #endregion

        public BaseModel(Model _model, Vector3 _position, Vector3 _rotation, Vector3 _scale)
        {
            this.mModel = _model;
            
            this.mPos = _position;
            this.mRot = _rotation;
            this.mScale = _scale;

            this.mTextures = new Dictionary<enumTextureType, Texture2D>();
            this.mTexturesCube = new Dictionary<enumTextureType, TextureCube>();
            this.mShaders = new Dictionary<string, ShaderPair>();

            this.matWorld = Matrix.Identity;
            this.matTransforms = new Matrix[this.mModel.Bones.Count];
            this.mModel.CopyAbsoluteBoneTransformsTo(this.matTransforms);

            this.enableLighting = true;

            this.shaderLightDirection = new Vector3(0.0f, 0.0f, 0.0f);
            this.shaderLightDirection.Normalize();

            this.shaderLightColour = new Vector3(1.0f, 1.0f, 1.0f);

            this.shaderSpecularIntensity = 10f;
            this.shaderSpecularColour = Color.White;

            this.shaderAmbientIntensity = 0.35f;
            this.shaderAmbientColour = Color.Gray;

            this.shaderDiffuseIntensity = 1f;
            this.shaderDiffuseColour = Color.White;

            BoundingSphere tmpSphere = new BoundingSphere(Vector3.Zero, 0);
            foreach (ModelMesh tmesh in this.mModel.Meshes)
            {
                tmpSphere = BoundingSphere.CreateMerged(tmpSphere, tmesh.BoundingSphere.Transform(this.matTransforms[tmesh.ParentBone.Index]));
            }

            this.bsObject = tmpSphere;
        }

        public BaseModel(Model _model)
            : this(_model, Vector3.Zero, Vector3.Zero, Vector3.One)
        {
        }

        public void AddTexture(enumTextureType _type, Texture2D _texture)
        {
            this.mTextures.Add(_type, _texture);
        }

        public void AddTextureCube(enumTextureType _type, TextureCube _texture)
        {
            this.mTexturesCube.Add(_type, _texture);
        }

        public void AddShader(string _ref, enumShaderType _type, Effect _shader)
        {
            ShaderPair _pair = new ShaderPair();
            _pair.Type = _type;
            _pair.Shader = _shader;
            this.mShaders.Add(_ref, _pair);
        }

        public abstract void UpdateModel(TimeSpan _totalTime);
        public abstract void DrawModel(BaseCamera _camera, bool _drawShaders);

    }
}
