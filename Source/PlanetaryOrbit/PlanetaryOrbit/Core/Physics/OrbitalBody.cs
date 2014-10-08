using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POSystem;

using PlanetaryOrbit.Core.Models;
using PlanetaryOrbit.Core.Physics;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PlanetaryOrbit.Core.Physics
{
    public enum Eccentricity
    {
        Circular = 0,
        Eliptic,
        Parabolic,
        Hyperbolic
    }

    public struct Event
    {
        public Vector3D q;
        public double t;
    }

    public enum BodyType
    {
        Star = 0,
        Planet,
        Moon,
        Comet,
        Dwarf,
        Minor,
        TransNeptunian,
        Plutoids,
        NaturalSatellite,
        ManMadeSatellite
    }

    public class OrbitalBody
    {
        public static OrbitalBody None = null; //Default null planet type

        public GraphicsDevice _obj_graphics;
        private OrbitalBodyManager pManager;

        private SpriteFont mFont;
        private Vector2 mTextPos, mTextCenter;
        private Vector3 mTextOffset;

        //Trail
        private BasicEffect mTrailEffect;
        private List<VertexPositionColor> mTrailPositions;
        private Color mTrailColour;
        private VertexPositionColor[] mVertexPosColourArr;
        public List<Event> mPredictedTrajectory = new List<Event>();

        public bool mIsRotate = true;

        public PlanetRing mRing;

        public AdvancedModel mModel;
        public string mObjectName;
        public OrbitalBody mParentObject;
        public BodyType mBodyType;
        public float mScale;
        public float mDistanceScale = 1f;

        public string dataInfo;
        public State sCurrent, sLast, sRHS, sTmp;
        public Vector3D qError, vError, qErrorTrajectory, vErrorTrajectory;
        public double mMU, mDistance, mDiameter, mMass, mTrueMass, mTrueMU;
        public double mSemiMajorAxis, mEccentricity, mInclination, mArgPeriapsis, mLongOfAscendNode, mMeanAnomAtEpoch;

        //Data we cant calulate (YET!!! Hopefully)
        public double dataSiderealDay, dataNutation, dataPrecession; //Not using nutation or precession at the moment

        //Data
        public double dataVelocityRadial, dataDistance, dataVelocity, dataAngularMomentum, dataInclination, dataLongOfAscendNode, dataEccentricity, dataArgPeriapsis, dataTheta, dataMeanMotion, dataApsis, dataPeriapsis, dataSemiMajorAxis, dataPeriod, dataNode, dataRotationalVelocity, dataObliquity;
        public Vector3D dataAngularMomentumVector, dataMeanMotionVector, dataEccentricityVector, dataNodeVector;

        #region "Star Properties"
        bool IsStar = false;
        bool mOcclusionWait;
        OcclusionQuery mOcclusionQuery;
        bool mOcclusionQueryActive = false;
        float mOcclusionAlpha;

        public StarType mStarType;
        public Vector3 mStarColour;
        Texture2D mGlowSprite;
        public float mGlowSize = 400;

        public enum StarType
        {
            M,
            K,
            G,
            F,
            A,
            B,
            O,
            Undefined,
        };

        SolarFlare[] mSolarFlares =
        {
            new SolarFlare(-0.5f, 0.7f, new Color( 50,  25,  50), "flare1"),
            new SolarFlare( 0.3f, 0.4f, new Color(100, 255, 200), "flare1"),
            new SolarFlare( 1.2f, 1.0f, new Color(100,  50,  50), "flare1"),
            new SolarFlare( 1.5f, 1.5f, new Color( 50, 100,  50), "flare1"),

            new SolarFlare(-0.3f, 0.7f, new Color(200,  50,  50), "flare2"),
            new SolarFlare( 0.6f, 0.9f, new Color( 50, 100,  50), "flare2"),
            new SolarFlare( 0.7f, 0.4f, new Color( 50, 200, 200), "flare2"),

            new SolarFlare(-0.7f, 0.7f, new Color( 50, 100,  25), "flare3"),
            new SolarFlare( 0.0f, 0.6f, new Color( 25,  25,  25), "flare3"),
            new SolarFlare( 2.0f, 1.4f, new Color( 25,  50, 100), "flare3"),
        };

        private void SetStarType(StarType _type)
        {
            this.mStarType = _type;
            switch (this.mStarType)
            {
                case StarType.A:
                    this.mStarColour = new Vector3(0.5f, 1, 0.5f);
                    break;
                case StarType.B:
                    this.mStarColour = new Vector3(0.8f, 0.8f, 1.0f);
                    break;
                case StarType.F:
                    this.mStarColour = new Vector3(1, 1, 1);
                    break;
                case StarType.G: //Sun
                    this.mStarColour = new Vector3(1, 1, 0.3f);
                    break;
                case StarType.K:
                    this.mStarColour = new Vector3(1, 0.8f, 0.3f);
                    break;
                case StarType.M:
                    this.mStarColour = new Vector3(1, 0.3f, 0.3f);
                    break;
                case StarType.O:
                    this.mStarColour = new Vector3(0.3f, 0.3f, 1);
                    break;
            }

            this.mGlowSize = 30 * this.mScale;
        }


        public void SetObjectAsStar(StarType _type, GraphicsDevice _graphics, ContentManager _content)
        {
            SetStarType(_type);

            this.mOcclusionQuery = new OcclusionQuery(_graphics);
            this.mGlowSprite = _content.Load<Texture2D>("Core/Textures/Extra/Star/glow");

            foreach (SolarFlare localFlare in this.mSolarFlares)
            {
                localFlare.mTexture = _content.Load<Texture2D>("Core/Textures/Extra/Flares/" + localFlare.mTextureName);
            }

            this.mModel.AmbientColor = new Color(this.mStarColour);
            this.IsStar = true;
        }

        public Vector2 TransformPosition(BaseCamera _camera, Vector3 _pos)
        {
            Vector4 TransPos = Vector4.Transform(_pos, _camera.View * _camera.Projection);

            if (TransPos.W != 0)
            {
                TransPos.X = TransPos.X / TransPos.W;
                TransPos.Y = TransPos.Y / TransPos.W;
                TransPos.Z = TransPos.Z / TransPos.W;
            }

            Vector2 Pos2D = new Vector2(
                TransPos.X * this._obj_graphics.PresentationParameters.BackBufferWidth / 2 + this._obj_graphics.PresentationParameters.BackBufferWidth / 2,
                -TransPos.Y * this._obj_graphics.PresentationParameters.BackBufferHeight / 2 + this._obj_graphics.PresentationParameters.BackBufferHeight / 2);

            return Pos2D;
        }

        public void DrawFlares(Vector2 _lightPos, SpriteBatch _sb)
        {
            Viewport vp = this._obj_graphics.Viewport;

            Vector2 screenCenter = new Vector2(vp.Width, vp.Height) / 2;
            Vector2 flareVec = screenCenter - _lightPos;

            _sb.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            foreach (SolarFlare localFlare in this.mSolarFlares)
            {
                Vector2 flarePos = _lightPos + flareVec * localFlare.mPosition;
                Vector4 flareColor = localFlare.mColor.ToVector4();
                flareColor.W *= this.mOcclusionAlpha;

                Vector2 flareOrigin = new Vector2(localFlare.mTexture.Width, localFlare.mTexture.Height) / 2;
                _sb.Draw(localFlare.mTexture, flarePos, null, new Color(flareColor), 1, flareOrigin, localFlare.mScale, SpriteEffects.None, 0);
            }

            _sb.End();
        }

        public void DrawGlow(Vector2 _lightPos, SpriteBatch _sb)
        {
            Color GlowColor = new Color(1, 1, 1, this.mOcclusionAlpha);
            Vector2 GlowOrigin = new Vector2(this.mGlowSprite.Width, this.mGlowSprite.Height) / 2;
            float GlowScale = this.mGlowSize * 2 / this.mGlowSprite.Width;

            _sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _sb.Draw(this.mGlowSprite, _lightPos, null, GlowColor, 0, GlowOrigin, GlowScale, SpriteEffects.None, 0);
            _sb.End();
        }
        #endregion

        public bool IsMassless
        {
            get { return this.mMU == 0; }
        }

        public OrbitalBody(string _objectName, ref OrbitalBodyManager _bodyManager, Model _model, ref OrbitalBody _parentObject, double _diameter, double _mass)
        {
            this.pManager = _bodyManager;
            this._obj_graphics = this.pManager.GraphicsDevice;
            this.mObjectName = _objectName;

            this.mDiameter = _diameter;
            this.mMass = _mass;
            this.mTrueMass = _mass;
            this.mMU = GameSettings.PHYSICS_GRAVITY_CONSTANT * this.mMass;
            this.mTrueMU = GameSettings.PHYSICS_GRAVITY_CONSTANT * this.mTrueMass;
            this.mScale = (float)(this.mDiameter / this.pManager.mMasterDiameter) * this.pManager.mMasterScale;

            if (_model != null)
                this.mModel = new AdvancedModel(_model, new Vector3(0, 0, 0), Vector3.Zero, new Vector3(this.mScale));

            if (_parentObject != null)
            {
                this.mParentObject = _parentObject;
                Vector3D _lightDir = this.mParentObject.sCurrent.position - this.sCurrent.position;

                if (this.mModel != null)
                    this.mModel.LightDirection = new Vector3((float)_lightDir.X, (float)_lightDir.Y, (float)_lightDir.Z);

                this.mTrailEffect = new BasicEffect(this._obj_graphics);
                this.mTrailEffect.VertexColorEnabled = true;
                this.mTrailEffect.CurrentTechnique.Passes[0].Apply();
                this.mTrailColour = Color.White;
            }
            else
            {
                if (this.mModel != null)
                    this.mModel.LightDirection = Vector3.Zero;
            }

            this.mFont = this.pManager.DefaultFont;

            if (this.mModel != null)
                this.mTextOffset = new Vector3(0f, this.mModel.CurrentBounds.Radius + 1f, 0f);

            this.mTextPos = new Vector2();

            this.mTrailPositions = new List<VertexPositionColor>();
        }

        public void Update(TimeSpan _elapsedTime, TimeSpan _totalTime, float _dT, int _speed, TimeSpan _timePassed)
        {
            if (this.mParentObject != null)
            {
                if (this.mModel != null)
                    this.mModel.LightDirection = (Vector3)this.pManager.mRootBody.sCurrent.position - (Vector3)this.sCurrent.position;

                //Data Calculations
                double trueVelocity = 0;
                Vector3D trueVelocityVector = Vector3D.Zero;
                Vector3D truePosition = this.sCurrent.position * this.pManager.mShrinkScale;

                Vector3D trueDistance = truePosition - (this.mParentObject.sCurrent.position * this.pManager.mShrinkScale);
                this.dataDistance = (trueDistance.Length()) / this.mDistanceScale;
                trueVelocity = OrbitalMechanics.CalculateVelocity(this.dataDistance, this.mParentObject.mTrueMass) / 1000; //Get Velocity to KM

                if (this.mParentObject != this.pManager.mRootBody)
                {
                    truePosition = (truePosition - ((this.mParentObject.sCurrent.position - this.mParentObject.mParentObject.sCurrent.position) * this.pManager.mShrinkScale)) / this.mDistanceScale;
                }

                double fixVelocity = trueVelocity / this.sCurrent.velocity.Length();
                trueVelocityVector = this.sCurrent.velocity * fixVelocity;
                this.dataVelocity = trueVelocityVector.Length();
                this.dataVelocityRadial = Vector3D.Dot(truePosition, trueVelocityVector) / this.dataDistance;

                this.dataAngularMomentumVector = Vector3D.Cross(truePosition, trueVelocityVector);
                this.dataAngularMomentum = this.dataAngularMomentumVector.Length();
                this.dataInclination = -Math.Cos(this.dataAngularMomentumVector.Z / this.dataAngularMomentum);
                this.dataInclination = MathHelper.ToDegrees((float)this.dataInclination);

                this.dataNodeVector = Vector3D.Cross(new Vector3D(0, 0, 1), this.dataAngularMomentumVector);
                this.dataNode = this.dataNodeVector.Length();

                if (this.dataNode != 0)
                {
                    this.dataLongOfAscendNode = -Math.Cos(this.dataNodeVector.X / this.dataNode);

                    if (this.dataNodeVector.Z < 0)
                        this.dataLongOfAscendNode = 2 * Math.PI - this.dataLongOfAscendNode;
                }
                else
                    this.dataLongOfAscendNode = 0;

                this.dataLongOfAscendNode = MathHelper.ToDegrees((float)this.dataLongOfAscendNode);
                this.dataEccentricityVector = 1 / (this.mParentObject.mTrueMU / 1000000000) * ((Math.Pow(this.dataVelocity, 2) - (this.mParentObject.mTrueMU / 1000000000) / this.dataDistance) * truePosition - this.dataDistance * this.dataVelocityRadial * trueVelocityVector);
                this.dataEccentricity = this.dataEccentricityVector.Length();

                double julianDays = this.pManager.ToJulianDate(this.pManager.mTime);
                this.dataObliquity = this.pManager.CalculateEpsilonMean(julianDays);

                if (this.dataNode != 0)
                {
                    if (this.dataEccentricity > 1.0E-15)
                    {
                        this.dataArgPeriapsis = -Math.Cos(Vector3D.Dot(this.dataNodeVector, this.dataEccentricityVector) / (this.dataNode * this.dataEccentricity));

                        if (this.dataEccentricityVector.Z < 0)
                            this.dataArgPeriapsis = 2 * Math.PI - this.dataArgPeriapsis;
                    }
                    else
                        this.dataArgPeriapsis = 0;
                }
                else
                    this.dataArgPeriapsis = 0;

                this.dataArgPeriapsis = MathHelper.ToDegrees((float)this.dataArgPeriapsis);

                if (this.dataEccentricity > 1.0E-10)
                {
                    this.dataTheta = -Math.Cos((Vector3D.Dot(this.dataEccentricityVector, truePosition) / this.dataEccentricity / this.dataDistance));

                    if (this.dataVelocityRadial < 0)
                        this.dataTheta = 2 * Math.PI - this.dataTheta;
                }
                else
                {
                    Vector3D tmpCross = Vector3D.Cross(this.dataNodeVector, truePosition);

                    if (tmpCross.Z >= 0)
                        this.dataTheta = -Math.Cos((Vector3D.Dot(this.dataNodeVector, truePosition) / this.dataNode / this.dataDistance));
                    else
                        this.dataTheta = 2 * Math.PI - -Math.Cos((Vector3D.Dot(this.dataNodeVector, truePosition) / this.dataNode / this.dataDistance));
                }

                double apsisP1 = Math.Pow(this.dataAngularMomentum, 2) / (this.mParentObject.mTrueMU / 1000000000);
                this.dataPeriapsis = apsisP1 * (1 / (1 + (this.dataEccentricity * Math.Cos(0))));
                this.dataApsis = apsisP1 * (1 / (1 + (this.dataEccentricity * Math.Cos(180))));
                this.dataSemiMajorAxis = Math.Pow(this.dataAngularMomentum, 2) / (this.mParentObject.mTrueMU / 1000000000) / (1 - Math.Pow(this.dataEccentricity, 2));
                this.dataPeriod = ((Math.PI * 2) / Math.Sqrt((this.mParentObject.mTrueMU / 1000000000))) * Math.Pow(this.dataSemiMajorAxis, 1.5); //Seconds
                //--Data Calculations

                if (this.pManager._SIM_MODE && this.mObjectName.Trim().ToLower() == this.pManager._SIM_MODE_PLANET)
                {
                    string data = string.Format("date,{0},{1},{2},{3},{4},{5},{6}", this.dataSemiMajorAxis.ToString(), this.dataEccentricity.ToString(), this.dataInclination.ToString(), this.dataArgPeriapsis.ToString(), this.dataPeriod.ToString(), this.dataPeriapsis.ToString(), this.dataApsis.ToString());
                    DataLogger.Output(data);
                }
            }

            if (!this.pManager._SIM_MODE)
            {
                if (this.mBodyType != BodyType.ManMadeSatellite)
                {
                    this.dataRotationalVelocity = (this.mDiameter * Math.PI) / (this.dataSiderealDay * 24);
                }

                if (this.mModel != null)
                    this.mModel.Position = this.sCurrent.position;

                float rotAddd = (MathHelper.ToRadians(360) * (float)(this.dataRotationalVelocity / (this.mDiameter * Math.PI))) * _speed;

                if (this.mModel != null)
                {
                    this.mModel.Rotation += new Vector3(0f, rotAddd, 0f) * (float)_elapsedTime.TotalSeconds; //dT or Total Seconds?
                    this.mModel.Rotation = new Vector3(this.mModel.Rotation.X, this.mModel.Rotation.Y, MathHelper.ToRadians((float)this.dataObliquity));
                }

                if (this.mRing != null && this.mModel != null)
                {
                    this.mRing.Rotation = this.mModel.Rotation;
                    this.mRing.Position = this.mModel.Position;
                    this.mRing.LightDirection = this.mModel.LightDirection;
                    this.mRing.PlanetRadiusSquared = (this.mModel.CurrentBounds.Radius * this.mModel.CurrentBounds.Radius) * 100f;
                    this.mRing.Update(_elapsedTime);
                }

                if (this.mModel != null)
                    this.mModel.UpdateModel(_timePassed); //Other Updates

                if (this.mTrailPositions.Count == 0)
                {
                    this.mTrailPositions.Add(new VertexPositionColor(this.sCurrent.position, this.mTrailColour));
                    this.mVertexPosColourArr = this.mTrailPositions.ToArray();
                }
                else
                {
                    if (Vector3.Distance(this.sCurrent.position, this.mTrailPositions[this.mTrailPositions.Count - 1].Position) >= this.pManager.mTrailSpacing)
                    {
                        if (this.mTrailPositions.Count > this.pManager.mMaxTrailDistance)
                            this.mTrailPositions.Remove(this.mTrailPositions.First());

                        this.mTrailPositions.Add(new VertexPositionColor(this.sCurrent.position, this.mTrailColour));
                        this.mVertexPosColourArr = this.mTrailPositions.ToArray();
                    }
                }
            }
        }

        public void Draw(SpriteBatch _sb, BaseCamera _camera, bool _drawShaders, bool _drawInfo)
        {
            if (this.IsStar)
            {
                if (_camera.CameraType == CameraType.Top)
                {
                    float _distanceToStar = Vector3.Distance(_camera.Position, this.sCurrent.position);
                    this.mGlowSize = (float)(this.pManager.mMaxLight - (5000f * (_distanceToStar / this.pManager.mMaxLightDistance)));
                }
                else
                {
                    float _distanceToStar = Vector3.Distance(_camera.Position, this.sCurrent.position);
                    this.mGlowSize = (float)(this.pManager.mMaxLight - (this.pManager.mMaxLight * (_distanceToStar / this.pManager.mMaxLightDistance)));
                }
            }

            if (_drawInfo) //Show Planet Information and previous positions
            {
                if (this.mParentObject != null)
                {
                    this.mTrailEffect.View = _camera.View;
                    this.mTrailEffect.Projection = _camera.Projection;
                    int vertexCount = this.mTrailPositions.Count;

                    if (vertexCount > 0)
                    {
                        this.mTrailEffect.CurrentTechnique.Passes[0].Apply();

                        int lineCount = vertexCount - 1;
                        int vertexOffset = 0;
                        if (vertexCount > 2)
                        {
                            try
                            {
                                while (lineCount > 0)
                                {
                                    this._obj_graphics.DrawUserPrimitives(PrimitiveType.LineStrip, this.mVertexPosColourArr, vertexOffset, lineCount);
                                    vertexCount += lineCount * 2;
                                    lineCount -= lineCount;
                                }
                            }
                            catch (Exception ex)
                            {
                                string msg = string.Format("Error ({0}|VC:{1}|LC:{2}|VO:{3}) {4}", this.mObjectName, this.mVertexPosColourArr.Length, lineCount, vertexOffset, ex.Message);
                                throw new Exception(msg);
                            }
                        }
                    }
                }

                _sb.Begin();
                this.pManager.internReset2D();
                string tmpInfo = String.Format("{0}", this.mObjectName);
                this.mTextCenter = this.mFont.MeasureString(tmpInfo) * 0.5f;

                this.mTextOffset = new Vector3(0, this.mModel.CurrentBounds.Radius + (5 * this.mScale), 0);
                Vector3 screenSpace = this._obj_graphics.Viewport.Project(Vector3.Zero, _camera.Projection, _camera.View, Matrix.CreateTranslation(this.sCurrent.position + this.mTextOffset));
                this.mTextPos.X = screenSpace.X;
                this.mTextPos.Y = screenSpace.Y;

                this.mTextPos.X = (int)(this.mTextPos.X - this.mTextCenter.X);
                this.mTextPos.Y = (int)(this.mTextPos.Y - this.mTextCenter.Y);

                if (screenSpace.Z < 1.0f)
                    _sb.DrawString(this.mFont, tmpInfo, this.mTextPos, Color.White);

                _sb.End();
            }

            this.pManager.internReset3D();

            if (this.IsStar) //If its a star do an occlusion query
            {
                if (this.mOcclusionQueryActive)
                {
                    if (this.mOcclusionQuery.IsComplete)
                    {
                        const float queryArea = 16 * 16;
                        if (this.mOcclusionQuery.PixelCount > 0)
                        {
                            this.mOcclusionAlpha = Math.Min(this.mOcclusionQuery.PixelCount / queryArea, 1);
                        }
                        else
                        {
                            this.mOcclusionAlpha = 0;
                        }

                        this.mOcclusionQuery.Begin();
                        this.mOcclusionWait = false;
                    }
                    else
                    {
                        this.mOcclusionWait = true;
                    }
                }
                else
                {
                    this.mOcclusionQuery.Begin();
                    this.mOcclusionQueryActive = true;
                    this.mOcclusionWait = false;
                }
            }

            if (this.mModel != null) //If model exists, draw it.
                this.mModel.DrawModel(_camera, _drawShaders);

if (this.IsStar) //Continue with occlusion query
{
    if (this.mOcclusionQueryActive)
        if (!this.mOcclusionWait)
            this.mOcclusionQuery.End();

    if (this.mOcclusionAlpha > 0)
    {
        this.DrawGlow(this.TransformPosition(_camera, this.mModel.Position), _sb);
        this.DrawFlares(this.TransformPosition(_camera, this.mModel.Position), _sb);
    }
}

            if (this.mRing != null)
                this.mRing.Draw(_camera, _drawShaders);
        }

        //PHYSICS

        #region "Symplectic method"

        public void SYM_kick_noforce(double _step)
        {
            if (this.mParentObject != null)
                this.sCurrent.velocity += this.sCurrent.force * _step;
        }

        public void SYM_drift(double _step)
        {
            if (this.mParentObject != null)
                this.sCurrent.position += this.sCurrent.velocity * _step;
        }

        public void SYM_kick(ref List<OrbitalBody> _list_planets, double dT)
        {
            this.sCurrent.force = Vector3D.Zero;

            if (this.mParentObject != null)
            {
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    if (this != _list_planets[i])
                    {
                        Vector3D pDistance = this.sCurrent.position - _list_planets[i].sLast.position;
                        double pDistanceSquare = (pDistance.X * pDistance.X + pDistance.Y * pDistance.Y + pDistance.Z * pDistance.Z);
                        double pDistanceTotal = Math.Sqrt(pDistanceSquare);
                        double pForce = GameSettings.PHYSICS_GRAVITY_CONSTANT * (this.mMass * _list_planets[i].mMass) / pDistanceSquare;

                        double xPart = pDistance.X / 2;
                        double newDistance = this.sCurrent.position.X + xPart;

                        if (((newDistance - _list_planets[i].sLast.position.X) * (newDistance - _list_planets[i].sLast.position.X)) < (pDistance.X * pDistance.X))
                            this.sCurrent.force.X += pForce * pDistance.X / dT;
                        else
                            this.sCurrent.force.X -= pForce * pDistance.X / dT;

                        double yPart = pDistance.Y / 2;
                        newDistance = this.sCurrent.position.Y + yPart;

                        if (((newDistance - _list_planets[i].sLast.position.Y) * (newDistance - _list_planets[i].sLast.position.Y)) < (pDistance.Y * pDistance.Y))
                            this.sCurrent.force.Y += pForce * pDistance.Y / dT;
                        else
                            this.sCurrent.force.Y -= pForce * pDistance.Y / dT;

                        double zPart = pDistance.Z / 2;
                        newDistance = this.sCurrent.position.Z + zPart;

                        if (((newDistance - _list_planets[i].sLast.position.Z) * (newDistance - _list_planets[i].sLast.position.Z)) < (pDistance.Z * pDistance.Z))
                            this.sCurrent.force.Z += pForce * pDistance.Z / dT;
                        else
                            this.sCurrent.force.Z -= pForce * pDistance.Z / dT;
                    }
                }

                this.SYM_accountForInertia();
                this.sCurrent.velocity += this.sCurrent.force * dT;
            }
        }

        public void SYM_accountForInertia()
        {
            if (this.mParentObject != null)
                this.sCurrent.force = this.sCurrent.force / this.mMass;
        }
        #endregion
    }
}
