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
    public enum CameraType
    {
        Free = 0,
        Chase,
        Orbit,
        Top
    }

    public enum CameraSpeed
    {
        Slow = 0,
        Normal,
        Fast,
        SuperFast
    }

    public abstract class BaseCamera
    {
        //Objects
        protected GraphicsDevice _obj_graphics;
        protected InputManager _obj_input;

        //General Settings
        protected Vector3D mPos;
        protected Vector3D mTarget;
        protected Matrix mViewMat;
        protected Matrix mProjMat;
        protected CameraSpeed mSpeedPreset;
        protected CameraType mCameraType;

        //Camera Settings
        protected float mYaw;
        protected float mPitch;
        protected float mRoll;
        protected float mSpeed;
        protected float mRotSpeed;
        protected Vector3D mDir;
        protected Matrix mRot;

        //Chase/Orbit Camera
        protected Vector3D mTargetOffset;
        protected Vector3D mTargetPos;
        protected Vector3D mTargetWho;

        #region "Properties"
        public CameraSpeed CurrentSpeedPreset
        {
            get { return this.mSpeedPreset; }
            set { this.mSpeedPreset = value; }
        }

        public Matrix View
        {
            get { return this.mViewMat; }
        }

        public Matrix Projection
        {
            get { return this.mProjMat; }
        }

        public Vector3D Position
        {
            get { return this.mPos; }
            set { this.mPos = value; }
        }

        public CameraType CameraType
        {
            get { return this.mCameraType; }
        }

        public float Yaw
        {
            get { return this.mYaw; }
            set { this.mYaw = value; }
        }

        public Vector3D Direction
        {
            get
            {
                Vector3D.Subtract(ref this.mTarget, ref this.mPos, out mDir);
                return mDir;
            }
        }

        public float Pitch
        {
            get { return this.mPitch; }
            set { this.mPitch = value; }
        }

        public float Roll
        {
            get { return this.mRoll; }
            set { this.mRoll = value; }
        }

        public float Speed
        {
            get { return this.mSpeed; }
            set { this.mSpeed = value; }
        }

        public Matrix Rotation
        {
            get { return this.mRot; }
            set { this.mRot = value; }
        }

        public Vector3D CameraOffset
        {
            get { return this.mTargetOffset; }
            set { this.mTargetOffset = value; }
        }
        #endregion

        public BaseCamera(GraphicsDevice _graphics, InputManager _input)
        {
            this._obj_graphics = _graphics;
            this._obj_input = _input;
        }

        public void SetCameraSpeed(CameraSpeed _speed)
        {
            this.mSpeedPreset = _speed;

            switch (_speed)
            {
                case CameraSpeed.Slow:
                    this.mSpeed = 0.01f;
                    break;
                case CameraSpeed.Normal:
                    this.mSpeed = 1.0f;
                    break;
                case CameraSpeed.Fast:
                    this.mSpeed = 2.5f;
                    break;
                case CameraSpeed.SuperFast:
                    this.mSpeed = 25.0f;
                    break;
            }
        }

        public void SetPosition(Vector3D _newPos)
        {
            this.mPos = _newPos;
        }

        public void ResetCamera()
        {
            this.mPos = new Vector3D(0, 0, 250);
            this.mTarget = Vector3D.Zero;
            this.mViewMat = Matrix.Identity;
            this.mProjMat = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), this._obj_graphics.Viewport.AspectRatio, .1f, 100000000f);
            this.SetCameraSpeed(CameraSpeed.Normal);

            this.mYaw = 0.0f;
            this.mPitch = 0.0f;
            this.mRoll = 0.0f;
            this.mRotSpeed = 0.5f;
            this.mRot = Matrix.Identity;

            //Custom
            this.mTargetPos = this.mPos;
            this.mTargetWho = this.mTarget;
            this.mTargetOffset = new Vector3D(0, 0, 25);
        }

        public void MoveCamera(Vector3 _moveVector)
        {
            this.mPos += this.mSpeed * (new Vector3D(_moveVector.X, _moveVector.Y, _moveVector.Z));
        }

        public void RotateCamera(Vector3 _rotateVector)
        {
            this.mYaw += _rotateVector.Y;
            this.mPitch -= _rotateVector.X;
            this.mPitch = MathHelper.Clamp(this.mPitch,MathHelper.ToRadians(-5000),MathHelper.ToRadians(5000));
        }

        public void HandleInput(CameraType _currentMode, PlayerIndex? _controllingPlayer)
        {
            if (this._obj_input.IsPressed("CAMERA_RESET", _controllingPlayer))
                this.ResetCamera();

            switch (_currentMode)
            {
                case CameraType.Chase:
                    if (this._obj_input.IsPressed("CAMERA_FORWARD", _controllingPlayer))
                        this.MoveCamera(this.Rotation.Forward);

                    if (this._obj_input.IsPressed("CAMERA_BACK", _controllingPlayer))
                        this.MoveCamera(-this.Rotation.Forward);

                    if (this._obj_input.IsPressed("CAMERA_LEFT", _controllingPlayer))
                        this.MoveCamera(-this.Rotation.Right);

                    if (this._obj_input.IsPressed("CAMERA_RIGHT", _controllingPlayer))
                        this.MoveCamera(this.Rotation.Right);
                    break;
                case CameraType.Orbit:
                    if (this._obj_input.IsPressed("CAMERA_FORWARD", _controllingPlayer))
                        this.RotateCamera(Vector3.UnitX * 1.0f);

                    if (this._obj_input.IsPressed("CAMERA_BACK", _controllingPlayer))
                        this.RotateCamera(Vector3.UnitX * -1.0f);

                    if (this._obj_input.IsPressed("CAMERA_LEFT", _controllingPlayer))
                        this.RotateCamera(Vector3.UnitY * -1.0f);

                    if (this._obj_input.IsPressed("CAMERA_RIGHT", _controllingPlayer))
                        this.RotateCamera(Vector3.UnitY * 1.0f);
                    break;
            }
        }

        public abstract void Update(TimeSpan _elapsedTime, Matrix _chaseWorld);
    }
}
