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
    class CameraChase : BaseCamera
    {
        public CameraChase(GraphicsDevice _graphics, InputManager _input)
            : base(_graphics, _input)
        {
            this.mCameraType = Core.CameraType.Chase;
            this.ResetCamera();
        }

        public override void Update(TimeSpan _elapsedTime, Matrix _chaseWorld)
        {
            float timeDiff = (float)_elapsedTime.TotalSeconds;

            this.mRot.Forward.Normalize();
            _chaseWorld.Right.Normalize();
            _chaseWorld.Up.Normalize();

            this.mRot = Matrix.CreateFromAxisAngle(this.mRot.Forward, this.mRoll);

            this.mTargetWho = new Vector3D(_chaseWorld.Translation.X, _chaseWorld.Translation.Y, _chaseWorld.Translation.Z);
            this.mTarget = this.mTargetWho;
            this.mTarget += (new Vector3D(_chaseWorld.Right.X, _chaseWorld.Right.Y, _chaseWorld.Right.Z)) * this.mYaw;
            this.mTarget += (new Vector3D(_chaseWorld.Up.X, _chaseWorld.Up.Y, _chaseWorld.Up.Z)) * this.mPitch;

            Vector3 newTargetPosTransform = Vector3.Transform(this.mTargetOffset, _chaseWorld);
            this.mTargetPos = new Vector3D(newTargetPosTransform.X, newTargetPosTransform.Y, newTargetPosTransform.Z);

            Vector3 newPosSmoothstep = Vector3.SmoothStep(this.mPos, this.mTargetPos, (10f * timeDiff));
            this.mPos = new Vector3D(newPosSmoothstep.X, newPosSmoothstep.Y, newPosSmoothstep.Z);

            this.mYaw = MathHelper.SmoothStep(this.mYaw, 0f, .1f);
            this.mPitch = MathHelper.SmoothStep(this.mPitch, 0f, .1f);
            this.mRoll = MathHelper.SmoothStep(this.mRoll, 0f, .2f);

            this.mViewMat = Matrix.CreateLookAt(this.mPos, this.mTarget, this.mRot.Up); //Update View Matrix every frame.
        }
    }
}
