using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PlanetaryOrbit.Core.Screen;
using PlanetaryOrbit.Core.Physics;

namespace PlanetaryOrbit.Core
{
    public class CameraFree : BaseCamera
    {

        public CameraFree(GraphicsDevice _graphics, InputManager _input)
            : base(_graphics, _input)
        {
            this.mCameraType = Core.CameraType.Free;
            this.ResetCamera();
        }

        public override void Update(TimeSpan _elapsedTime, Matrix _chaseWorld)
        {
            float timeDiff = (float)_elapsedTime.TotalSeconds;

            if (this._obj_input.CurrentMouseState != this._obj_input.OriginalMouseState)
            {
                float xDifference = this._obj_input.CurrentMouseState.X - this._obj_input.OriginalMouseState.X;
                float yDifference = this._obj_input.CurrentMouseState.Y - this._obj_input.OriginalMouseState.Y;
                this.mYaw -= this.mRotSpeed * xDifference * timeDiff;
                this.mPitch -= this.mRotSpeed * yDifference * timeDiff;

                Mouse.SetPosition(this._obj_graphics.Viewport.Width / 2, this._obj_graphics.Viewport.Height / 2);
            }

            this.mRot = Matrix.CreateFromYawPitchRoll(this.mYaw, this.mPitch, this.mRoll);

            Vector3D newRotForward = new Vector3D(this.mRot.Forward.X, this.mRot.Forward.Y, this.mRot.Forward.Z);
            this.mTarget = this.mPos + newRotForward;
            this.mViewMat = Matrix.CreateLookAt(this.mPos, this.mTarget, this.mRot.Up); //Update View Matrix every frame.
        }
    }
}
