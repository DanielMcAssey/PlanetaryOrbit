using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlanetaryOrbit.Core.Models
{
    class AstronomicalObject : BaseModel
    {
        public AstronomicalObject(Model _model, Vector3 _position, Vector3 _rotation, Vector3 _scale)
            : base(_model, _position, _rotation, _scale)
        {
        }

        public AstronomicalObject(Model _model)
            : base(_model)
        {
        }

        public override void UpdateModel()
        {
            this.matWorld = (Matrix.CreateScale(this.mScale) * Matrix.CreateFromYawPitchRoll(this.mRot.Y, this.mRot.X, this.mRot.Z) * Matrix.CreateTranslation(this.mTransform));

            this.mRot.Z += 0.01f;
            this.shaderLightDirection = Vector3.Transform(this.shaderLightDirection, Matrix.CreateRotationY(0.01f));
        }

        public override void DrawModel(Matrix _view, Matrix _projection, bool _drawShaders)
        {
            foreach (ModelMesh localMesh in this.mModel.Meshes)
            {
                Matrix localWorld = this.matWorld * this.matTransforms[localMesh.ParentBone.Index] * Matrix.CreateTranslation(this.mPos);
                Matrix localWorldInverseTranspose = Matrix.Transpose(Matrix.Invert(localMesh.ParentBone.Transform * localWorld));

                if (this.mShader != null && _drawShaders == true)
                {
                    foreach (ModelMeshPart localMeshPart in localMesh.MeshParts)
                    {
                        localMeshPart.Effect = mShader;
                        mShader.Parameters["World"].SetValue(localWorld);
                        mShader.Parameters["View"].SetValue(_view);
                        mShader.Parameters["Projection"].SetValue(_projection);
                        mShader.Parameters["ModelTexture"].SetValue(this.mTextures[enumTextureType.MAP_COLOUR]);
                        mShader.Parameters["WorldInverseTranspose"].SetValue(localWorldInverseTranspose);
                        mShader.Parameters["DiffuseLightDirection"].SetValue(this.shaderLightDirection);
                        //mShader.Parameters["DiffuseIntensity"].SetValue(this.shaderLightIntensity);
                        //mShader.Parameters["SpecularIntensity"].SetValue(this.shaderSpecularIntensity);
                        //mShader.Parameters["AmbientIntensity"].SetValue(this.shaderAmbientIntensity);

                        switch (this.mShaderType)
                        {
                            case enumTextureType.MAP_NORMAL:
                                mShader.Parameters["NormalMap"].SetValue(this.mTextures[enumTextureType.MAP_NORMAL]);
                                mShader.CurrentTechnique = mShader.Techniques["Normal"];
                                break;
                            case enumTextureType.MAP_BUMP:
                                mShader.Parameters["NormalMap"].SetValue(this.mTextures[enumTextureType.MAP_BUMP]);
                                mShader.CurrentTechnique = mShader.Techniques["Textured"];
                                break;
                            case enumTextureType.MAP_SPECULAR:
                                mShader.Parameters["NormalMap"].SetValue(this.mTextures[enumTextureType.MAP_NORMAL]);
                                mShader.Parameters["SpecularMap"].SetValue(this.mTextures[enumTextureType.MAP_SPECULAR]);
                                break;
                        }
                    }
                }
                else
                {
                    foreach (BasicEffect localShader in localMesh.Effects)
                    {
                        localShader.EnableDefaultLighting();

                        localShader.TextureEnabled = true;
                        localShader.Texture = this.mTextures[enumTextureType.MAP_COLOUR];
                        localShader.World = localWorld;    //Tell the shader what World Matrix to use to draw the object.
                        localShader.View = _view;             //Tell the shader where to draw from (the camera).
                        localShader.Projection = _projection; //Tell the shader how to draw (like the camera lens... sort of).
                    }
                }

                localMesh.Draw();
            }
        }
    }
}
