using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using POSystem;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlanetaryOrbit.Core.Screen;

namespace PlanetaryOrbit.Core.Physics
{
    public enum SimulationCheckType
    {
        SMA,
        PA,
        AP,
        OP,
        E
    }

    public class OrbitalBodyManager
    {
        protected GraphicsDevice _obj_graphics;
        protected SpriteFont _default_ui;
        protected List<OrbitalBody> _list_planets;

        public bool initDone = false;

        public OrbitalBody mRootBody;
        public float mMasterScale;
        public double mMasterDiameter;
        public float mShrinkScale;
        public DateTime mTime;
        public JulianCalendar mTimeJulian;

        #region "Simulation Data"
        public bool _SIM_MODE = true;
        public string _SIM_MODE_PLANET = "Mars";
        public bool _SIM_MODE_ACTIVE_LOGGING = false;
        public double _SIM_CHECK_START_SMA = 227931320.1;
        public double _SIM_CHECK_START_PA = 206668469.4;
        public double _SIM_CHECK_START_AP = 249194170.9;
        public double _SIM_CHECK_START_OP = 59351266.39;
        public bool _SIM_CHECK_START_OK = false;

        public double _SIM_CHECK_END_SMA = 227931320.1;
        public double _SIM_CHECK_END_PA = 206668469.4;
        public double _SIM_CHECK_END_AP = 249194170.9;
        public double _SIM_CHECK_END_OP = 59351266.39;
        public bool _SIM_CHECK_END_OK = false;
        #endregion

        #region "Star Properties"
        public double mMaxLightDistance;
        public float mMaxLight;
        #endregion

        public double mInitialEnergy;
        public State mInitialMomentum;

        private List<string> mInfoText;
        private float mInfoOffset;

        public int mMaxTrailDistance = 10000;
        public float mTrailSpacing = 2;
        protected int mSimSpeed = 0;

        private TimeSpan mTimePassed;

        #region "Properties"
        public GraphicsDevice GraphicsDevice
        {
            get { return this._obj_graphics; }
        }

        public SpriteFont DefaultFont
        {
            get { return this._default_ui; }
        }

        public float MasterScale
        {
            get { return this.mMasterScale; }
        }

        public double MasterDiameter
        {
            get { return this.mMasterDiameter; }
        }

        public double GravitantionalConstant
        {
            get { return GameSettings.PHYSICS_GRAVITY_CONSTANT; }
        }
        #endregion

        public OrbitalBodyManager(GraphicsDevice _graphics, SpriteFont _font, float _masterScale, double _masterDiameter, float _shrinkScale)
        {
            this._obj_graphics = _graphics;
            this._list_planets = new List<OrbitalBody>();
            this.mMasterScale = _masterScale;
            this.mMasterDiameter = _masterDiameter;
            this._default_ui = _font;
            this.mInfoText = new List<string>();
            this.mInfoOffset = 1f;
            this.mTime = new DateTime(2014, 1, 1);
            this.mTimeJulian = new JulianCalendar();
            this.mShrinkScale = _shrinkScale;
            this.mMaxLight = 500f;
        }

        public void SetRootBody(OrbitalBody _body) {this.mRootBody = _body;}
        public OrbitalBody GetRootBody() { return this.mRootBody; }
        public void Add(OrbitalBody _planet) {this._list_planets.Add(_planet);}
        public int Size() { return this._list_planets.Count; }
        public double ToJulianDate(DateTime _date) { return _date.ToOADate() + 2415018.5; }

        public OrbitalBody Get(int _id)
        {
            if (this._list_planets[_id] != null)
                return this._list_planets[_id];

            return null;
        }

        public bool SimulationParameterPreCheck(SimulationCheckType _type, double _actual)
        {
            bool result = false;
            double _val = 0;
            switch (_type)
            {
                case SimulationCheckType.SMA:
                    _val = this._SIM_CHECK_START_SMA;
                    if (_actual < (_val + 100) && _actual > (_val - 100)) { result = true; }
                    break;
                case SimulationCheckType.PA:
                    _val = this._SIM_CHECK_START_SMA;
                    if (_actual < (_val + 10) && _actual > (_val - 10)) { result = true; }
                    break;
                case SimulationCheckType.AP:
                    _val = this._SIM_CHECK_START_SMA;
                    if (_actual < (_val + 10) && _actual > (_val - 10)) { result = true; }
                    break;
                case SimulationCheckType.OP:
                    _val = this._SIM_CHECK_START_SMA;
                    if (_actual < (_val + 10) && _actual > (_val - 10)) { result = true; }
                    break;
                case SimulationCheckType.E:
                    _val = this._SIM_CHECK_START_SMA;
                    if (_actual < (_val + 10) && _actual > (_val - 10)) { result = true; }
                    break;
            }

            return result;
        }

        public Eccentricity DefineEccentricity(double e)
        {
            if (e == 0)
                return Eccentricity.Circular;
            else if (e > 0 && e < 1)
                return Eccentricity.Eliptic;
            else if (e == 1)
                return Eccentricity.Parabolic;
            else if (e > 1)
                return Eccentricity.Hyperbolic;

            return Eccentricity.Circular;
        }

        public double CalculateEpsilonMean(double _JD)
        {
            double t, p, w, EpsMeanDeg;

            t = (_JD - 2451545.0) / 3652500.0;
            w = 84381.448; p = t;
            w -= 4680.93 * p; p *= t;
            w -= 1.55 * p; p *= t;
            w += 1999.25 * p; p *= t;
            w -= 51.38 * p; p *= t;
            w -= 249.67 * p; p *= t;
            w -= 39.05 * p; p *= t;
            w += 7.12 * p; p *= t;
            w += 27.87 * p; p *= t;
            w += 5.79 * p; p *= t;
            w += 2.45 * p;
            EpsMeanDeg = w / 3600;

            return EpsMeanDeg;
        }

        public void internReset3D()
        {
            _obj_graphics.BlendState = BlendState.Opaque;
            _obj_graphics.DepthStencilState = DepthStencilState.Default;
            _obj_graphics.RasterizerState = RasterizerState.CullCounterClockwise;

            SamplerState tstate = new SamplerState();
            tstate.AddressU = TextureAddressMode.Wrap;
            tstate.AddressV = TextureAddressMode.Wrap;
            tstate.Filter = TextureFilter.Anisotropic;
            _obj_graphics.SamplerStates[0] = tstate;
        }

        public void internReset2D()
        {
            _obj_graphics.BlendState = BlendState.AlphaBlend;
            _obj_graphics.DepthStencilState = DepthStencilState.None;
            SamplerState tstate = new SamplerState();
            tstate.AddressU = TextureAddressMode.Clamp;
            tstate.AddressV = TextureAddressMode.Clamp;
            _obj_graphics.SamplerStates[0] = tstate;
        }

        public void Update(TimeSpan _elapsedTime, TimeSpan _totalTime, bool _pause, int _speed)
        {
            float elapsedTime = (float)_elapsedTime.TotalSeconds; //dT
            float dT = elapsedTime * 0.0005f; //dT multiplied by speed.

            if (this.mTimePassed == null)
                this.mTimePassed = _totalTime;

            if (!this.initDone) //Initialize Planet Locations and Initial Energy Calculations
            {
                if (this._SIM_MODE) //Initialize Simulation Data Logger
                {
                    DataLogger.init(this._SIM_MODE_PLANET + " dt,a,e,i,w,op,pa,ap");
                }
                OrbitalMechanics.StartRKF45(ref this._list_planets);
#if DEBUG
                this.mInitialMomentum = OrbitalMechanics.CalculateMomentum(ref this._list_planets);
                this.mInitialEnergy = OrbitalMechanics.CalculateEnergy(ref this._list_planets);
                this.mInfoText.Add(String.Format("Total Energy: {0}", ((OrbitalMechanics.CalculateEnergy(ref this._list_planets) - this.mInitialEnergy) / this.mInitialEnergy).ToString()));
                this.mInfoText.Add(String.Format("Total Momentum (Velocity): {0}", ((OrbitalMechanics.CalculateMomentum(ref this._list_planets).velocity - this.mInitialMomentum.velocity) / this.mInitialMomentum.velocity).Length().ToString()));
                this.mInfoText.Add(String.Format("Total Momentum (Direction): {0}", ((OrbitalMechanics.CalculateMomentum(ref this._list_planets).position - this.mInitialMomentum.position) / this.mInitialMomentum.position).Length().ToString()));
#endif
                this.mMaxLightDistance = Vector3.Distance(this.mRootBody.sCurrent.position, this._list_planets[this._list_planets.Count - 1].sCurrent.position) + 10000;
                this.initDone = true;
            }
#if DEBUG
            mInfoText[0] = (String.Format("Total Energy: {0}", ((OrbitalMechanics.CalculateEnergy(ref this._list_planets) - this.mInitialEnergy) / this.mInitialEnergy).ToString()));
            mInfoText[1] = (String.Format("Total Momentum (Velocity): {0}", ((OrbitalMechanics.CalculateMomentum(ref this._list_planets).velocity - this.mInitialMomentum.velocity) / this.mInitialMomentum.velocity).Length().ToString()));
            mInfoText[2] = (String.Format("Total Momentum (Direction): {0}", ((OrbitalMechanics.CalculateMomentum(ref this._list_planets).position - this.mInitialMomentum.position) / this.mInitialMomentum.position).Length().ToString()));
#endif

            if (!_pause)
            {
                this.mTimePassed += _elapsedTime;
                this.mSimSpeed = _speed;
                for (int i = 0; i < _speed; i++)
                {
                    this.mTime = this.mTime.Add(new TimeSpan(1, 0, 0, 0));
                    foreach (OrbitalBody localBody in this._list_planets)
                        localBody.sLast = localBody.sCurrent;

                    OrbitalMechanics.StepRKF45(ref this._list_planets, dT, 1e-8, 5e-14);
                    //OrbitalMechanics.StepSym(ref this._list_planets, dT); //Symplectic Method - NOT WORKING
                }

                foreach (OrbitalBody localBody in this._list_planets)
                    localBody.Update(_elapsedTime, _totalTime, dT, _speed, this.mTimePassed);

            }
        }

        public void Draw(SpriteBatch _sb, BaseCamera _camera, bool _drawShaders, bool _drawInfo)
        {
            if (!this._SIM_MODE)
            {
                foreach (OrbitalBody localBody in this._list_planets)
                    localBody.Draw(_sb, _camera, _drawShaders, _drawInfo);

                if (_drawInfo)
                {
                    _sb.Begin();

                    string strSimSpeed = string.Format("x{0}", this.mSimSpeed);
                    _sb.DrawString(this._default_ui, strSimSpeed, new Vector2(10, 10), Color.White, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);

                    //DEBUG INFO BELOW
#if DEBUG
                    for (int i = 0; i < this.mInfoText.Count; i++)
                    {
                        float newOffset = GraphicsDevice.Viewport.Height - this._default_ui.MeasureString(this.mInfoText[i]).Y * ((i + 1) * this.mInfoOffset);
                        Vector2 newPos = new Vector2(10, newOffset);
                        _sb.DrawString(this._default_ui, this.mInfoText[i], newPos, Color.White);
                    }
#endif
                    _sb.End();
                }
            }
            else
            {
                foreach (OrbitalBody localBody in this._list_planets)
                {
                    if (localBody.mObjectName.Trim().ToLower() == this._SIM_MODE_PLANET)
                    {
                        _sb.Begin();
                        string _sim_msg = String.Format("Semi-Major Axis: {0}\nEccentricity: {1}\nInclination: {2}\n Argument of Periapsis: {3}\nOrbital Period: {4}\nPeriapsis: {5}\n Apsis: {6}", localBody.dataSemiMajorAxis.ToString(), localBody.dataEccentricity.ToString(), localBody.dataInclination.ToString(), localBody.dataArgPeriapsis.ToString(), localBody.dataPeriod.ToString(), localBody.dataPeriapsis.ToString(), localBody.dataApsis.ToString());
                        Vector2 _sim_msg_origin = this._default_ui.MeasureString(_sim_msg) / 2f;
                        _sb.DrawString(this._default_ui, _sim_msg, new Vector2(this.GraphicsDevice.Viewport.Width / 2, (this.GraphicsDevice.Viewport.Height / 2) + 100), Color.White, 0f, _sim_msg_origin, 1f, SpriteEffects.None, 0f);
                        _sb.End();
                    }
                }
            }
        }
    }
}
