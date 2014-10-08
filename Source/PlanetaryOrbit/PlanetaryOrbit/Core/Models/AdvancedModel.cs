using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlanetaryOrbit.Core.Models
{
    public class AdvancedModel : BaseModel
    {
        public AdvancedModel(Model _model, Vector3 _position, Vector3 _rotation, Vector3 _scale)
            : base(_model, _position, _rotation, _scale)
        {
        }

        public AdvancedModel(Model _model)
            : base(_model)
        {
        }

        public override void UpdateModel(TimeSpan _elapsedTime)
        {
            this.mElapsedTime = _elapsedTime;
            this.matWorld = (Matrix.CreateScale(this.mScale) * (Matrix.CreateRotationY(this.mRot.Y) * Matrix.CreateRotationZ(this.mRot.Z) * Matrix.CreateRotationX(this.mRot.X)) * Matrix.CreateTranslation(this.mPos));
        }

        public override void DrawModel(BaseCamera _camera, bool _drawShaders)
        {
            foreach (ModelMesh localMesh in this.mModel.Meshes)
            {
                Matrix localWorld = this.matTransforms[localMesh.ParentBone.Index] * this.matWorld;
                Matrix localWorldInverseTranspose = Matrix.Transpose(Matrix.Invert(localWorld));

                if (this.mShaders.Count > 0 && _drawShaders == true)
                {
                    foreach (ModelMeshPart localMeshPart in localMesh.MeshParts)
                    {
                        localMeshPart.Effect = this.mShaders[this.mShaders.Keys.First()].Shader;

                        //Set initial shader parameters
                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xWorld"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xWorld"].SetValue(localWorld);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xView"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xView"].SetValue(_camera.View);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xProjection"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xProjection"].SetValue(_camera.Projection);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xWorldInverseTranspose"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xWorldInverseTranspose"].SetValue(localWorldInverseTranspose);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xCameraPosition"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xCameraPosition"].SetValue(_camera.Position);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xEnableLighting"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xEnableLighting"].SetValue(this.enableLighting);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xLightDirection"] != null)
                        {
                            this.shaderLightDirection.Normalize();
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xLightDirection"].SetValue(this.shaderLightDirection);
                        }

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xLightColor"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xLightColor"].SetValue(this.shaderLightColour);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xSpecularIntensity"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xSpecularIntensity"].SetValue(this.shaderSpecularIntensity);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xSpecularColor"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xSpecularColor"].SetValue(this.shaderSpecularColour.ToVector4());

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xAmbientIntensity"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xAmbientIntensity"].SetValue(this.shaderAmbientIntensity);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xAmbientColor"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xAmbientColor"].SetValue(this.shaderAmbientColour.ToVector4());

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xDiffuseIntensity"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xDiffuseIntensity"].SetValue(this.shaderDiffuseIntensity);

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xDiffuseColor"] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xDiffuseColor"].SetValue(this.shaderDiffuseColour.ToVector4());

                        if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["ModelTexture"] != null && this.mTextures[enumTextureType.MAP_COLOUR] != null)
                            this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["ModelTexture"].SetValue(this.mTextures[enumTextureType.MAP_COLOUR]);

                        switch (this.mShaders[this.mShaders.Keys.First()].Type)
                        {
                            case enumShaderType.NORMAL:
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["NormalMap"].SetValue(this.mTextures[enumTextureType.MAP_NORMAL]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.CurrentTechnique = this.mShaders[this.mShaders.Keys.First()].Shader.Techniques["Normal"];
                                break;
                            case enumShaderType.BUMP:
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["NormalMap"].SetValue(this.mTextures[enumTextureType.MAP_BUMP]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.CurrentTechnique = this.mShaders[this.mShaders.Keys.First()].Shader.Techniques["Textured"];
                                break;
                            case enumShaderType.FLAT:
                                this.mShaders[this.mShaders.Keys.First()].Shader.CurrentTechnique = this.mShaders[this.mShaders.Keys.First()].Shader.Techniques["Pretransformed"];
                                break;
                            case enumShaderType.SUN: //Sun Shader
                                this.mShaders[this.mShaders.Keys.First()].Shader.CurrentTechnique = this.mShaders[this.mShaders.Keys.First()].Shader.Techniques["StarColour"];

                                if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xWVP"] != null)
                                    this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xWVP"].SetValue(localWorld * _camera.View * _camera.Projection);

                                if (this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xColour"] != null)
                                    this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["xColour"].SetValue(this.shaderAmbientColour.ToVector3());
                                break;
                            case enumShaderType.EARTH: //Earth Shader
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["wvp"].SetValue(localWorld * _camera.View * _camera.Projection);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["time"].SetValue((float)this.mElapsedTime.TotalSeconds * 3);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["cloudSpeed"].SetValue(0.0025f);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["cloudHeight"].SetValue(0.005f);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["cloudShadowIntensity"].SetValue(0.2f);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["NormalMap"].SetValue(this.mTextures[enumTextureType.MAP_NORMAL]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["GlowMap"].SetValue(this.mTextures[enumTextureType.MAP_LIGHTS]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["SpecularMap"].SetValue(this.mTextures[enumTextureType.MAP_SPECULAR]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["CloudMap"].SetValue(this.mTextures[enumTextureType.MAP_CLOUD]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["WaveMap"].SetValue(this.mTextures[enumTextureType.MAP_WATER]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.Parameters["AtmosMap"].SetValue(this.mTextures[enumTextureType.LAYER_SPECIAL1]);
                                this.mShaders[this.mShaders.Keys.First()].Shader.CurrentTechnique = this.mShaders[this.mShaders.Keys.First()].Shader.Techniques["EarthShader"];
                                break;
                        }

                        foreach (EffectPass localEffectPass in this.mShaders[this.mShaders.Keys.First()].Shader.CurrentTechnique.Passes)
                            localEffectPass.Apply();

                    }
                }
                else if (_drawShaders == false)
                {
                    foreach (BasicEffect localShader in localMesh.Effects)
                    {
                        localShader.EnableDefaultLighting();

                        localShader.TextureEnabled = true;
                        localShader.Texture = this.mTextures[enumTextureType.MAP_COLOUR];
                        localShader.World = localWorld;    //Tell the shader what World Matrix to use to draw the object.
                        localShader.View = _camera.View;             //Tell the shader where to draw from (the camera).
                        localShader.Projection = _camera.Projection; //Tell the shader how to draw (like the camera lens... sort of).
                    }
                }

                localMesh.Draw();
            }
        }
    }
}
