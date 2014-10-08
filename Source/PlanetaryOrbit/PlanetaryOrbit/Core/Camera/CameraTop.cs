using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PlanetaryOrbit.Core.Screen;
using PlanetaryOrbit.Core.Physics;

namespace PlanetaryOrbit.Core
{
    public class CameraTop : BaseCamera
    {
        public CameraTop(GraphicsDevice _graphics, InputManager _input)
            : base(_graphics, _input)
        {
            this.mCameraType = Core.CameraType.Top;
            this.ResetCamera();
            this.Position = Vector3D.Zero;
        }

        public override void Update(TimeSpan _elapsedTime, Matrix _chaseWorld)
        {
            float timeDiff = (float)_elapsedTime.TotalSeconds;
            Vector3 newLookatPos = Vector3.Transform(Vector3.Zero, _chaseWorld);
            this.mViewMat = Matrix.CreateLookAt(this.mPos, newLookatPos, this.mRot.Up);
        }
    }
}
