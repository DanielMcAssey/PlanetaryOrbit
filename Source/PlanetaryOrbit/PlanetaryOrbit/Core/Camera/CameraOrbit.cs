using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PlanetaryOrbit.Core.Screen;
using PlanetaryOrbit.Core.Physics;

namespace PlanetaryOrbit.Core
{
    class CameraOrbit : BaseCamera
    {
        public CameraOrbit(GraphicsDevice _graphics, InputManager _input)
            : base(_graphics, _input)
        {
            this.mCameraType = CameraType.Orbit;
            this.ResetCamera();
        }

        public override void Update(TimeSpan _elapsedTime, Matrix _chaseWorld)
        {
            float timeDiff = (float)_elapsedTime.TotalSeconds;

            this.mRot.Forward.Normalize();

            this.mRot = Matrix.CreateRotationX(this.mPitch * timeDiff) * Matrix.CreateRotationY(this.mYaw * timeDiff) * Matrix.CreateFromAxisAngle(this.mRot.Forward, this.mRoll);
            Vector3 newTransform = Vector3.Transform(this.mTargetOffset, this.mRot);
            this.mTargetPos = new Vector3D(newTransform.X, newTransform.Y, newTransform.Z);
            Vector3D newChaseWorldTranslation = new Vector3D(_chaseWorld.Translation.X, _chaseWorld.Translation.Y, _chaseWorld.Translation.Z);
            this.mTargetPos += newChaseWorldTranslation;
            this.mPos = this.mTargetPos;

            this.mTarget = newChaseWorldTranslation;

            this.mRoll = MathHelper.SmoothStep(this.mRoll, 0f, .2f);

            this.mViewMat = Matrix.CreateLookAt(this.mPos, this.mTarget, this.mRot.Up); //Update View Matrix every frame.
        }
    }
}
