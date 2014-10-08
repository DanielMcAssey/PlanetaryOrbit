using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlanetaryOrbit.Core
{
    class Skybox
    {
        private Model mModel;
        private Effect mEffect;
        private Texture2D[] mTextures;
        private Matrix[] mTransforms;

        public Skybox(Model _model, Effect _shader)
        {
            this.mEffect = _shader;
            this.mModel = _model;
            this.mTextures = new Texture2D[this.mModel.Meshes.Count];
            int i = 0;

            foreach (ModelMesh localMesh in this.mModel.Meshes)
                foreach (BasicEffect currentEffect in localMesh.Effects)
                    this.mTextures[i++] = currentEffect.Texture;

            foreach (ModelMesh localMesh in this.mModel.Meshes)
                foreach (ModelMeshPart localMeshPart in localMesh.MeshParts)
                    localMeshPart.Effect = mEffect.Clone();
        }

        public void Draw(BaseCamera _camera)
        {
            this.mTransforms = new Matrix[this.mModel.Bones.Count];
            this.mModel.CopyAbsoluteBoneTransformsTo(this.mTransforms);
            int i = 0;
            foreach (ModelMesh localMesh in this.mModel.Meshes)
            {
                foreach (Effect currentEffect in localMesh.Effects)
                {
                    Matrix worldMatrix = this.mTransforms[localMesh.ParentBone.Index] * Matrix.CreateTranslation(_camera.Position);
                    currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(_camera.View);
                    currentEffect.Parameters["xProjection"].SetValue(_camera.Projection);
                    currentEffect.Parameters["xTexture"].SetValue(this.mTextures[i++]);
                }
                localMesh.Draw();
            }
        }
    }
}
