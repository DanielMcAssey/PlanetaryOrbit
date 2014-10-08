using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlanetaryOrbit.Core
{
    public class PlanetRing
    {
        protected GraphicsDevice _obj_graphics;
        protected Texture2D mTexRing, mTexAlpha, mTexNoise;
        protected Vector3 mPos, mRot, mScale;
        protected Model mModel;
        protected Effect mRingShader;

        protected float shaderAmbientIntensity, shaderDiffuseIntensity;
        protected Color shaderAmbientColour, shaderDiffuseColour;

        protected Vector3 shaderLightDirection;

        protected Matrix matWorld;
        protected Matrix[] matTransforms;

        protected float mPlanetRadius;

        protected VertexPositionColor[] vertices;

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

        public Vector3 LightDirection
        {
            get { return this.shaderLightDirection; }
            set { this.shaderLightDirection = value; }
        }

        public float PlanetRadiusSquared
        {
            get { return this.mPlanetRadius; }
            set { this.mPlanetRadius = value; }
        }
        #endregion

        public PlanetRing(GraphicsDevice _graphics, Texture2D _ring, Texture2D _alpha, Vector3 _scale, Model _model, Effect _shader)
        {
            this._obj_graphics = _graphics;
            this.mTexRing = _ring;
            this.mTexAlpha = _alpha;
            this.mScale = _scale;
            this.mModel = _model;
            this.mRingShader = _shader;

            this.shaderAmbientIntensity = 0.05f;
            this.shaderAmbientColour = Color.Black;

            this.shaderDiffuseIntensity = 0.75f;
            this.shaderDiffuseColour = Color.White;

            this.matWorld = Matrix.Identity;
            this.matTransforms = new Matrix[this.mModel.Bones.Count];
            this.mModel.CopyAbsoluteBoneTransformsTo(this.matTransforms);
        }

        public void Update(TimeSpan _elapsedTime)
        {
            this.matWorld = (Matrix.CreateScale(this.mScale) * (Matrix.CreateRotationY(this.mRot.Y) * Matrix.CreateRotationZ(this.mRot.Z) * Matrix.CreateRotationX(this.mRot.X)) * Matrix.CreateTranslation(this.mPos));
        }

        public void Draw(BaseCamera _camera, bool _drawShaders)
        {
            foreach (ModelMesh localMesh in this.mModel.Meshes)
            {
                Matrix localWorld = this.matTransforms[localMesh.ParentBone.Index] * this.matWorld;
                Matrix localWorldInverseTranspose = Matrix.Transpose(Matrix.Invert(localWorld));

                if (this.mRingShader != null && _drawShaders == true)
                {
                    foreach (ModelMeshPart localMeshPart in localMesh.MeshParts)
                    {
                        localMeshPart.Effect = this.mRingShader;

                        //Set initial shader parameters
                        if (this.mRingShader.Parameters["xWorld"] != null)
                            this.mRingShader.Parameters["xWorld"].SetValue(localWorld);

                        if (this.mRingShader.Parameters["xView"] != null)
                            this.mRingShader.Parameters["xView"].SetValue(_camera.View);

                        if (this.mRingShader.Parameters["xProjection"] != null)
                            this.mRingShader.Parameters["xProjection"].SetValue(_camera.Projection);

                        if (this.mRingShader.Parameters["xWorldInverseTranspose"] != null)
                            this.mRingShader.Parameters["xWorldInverseTranspose"].SetValue(localWorldInverseTranspose);

                        if (this.mRingShader.Parameters["xCameraPosition"] != null)
                            this.mRingShader.Parameters["xCameraPosition"].SetValue(_camera.Position);

                        if (this.mRingShader.Parameters["xLightPosition"] != null)
                            this.mRingShader.Parameters["xLightPosition"].SetValue(this.shaderLightDirection);

                        if (this.mRingShader.Parameters["xAmbientIntensity"] != null)
                            this.mRingShader.Parameters["xAmbientIntensity"].SetValue(this.shaderAmbientIntensity);

                        if (this.mRingShader.Parameters["xAmbientColor"] != null)
                            this.mRingShader.Parameters["xAmbientColor"].SetValue(this.shaderAmbientColour.ToVector4());

                        if (this.mRingShader.Parameters["xDiffuseIntensity"] != null)
                            this.mRingShader.Parameters["xDiffuseIntensity"].SetValue(this.shaderDiffuseIntensity);

                        if (this.mRingShader.Parameters["xDiffuseColor"] != null)
                            this.mRingShader.Parameters["xDiffuseColor"].SetValue(this.shaderDiffuseColour.ToVector4());

                        if (this.mRingShader.Parameters["ColourMap"] != null && this.mTexRing != null)
                            this.mRingShader.Parameters["ColourMap"].SetValue(this.mTexRing);

                        if (this.mRingShader.Parameters["AlphaMap"] != null && this.mTexAlpha != null)
                            this.mRingShader.Parameters["AlphaMap"].SetValue(this.mTexAlpha);

                        if (this.mRingShader.Parameters["NoiseMap"] != null && this.mTexNoise != null)
                            this.mRingShader.Parameters["NoiseMap"].SetValue(this.mTexNoise);

                        this.mRingShader.CurrentTechnique = this.mRingShader.Techniques["RingShader"];

                        foreach (EffectPass localEffectPass in this.mRingShader.CurrentTechnique.Passes)
                        {
                            localEffectPass.Apply();
                        }
                    }
                }
                else if (_drawShaders == false)
                {
                    foreach (BasicEffect localShader in localMesh.Effects)
                    {
                        localShader.EnableDefaultLighting();

                        localShader.TextureEnabled = true;
                        localShader.Texture = this.mTexRing;
                        localShader.World = localWorld;
                        localShader.View = _camera.View;
                        localShader.Projection = _camera.Projection;
                    }
                }

                localMesh.Draw();
            }
        }
    }
}
