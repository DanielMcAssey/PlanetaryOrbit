using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using POSystem;

using PlanetaryOrbit.Core;
using PlanetaryOrbit.Core.Models;
using PlanetaryOrbit.Core.Physics;

using Squid;
using SquidXNA;
using OculusRift.Oculus;

namespace PlanetaryOrbit.Core.Screen
{
    public class ScreenGame : AppScreen
    {
        Skybox _obj_skybox; //Skybox
        OrbitalBodyManager _obj_orbitalbodymanager;
        List<SoundEffect> _obj_sfx;
        BasicEffect _obj_defaultEffect;
        SoundEffectInstance _sfx_ambience;
        SoundEffectInstance _sfx_music;

        public ScreenGameDesktop _obj_gui;

        Dictionary<CameraType, BaseCamera> _obj_cameras;
        CameraType _active_camera;

        int _chase_active_planet;
        Matrix _chaseWorld;
        bool _sim_pause = false;
        int _sim_speed = 1;
        int _sim_max_speed = 20;

        bool mDrawInfo = false;

        public ScreenGame()
        {
            this._bgcolour = Color.Black;
            this._trans_on_time = TimeSpan.FromSeconds(2.0d);
        }

        public override void loadContent()
        {
            base.loadContent();

            this.BGColour = Color.Black;
            this._obj_defaultEffect = new BasicEffect(this.ScreenManager.GraphicsDevice);
            GuiHost.Renderer = new RendererXNA(this.ScreenManager.Game);
            this._obj_gui = new ScreenGameDesktop();

            this.ScreenManager.GameIsMouseVisible = true;

            //Initialize SFX
            this._obj_sfx = new List<SoundEffect>();
            this._obj_sfx.Add(this._local_content.Load<SoundEffect>("Core/Audio/Ambient/space01"));
            this._obj_sfx.Add(this._local_content.Load<SoundEffect>("Core/Audio/Music/music01"));
            this._sfx_ambience = this._obj_sfx[0].CreateInstance();
            this._sfx_ambience.IsLooped = true;
            this._sfx_ambience.Volume = 0.1f;
            this._sfx_music = this._obj_sfx[1].CreateInstance();

            //Initialise Cameras
            this._obj_cameras = new Dictionary<CameraType, BaseCamera>();
            this._obj_cameras.Add(CameraType.Free, new CameraFree(ScreenManager.GraphicsDevice, this.GlobalInput));
            this._obj_cameras.Add(CameraType.Chase, new CameraChase(ScreenManager.GraphicsDevice, this.GlobalInput));
            this._obj_cameras.Add(CameraType.Orbit, new CameraOrbit(ScreenManager.GraphicsDevice, this.GlobalInput));
            this._obj_cameras.Add(CameraType.Top, new CameraTop(ScreenManager.GraphicsDevice, this.GlobalInput));
            this._active_camera = CameraType.Chase;

            this._chase_active_planet = 0;
            this._chaseWorld = Matrix.Identity;
            this._obj_skybox = new Skybox(this._local_content.Load<Model>("Core/Skybox/model"), this._local_content.Load<Effect>("Core/Shaders/Special/globalEffects"));

            float mMasterScale = 100f;
            float mSmallScale = 10000f;

            double sunDiameter = 1391000; // Kilometers
            double mercuryDiameter = 4879;
            double venusDiameter = 12104;
            double earthDiameter = 12742;
            double moonDiameter = 3474.8;
            double marsDiameter = 6779;
            double jupiterDiameter = 139822;
            double saturnDiameter = 116464;
            double uranusDiameter = 50724;
            double neptuneDiameter = 49244;
            double plutoDiameter = 2360;

            double sunMass = 1988500E24; // Kilograms
            double mercuryMass = 3.302000e+023;
            double venusMass = 4.868500e+024;
            double earthMass = 5.9726E24;
            double moonMass = 7.349000e+022;
            double marsMass = 6.421900e+023;
            double jupiterMass = 1.898600e+027;
            double saturnMass = 5.684623e+026;
            double uranusMass = 8.683200e+025;
            double neptuneMass = 1.024700e+026;
            double plutoMass = 1.30900E22;

            //Set Simulation Settings
            if (this.ScreenManager.ConfigManager.CoreSettings.SIM_ENABLED)
            {
                mMasterScale = 1f;
                mSmallScale = 1f;
                _sim_speed = this.ScreenManager.ConfigManager.CoreSettings.SIM_SPEED;
                this.ScreenManager.Game.IsFixedTimeStep = false;
            }
            else if (this.ScreenManager.ConfigManager.CoreSettings.GAME_TRUESCALE)
            {
                mMasterScale = 1f;
                mSmallScale = 1f;
            }

            //Create the orbital body manager.
            this._obj_orbitalbodymanager = new OrbitalBodyManager(this.ScreenManager.GraphicsDevice, this._local_content.Load<SpriteFont>(GameSettings.ASSET_CONFIG_PLANET_FONT), mMasterScale, sunDiameter, mSmallScale);

            //Set Simulation Settings for the Orbital Body manager
            this._obj_orbitalbodymanager._SIM_MODE = this.ScreenManager.ConfigManager.CoreSettings.SIM_ENABLED;
            this._obj_orbitalbodymanager._SIM_MODE_PLANET = this.ScreenManager.ConfigManager.CoreSettings.SIM_PLANET.Trim().ToLower();

            //Create Primary Planets
            OrbitalBody entSun = new OrbitalBody("Sun", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref OrbitalBody.None, sunDiameter, sunMass);
            entSun.SetObjectAsStar(OrbitalBody.StarType.G, this.ScreenManager.GraphicsDevice, this._local_content);
            entSun.mModel.AddShader("Main", enumShaderType.SUN, this._local_content.Load<Effect>("Core/Shaders/Special/Star"));
            entSun.dataSiderealDay = 25.38; //Days
            entSun.dataObliquity = 7.25; //Degrees
            entSun.dataInfo = "The Sun is the star at the center of the Solar System. It is almost perfectly spherical and consists of hot plasma interwoven with magnetic fields.";
            entSun.dataInfo += "\n.\n.\nThe Sun formed about 4.6 billion years ago from the gravitational collapse of a region within a large molecular cloud. Most of the matter gathered in the center, while the rest flattened into an orbiting disk that would become the Solar System. The central mass became increasingly hot and dense, eventually initiating thermonuclear fusion in its core. It is thought that almost all stars form by this process. The Sun is a G-type main-sequence star (G2V) based on spectral class and it is informally designated as a yellow dwarf because its visible radiation is most intense in the yellow-green portion of the spectrum, and although it is actually white in color, from the surface of the Earth it may appear yellow because of atmospheric scattering of blue light. In the spectral class label, G2 indicates its surface temperature, of approximately 5778 K (5505 °C), and V indicates that the Sun, like most stars, is a main-sequence star, and thus generates its energy by nuclear fusion of hydrogen nuclei into helium. In its core, the Sun fuses 620 million metric tons of hydrogen each second.";
            entSun.mBodyType = BodyType.Star;

            this._obj_orbitalbodymanager.SetRootBody(entSun); //Set the systems root body.

            OrbitalBody entMercury = new OrbitalBody("Mercury", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, mercuryDiameter, mercuryMass);
            entMercury.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Mercury/colour"));
            entMercury.mModel.AddTexture(enumTextureType.MAP_NORMAL, this._local_content.Load<Texture2D>("Core/Textures/Mercury/normal"));
            entMercury.mModel.AddShader("Main", enumShaderType.NORMAL, this._local_content.Load<Effect>("Core/Shaders/NormalMap"));
            entMercury.mSemiMajorAxis = 57909295.94;
            entMercury.mEccentricity = 0.205639138;
            entMercury.mInclination = 7.004221459; //Degrees
            entMercury.mArgPeriapsis = 29.16250964; //Degrees
            entMercury.mLongOfAscendNode = 48.3148742; //Degrees
            entMercury.mMeanAnomAtEpoch = 2.91890837; //Radians
            entMercury.dataSiderealDay = 0.24101260274; //Days
            entMercury.dataObliquity = 2.11; //Degrees
            entMercury.dataInfo = "Mercury is the smallest and closest to the Sun of the eight planets in the Solar System.";
            entMercury.dataInfo += "\n.\n.\nMercury does not experience seasons in the same way as most other planets, such as the Earth. It is locked so it rotates in a way that is unique in the Solar System. As seen relative to the fixed stars, it rotates exactly three times for every two revolutions it makes around its orbit. As seen from the Sun, in a frame of reference that rotates with the orbital motion, it appears to rotate only once every two Mercurian years. An observer on Mercury would therefore see only one day every two years.";
            entMercury.mBodyType = BodyType.Planet;

            OrbitalBody entVenus = new OrbitalBody("Venus", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, venusDiameter, venusMass);
            entVenus.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Venus/colour"));
            entVenus.mModel.AddTexture(enumTextureType.MAP_NORMAL, this._local_content.Load<Texture2D>("Core/Textures/Venus/normal"));
            entVenus.mModel.AddShader("Main", enumShaderType.NORMAL, this._local_content.Load<Effect>("Core/Shaders/NormalMap"));
            entVenus.mSemiMajorAxis = 108208736.8;
            entVenus.mEccentricity = 0.006777661;
            entVenus.mInclination = 3.39466769; //Degrees
            entVenus.mArgPeriapsis = 55.19975855; //Degrees
            entVenus.mLongOfAscendNode = 76.64407235; //Degrees
            entVenus.mMeanAnomAtEpoch = 1.70787873; //Radians
            entVenus.dataSiderealDay = 0.61561643835; //Days
            entVenus.dataObliquity = 177.36; //Degrees
            entVenus.dataInfo = "Venus is the second planet from the Sun, orbiting it every 224.7 Earth days.";
            entVenus.dataInfo += "\n.\n.\nVenus is a terrestrial planet and is sometimes called Earth's \"sister planet\" because of their similar size, gravity, and bulk composition (Venus is both the closest planet to Earth and the planet closest in size to Earth). However, it has also been shown to be very different from Earth in other respects. It has the densest atmosphere of the four terrestrial planets, consisting of more than 96% carbon dioxide. The atmospheric pressure at the planet's surface is 92 times that of Earth's. With a mean surface temperature of 735 K (462 °C; 863 °F), Venus is by far the hottest planet in the Solar System.";
            entVenus.mBodyType = BodyType.Planet;

            OrbitalBody entEarth = new OrbitalBody("Earth", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, earthDiameter, earthMass);
            entEarth.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Earth/colour"));
            entEarth.mModel.AddTexture(enumTextureType.MAP_NORMAL, this._local_content.Load<Texture2D>("Core/Textures/Earth/normal"));
            entEarth.mModel.AddTexture(enumTextureType.MAP_SPECULAR, this._local_content.Load<Texture2D>("Core/Textures/Earth/specular"));
            entEarth.mModel.AddTexture(enumTextureType.MAP_LIGHTS, this._local_content.Load<Texture2D>("Core/Textures/Earth/lights"));
            entEarth.mModel.AddTexture(enumTextureType.MAP_CLOUD, this._local_content.Load<Texture2D>("Core/Textures/Earth/cloud"));
            entEarth.mModel.AddTexture(enumTextureType.MAP_WATER, this._local_content.Load<Texture2D>("Core/Textures/Earth/water"));
            entEarth.mModel.AddTexture(enumTextureType.LAYER_SPECIAL1, this._local_content.Load<Texture2D>("Core/Textures/Earth/atmosphere"));
            entEarth.mModel.AddShader("Main", enumShaderType.EARTH, this._local_content.Load<Effect>("Core/Shaders/Special/Earth"));
            entEarth.mModel.AmbientIntensity = 0.15f;
            entEarth.mModel.AmbientColor = Color.White;

            entEarth.mSemiMajorAxis = 149000000;
            entEarth.mEccentricity = 0.0161;
            entEarth.mInclination = 0.0016; //Degrees
            entEarth.mArgPeriapsis = 288; //Degress 
            entEarth.mLongOfAscendNode = 174; //Degrees
            entEarth.mMeanAnomAtEpoch = 6.26573201; //Radians
            entEarth.dataSiderealDay = 0.99885753424; //Days
            entEarth.dataObliquity = 23.44; //Degrees
            entEarth.dataInfo = "Earth is the third planet from the Sun. It is the densest and fifth-largest of the eight planets in the Solar System. It is also the largest of the Solar System's four terrestrial planets. It is sometimes referred to as the world or the Blue Planet.";
            entEarth.dataInfo += "\n.\n.\nEarth formed approximately 4.54 billion years ago, and life appeared on its surface within its first billion years. Earth's biosphere then significantly altered the atmospheric and other basic physical conditions, which enabled the proliferation of organisms as well as the formation of the ozone layer, which together with Earth's magnetic field blocked harmful solar radiation, and permitted formerly ocean-confined life to move safely to land. The physical properties of the Earth, as well as its geological history and orbit, have allowed life to persist.";
            entEarth.mBodyType = BodyType.Planet;

            OrbitalBody entMoon = new OrbitalBody("Moon", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entEarth, moonDiameter, moonMass);
            entMoon.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Satellites/Earth/Moon/colour"));
            entMoon.mModel.AddTexture(enumTextureType.MAP_NORMAL, this._local_content.Load<Texture2D>("Core/Textures/Satellites/Earth/Moon/normal"));
            entMoon.mModel.AddShader("Main", enumShaderType.NORMAL, this._local_content.Load<Effect>("Core/Shaders/NormalMap"));
            entMoon.mSemiMajorAxis = 384171.7431;
            entMoon.mEccentricity = 0.04800149;
            entMoon.mInclination = 5.13700329; //Degrees
            entMoon.mArgPeriapsis = 33.57946453; //Degrees
            entMoon.mLongOfAscendNode = 234.8040878; //Degrees
            entMoon.mMeanAnomAtEpoch = 4.1302971; //Radians
            entMoon.dataSiderealDay = 0.07468575342; //Days
            entMoon.dataObliquity = 6.68; //Degrees
            entMoon.dataInfo = "The Moon is the only natural satellite of the Earth and the fifth largest moon in the Solar System. It is the largest natural satellite of a planet in the Solar System relative to the size of its primary, having 27% the diameter and 60% the density of Earth, resulting in 1 81st its mass. Among satellites with known densities, the Moon is the second densest, after Io, a satellite of Jupiter.";
            entMoon.dataInfo += "\n.\n.\nThe Moon is in synchronous rotation with Earth, always showing the same face with its near side marked by dark volcanic maria that fill between the bright ancient crustal highlands and the prominent impact craters. It is the brightest object in the sky after the Sun, although its surface is actually dark, with a reflectance just slightly higher than that of worn asphalt. Its prominence in the sky and its regular cycle of phases have, since ancient times, made the Moon an important cultural influence on language, calendars, art and mythology. The Moon's gravitational influence produces the ocean tides and the minute lengthening of the day.";
            entMoon.mBodyType = BodyType.Moon;

            OrbitalBody entMars = new OrbitalBody("Mars", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, marsDiameter, marsMass);
            entMars.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Mars/colour"));
            entMars.mModel.AddTexture(enumTextureType.MAP_NORMAL, this._local_content.Load<Texture2D>("Core/Textures/Mars/normal"));
            entMars.mModel.AddShader("Main", enumShaderType.NORMAL, this._local_content.Load<Effect>("Core/Shaders/NormalMap"));
            entMars.mSemiMajorAxis = 227931320.1;
            entMars.mEccentricity = 0.093286218;
            entMars.mInclination = 1.848696949; //Degrees
            entMars.mArgPeriapsis = 286.5393162; //Degrees
            entMars.mLongOfAscendNode = 49.52462562; //Degrees
            entMars.mMeanAnomAtEpoch = 6.06942603; //Radians
            entMars.dataSiderealDay = 1.88201643836; //Days
            entMars.dataObliquity = 25.19; //Degrees
            entMars.dataInfo = "Mars is the fourth planet from the Sun and the second smallest planet in the Solar System. Named after the Roman god of war, it is often described as the \"Red Planet\" because the iron oxide prevalent on its surface gives it a reddish appearance.";
            entMars.dataInfo += "\n.\n.\nMars is a terrestrial planet with a thin atmosphere, having surface features reminiscent both of the impact craters of the Moon and the volcanoes, valleys, deserts, and polar ice caps of Earth. The rotational period and seasonal cycles of Mars are likewise similar to those of Earth, as is the tilt that produces the seasons. Mars is the site of Olympus Mons, the second highest known mountain within the Solar System (the tallest on a planet), and of Valles Marineris, one of the largest canyons. The smooth Borealis basin in the northern hemisphere covers 40% of the planet and may be a giant impact feature.";
            entMars.mBodyType = BodyType.Planet;

            OrbitalBody entJupiter = new OrbitalBody("Jupiter", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, jupiterDiameter, jupiterMass);
            entJupiter.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Jupiter/colour"));
            entJupiter.mModel.AddShader("Main", enumShaderType.TEXTURE, this._local_content.Load<Effect>("Core/Shaders/Texture"));
            entJupiter.mSemiMajorAxis = 778239866.1;
            entJupiter.mEccentricity = 0.048844331;
            entJupiter.mInclination = 1.303822707; //Degrees
            entJupiter.mArgPeriapsis = 273.8228136; //Degrees
            entJupiter.mLongOfAscendNode = 100.5137646; //Degrees
            entJupiter.mMeanAnomAtEpoch = 0.95268889; //Radians
            entJupiter.dataSiderealDay = 11.8680821918; //Days
            entJupiter.dataObliquity = 3.13; //Degrees
            entJupiter.dataInfo = "Jupiter is the fifth planet from the Sun and the largest planet in the Solar System.";
            entJupiter.dataInfo += "\n.\n.\nIt is a gas giant with mass one-thousandth of that of the Sun but is two and a half times the mass of all the other planets in the Solar System combined. Jupiter is classified as a gas giant along with Saturn, Uranus and Neptune. Together, these four planets are sometimes referred to as the Jovian or outer planets. The planet was known by astronomers of ancient times, and was associated with the mythology and religious beliefs of many cultures. The Romans named the planet after the Roman god Jupiter. When viewed from Earth, Jupiter can reach an apparent magnitude of negative 2.94, bright enough to cast shadows, and making it on average the third-brightest object in the night sky after the Moon and Venus. (Mars can briefly match Jupiter's brightness at certain points in its orbit.)";
            entJupiter.mBodyType = BodyType.Planet;

            OrbitalBody entSaturn = new OrbitalBody("Saturn", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, saturnDiameter, saturnMass);
            entSaturn.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Saturn/colour"));
            entSaturn.mModel.AddShader("Main", enumShaderType.TEXTURE, this._local_content.Load<Effect>("Core/Shaders/Texture"));
            entSaturn.mRing = new PlanetRing(this.ScreenManager.GraphicsDevice, this._local_content.Load<Texture2D>("Core/Textures/Saturn/Ring/colour"), this._local_content.Load<Texture2D>("Core/Textures/Saturn/Ring/transparency"), new Vector3(1, 1, 1) * 100f, this._local_content.Load<Model>("Core/Models/Extra/Ring/model"), this._local_content.Load<Effect>("Core/Shaders/Special/Rings"));
            entSaturn.mSemiMajorAxis = 1425565345;
            entSaturn.mEccentricity = 0.055139264;
            entSaturn.mInclination = 2.487527057; //Degrees
            entSaturn.mArgPeriapsis = 337.9674662; //Degrees
            entSaturn.mLongOfAscendNode = 113.6097295; //Degrees
            entSaturn.mMeanAnomAtEpoch = 2.04707715; //Radians
            entSaturn.dataSiderealDay = 29.4331506849; //Days
            entSaturn.dataObliquity = 26.73; //Degrees
            entSaturn.dataInfo = "Saturn is the sixth planet from the Sun and the second largest planet in the Solar System, after Jupiter. Named after the Roman god of agriculture, Saturn, its astronomical symbol represents the god's sickle.";
            entSaturn.dataInfo += "\n.\n.\nSaturn is a gas giant with an average radius about nine times that of Earth. While only one-eighth the average density of Earth, with its larger volume Saturn is just over 95 times more massive.";
            entSaturn.mBodyType = BodyType.Planet;

            OrbitalBody entUranus = new OrbitalBody("Uranus", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, uranusDiameter, uranusMass);
            entUranus.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Uranus/colour"));
            entUranus.mModel.AddShader("Main", enumShaderType.TEXTURE, this._local_content.Load<Effect>("Core/Shaders/Texture"));
            entUranus.mRing = new PlanetRing(this.ScreenManager.GraphicsDevice, this._local_content.Load<Texture2D>("Core/Textures/Uranus/Ring/colour"), this._local_content.Load<Texture2D>("Core/Textures/Uranus/Ring/transparency"), new Vector3(1, 1, 1) * 100f, this._local_content.Load<Model>("Core/Models/Extra/Ring/model"), this._local_content.Load<Effect>("Core/Shaders/Special/Rings"));
            entUranus.mSemiMajorAxis = 2876880639;
            entUranus.mEccentricity = 0.045660987;
            entUranus.mInclination = 0.772862413; //Degrees
            entUranus.mArgPeriapsis = 94.71313686; //Degrees
            entUranus.mLongOfAscendNode = 73.85498192; //Degrees
            entUranus.mMeanAnomAtEpoch = 3.50016014; //Radians
            entUranus.dataSiderealDay = -84.3898630137; //Days
            entUranus.dataObliquity = 97.77; //Degrees
            entUranus.dataInfo = "Uranus is the seventh planet from the Sun. It has the third-largest planetary radius and fourth-largest planetary mass in the Solar System.";
            entUranus.dataInfo += "\n.\n.\nUranus is similar in composition to Neptune, and both are of different chemical composition than the larger gas giants Jupiter and Saturn. For this reason, astronomers sometimes place them in a separate category called \"ice giants\". Uranus's atmosphere, although similar to Jupiter's and Saturn's in its primary composition of hydrogen and helium, contains more \"ices\" such as water, ammonia, and methane, along with traces of hydrocarbons.";
            entUranus.mBodyType = BodyType.Planet;

            OrbitalBody entNeptune = new OrbitalBody("Neptune", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, neptuneDiameter, neptuneMass);
            entNeptune.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Neptune/colour"));
            entNeptune.mModel.AddShader("Main", enumShaderType.TEXTURE, this._local_content.Load<Effect>("Core/Shaders/Texture"));
            entNeptune.mSemiMajorAxis = 4497972602;
            entNeptune.mEccentricity = 0.010492249;
            entNeptune.mInclination = 1.771660459; //Degrees
            entNeptune.mArgPeriapsis = 277.0970306; //Degrees
            entNeptune.mLongOfAscendNode = 131.8082139; //Degrees
            entNeptune.mMeanAnomAtEpoch = 4.96733787; //Radians
            entNeptune.dataSiderealDay = 164.98; //Days
            entNeptune.dataObliquity = 28.32; //Degrees
            entNeptune.dataInfo = "Neptune is the eighth and farthest planet from the Sun in the Solar System. It is the fourth-largest planet by diameter and the third-largest by mass.";
            entNeptune.dataInfo += "\n.\n.\nAmong the gaseous planets in the solar system, Neptune is the most dense. Neptune is 17 times the mass of Earth and is slightly more massive than its near-twin Uranus, which is 15 times the mass of Earth but not as dense. On average, Neptune orbits the Sun at a distance of 30.1 AU, approximately 30 times the Earth to Sun distance. Named after the Roman god of the sea, its astronomical symbol is a stylised version of the god Neptune's trident.";
            entNeptune.mBodyType = BodyType.Planet;

            OrbitalBody entPluto = new OrbitalBody("Pluto", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/BaseObject/High/model"), ref entSun, plutoDiameter, plutoMass);
            entPluto.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Pluto/colour"));
            entPluto.mModel.AddTexture(enumTextureType.MAP_NORMAL, this._local_content.Load<Texture2D>("Core/Textures/Pluto/normal"));
            entPluto.mModel.AddShader("Main", enumShaderType.NORMAL, this._local_content.Load<Effect>("Core/Shaders/NormalMap"));
            entPluto.mSemiMajorAxis = 5829948667;
            entPluto.mEccentricity = 0.242982541;
            entPluto.mInclination = 17.12521472; //Degrees
            entPluto.mArgPeriapsis = 111.5170014; //Degrees
            entPluto.mLongOfAscendNode = 110.3158146; //Degrees
            entPluto.mMeanAnomAtEpoch = 0.624046112; //Radians
            entPluto.dataSiderealDay = -243.45260274; //Days
            entPluto.dataObliquity = 122.53; //Degrees
            entPluto.dataInfo = "Pluto, minor-planet designation 134340 Pluto, is the largest object in the Kuiper belt, and the tenth-most-massive body observed directly orbiting the Sun. It is the second-most-massive known dwarf planet, after Eris.";
            entPluto.dataInfo += "\n.\n.\nLike other Kuiper-belt objects, Pluto is composed primarily of rock and ice and is relatively small, approximately one-sixth the mass of the Moon and one-third its volume. It has an eccentric and highly inclined orbit that takes it from 30 to 49 AU (4.4 to 7.4 billion km) from the Sun. This causes Pluto to periodically come closer to the Sun than Neptune. As of 2014, it is 32.6 AU from the Sun.";
            entPluto.mBodyType = BodyType.Dwarf;
            //!Create Primary Bodies

            //Extra Objects
            OrbitalBody entISS = new OrbitalBody("Internaional Space Station", ref this._obj_orbitalbodymanager, this._local_content.Load<Model>("Core/Models/ISS/Low/model"), ref entEarth, 200, 450000);
            entISS.mModel.AddTexture(enumTextureType.MAP_COLOUR, this._local_content.Load<Texture2D>("Core/Textures/Pluto/colour"));
            entISS.mModel.AddShader("Main", enumShaderType.TEXTURE, this._local_content.Load<Effect>("Core/Shaders/Texture"));
            entISS.mDistanceScale = 15;
            entISS.mSemiMajorAxis = 6792.600738 * entISS.mDistanceScale;
            entISS.mEccentricity = 0.001994877;
            entISS.mInclination = 70.95790737; //Degrees
            entISS.mArgPeriapsis = 114.8348368; //Degrees
            entISS.mLongOfAscendNode = 211.1753628; //Degrees
            entISS.mMeanAnomAtEpoch = 3.99196249; //Radians
            entISS.dataInfo = "The International Space Station (ISS) is a space station, or a habitable artificial satellite in low Earth orbit. It is a modular structure whose first component was launched in 1998. Now the largest artificial body in orbit, it can often be seen at the appropriate time with the naked eye from Earth. The ISS consists of pressurised modules, external trusses, solar arrays and other components. ISS components have been launched by American Space Shuttles as well as Russian Proton and Soyuz rockets. In 1984 the ESA was invited to participate in Space Station Freedom. In 1993, after the USSR ended, the United States and Russia merged Mir-2 and Freedom together.";
            entISS.dataInfo += "\n.\n.\nThe ISS serves as a microgravity and space environment research laboratory in which crew members conduct experiments in biology, human biology, physics, astronomy, meteorology and other fields. The station is suited for the testing of spacecraft systems and equipment required for missions to the Moon and Mars.";
            entISS.mBodyType = BodyType.ManMadeSatellite;
            entISS.mScale = entISS.mScale * 100;
            //!Extra Objects

            //Primary Bodies
            this._obj_orbitalbodymanager.Add(entSun);
            this._obj_orbitalbodymanager.Add(entMercury);
            this._obj_orbitalbodymanager.Add(entVenus);
            this._obj_orbitalbodymanager.Add(entEarth);
            this._obj_orbitalbodymanager.Add(entISS); // Extra Object - ISS
            this._obj_orbitalbodymanager.Add(entMoon);
            this._obj_orbitalbodymanager.Add(entMars);
            this._obj_orbitalbodymanager.Add(entJupiter);
            this._obj_orbitalbodymanager.Add(entSaturn);
            this._obj_orbitalbodymanager.Add(entUranus);
            this._obj_orbitalbodymanager.Add(entNeptune);
            this._obj_orbitalbodymanager.Add(entPluto);

            for (int i = 0; i < this._obj_orbitalbodymanager.Size(); i++)
            {
                this._obj_orbitalbodymanager.Get(i).mMass = this._obj_orbitalbodymanager.Get(i).mMass / mSmallScale;
                this._obj_orbitalbodymanager.Get(i).mMU = (GameSettings.PHYSICS_GRAVITY_CONSTANT / mSmallScale) * this._obj_orbitalbodymanager.Get(i).mMass;
                this._obj_orbitalbodymanager.Get(i).mSemiMajorAxis = this._obj_orbitalbodymanager.Get(i).mSemiMajorAxis / mSmallScale;
                OrbitalBody tmpBody = this._obj_orbitalbodymanager.Get(i);
                OrbitalMechanics.AddToSystem(tmpBody);
            }

            if (!this._obj_orbitalbodymanager._SIM_MODE)
            {
                //Play Music
                this._sfx_music.Play();
            }
        }

        public override void unloadContent()
        {
            this._local_content.Unload();
            base.unloadContent();
        }

        public override void bgUpdate(bool potherfocused, bool poverlaid)
        {
            base.bgUpdate(potherfocused, poverlaid);
        }

        public override void update()
        {
            TimeSpan _timer = this.GlobalGameTimer.ElapsedGameTime;
            TimeSpan _totalTime = this.GlobalGameTimer.TotalGameTime;
            float dT = (float)_timer.TotalSeconds; //dT

            if (!this._obj_orbitalbodymanager._SIM_MODE)
            {
                this._obj_gui.controlsWindow.Position = new Squid.Point((this.ScreenManager.GameViewport.Width / 2) - (this._obj_gui.controlsWindow.Size.x / 2), (this.ScreenManager.GameViewport.Height / 2) - (this._obj_gui.controlsWindow.Size.y / 2));

                //Update Info Window
                TextBox _tmpBox;
                Label _tmpLabel;

                _tmpBox = (TextBox)this._obj_gui.infoPanel.Content.Controls[1];
                _tmpBox.Text = this._obj_orbitalbodymanager.Get(_chase_active_planet).mObjectName;
                this._obj_gui.infoPanel.Content.Controls[1] = _tmpBox;

                _tmpBox = (TextBox)this._obj_gui.infoPanel.Content.Controls[3];
                _tmpBox.Text = String.Format("{0} km/s", Math.Round(this._obj_orbitalbodymanager.Get(_chase_active_planet).dataVelocity, 2).ToString());
                this._obj_gui.infoPanel.Content.Controls[3] = _tmpBox;

                _tmpBox = (TextBox)this._obj_gui.infoPanel.Content.Controls[5];
                _tmpBox.Text = String.Format("{0} km", Math.Round(this._obj_orbitalbodymanager.Get(_chase_active_planet).dataDistance, 2).ToString());
                this._obj_gui.infoPanel.Content.Controls[5] = _tmpBox;

                _tmpBox = (TextBox)this._obj_gui.infoPanel.Content.Controls[7];

                double _orbitPeriod = this._obj_orbitalbodymanager.Get(_chase_active_planet).dataPeriod;
                string _timeFrame = "seconds";

                if (_orbitPeriod > 60) //Greater than 60 seconds
                {
                    _orbitPeriod = _orbitPeriod / 60;
                    _timeFrame = "minutes";

                    if (_orbitPeriod > 120) //Greater than 120 Minutes
                    {
                        _orbitPeriod = _orbitPeriod / 60;
                        _timeFrame = "hours";

                        if (_orbitPeriod > 24) //Greater than 24 hours
                        {
                            _orbitPeriod = _orbitPeriod / 24;
                            _timeFrame = "days";

                            if (_orbitPeriod > 365) //Greater than 365 days
                            {
                                _orbitPeriod = _orbitPeriod / 365;
                                _timeFrame = "years";

                                if (_orbitPeriod > 10) //Greater than 10 years
                                {
                                    _orbitPeriod = _orbitPeriod / 10;
                                    _timeFrame = "decades";

                                    if (_orbitPeriod > 10) //Greater than 10 decades
                                    {
                                        _orbitPeriod = _orbitPeriod / 10;
                                        _timeFrame = "centuries";

                                        if (_orbitPeriod > 10) //Greater than 10 centuries
                                        {
                                            _orbitPeriod = _orbitPeriod / 10;
                                            _timeFrame = "millenia";

                                            if (_orbitPeriod > 10) //Greater than 10 millenia
                                            {
                                                _orbitPeriod = _orbitPeriod / 10;
                                                _timeFrame = "epoch";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                _tmpBox.Text = String.Format("{0} " + _timeFrame, Math.Round(_orbitPeriod, 2));
                this._obj_gui.infoPanel.Content.Controls[7] = _tmpBox;

                _tmpLabel = (Label)this._obj_gui.infoPanel.Content.Controls[9];
                _tmpLabel.Text = this._obj_orbitalbodymanager.Get(_chase_active_planet).dataInfo;
                this._obj_gui.infoPanel.Content.Controls[9] = _tmpLabel;
                //--Update Info Window

                if (this._sfx_music.State == SoundState.Stopped && this._sfx_ambience.State == SoundState.Stopped)
                    this._sfx_ambience.Play();

                if (this.GlobalInput.IsPressed("PLANET_INFO", this.ControllingPlayer))
                    this.mDrawInfo = !this.mDrawInfo;

                if (this.GlobalInput.IsPressed("SYSTEM_TOGGLE_FULLSCREEN", this.ControllingPlayer))
                {
                    if (this.ScreenManager.GraphicsDeviceManager.IsFullScreen == true)
                    {
                        this.ScreenManager.GraphicsDeviceManager.PreferredBackBufferWidth = this.ScreenManager.ConfigManager.CoreSettings.VIDEO_RES_WIDTH;
                        this.ScreenManager.GraphicsDeviceManager.PreferredBackBufferHeight = this.ScreenManager.ConfigManager.CoreSettings.VIDEO_RES_HEIGHT;
                        this.ScreenManager.GraphicsDeviceManager.IsFullScreen = false;
                        this.ScreenManager.GraphicsDeviceManager.ApplyChanges();
                        this._obj_cameras[_active_camera].ResetCamera();
                        this._obj_cameras[_active_camera].Update(_timer, this._chaseWorld); //Update the camera.
                    }
                    else
                    {
                        this.ScreenManager.GraphicsDeviceManager.PreferredBackBufferWidth = this.ScreenManager.GraphicsDevice.DisplayMode.Width;
                        this.ScreenManager.GraphicsDeviceManager.PreferredBackBufferHeight = this.ScreenManager.GraphicsDevice.DisplayMode.Height;
                        this.ScreenManager.GraphicsDeviceManager.IsFullScreen = true;
                        this.ScreenManager.GraphicsDeviceManager.ApplyChanges();
                        this._obj_cameras[_active_camera].ResetCamera();
                        this._obj_cameras[_active_camera].Update(_timer, this._chaseWorld); //Update the camera.
                    }
                }

                if (this.GlobalInput.IsPressed("CAMERA_FREE", this.ControllingPlayer))
                {
                    if (_active_camera != CameraType.Free)
                    {
                        _active_camera = CameraType.Free;
                        this._obj_cameras[_active_camera].ResetCamera();
                        this._chaseWorld = Matrix.Identity;
                    }
                }
                if (this.GlobalInput.IsPressed("CAMERA_CHASE", this.ControllingPlayer))
                {
                    if (_active_camera != CameraType.Chase)
                    {
                        _active_camera = CameraType.Chase;
                        this._obj_cameras[_active_camera].ResetCamera();
                        this._chaseWorld = this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.World;
                    } 
                }
                if (this.GlobalInput.IsPressed("CAMERA_ORBIT", this.ControllingPlayer))
                {
                    if (_active_camera != CameraType.Orbit)
                    {
                        _active_camera = CameraType.Orbit;
                        this._obj_cameras[_active_camera].ResetCamera();
                        this._chaseWorld = this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.World;
                        this._obj_cameras[_active_camera].CameraOffset = new Vector3D(0, 0, (float)this._obj_orbitalbodymanager.Get(_chase_active_planet).mScale * 50);
                    }
                }
                if (this.GlobalInput.IsPressed("CAMERA_TOP", this.ControllingPlayer))
                {
                    if (_active_camera != CameraType.Top)
                    {
                        _active_camera = CameraType.Top;
                        this._obj_cameras[_active_camera].ResetCamera();
                        _chase_active_planet = 0;
                        this._obj_cameras[_active_camera].Position = new Vector3D(this._obj_cameras[_active_camera].Position.X, this._obj_cameras[_active_camera].Position.Y + 100000, this._obj_cameras[_active_camera].Position.Z);
                        this._chaseWorld = this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.World;
                        this._obj_cameras[_active_camera].CameraOffset = new Vector3D(0, 0, (float)this._obj_orbitalbodymanager.Get(_chase_active_planet).mScale * 50);
                    }
                }

                if (_active_camera == CameraType.Orbit || _active_camera == CameraType.Chase)
                {
                    if (this.GlobalInput.IsPressed("CAMERA_ZOOM_IN", this.ControllingPlayer))
                    {
                        if (Vector3D.Distance(this._obj_cameras[_active_camera].Position, this._obj_orbitalbodymanager.Get(_chase_active_planet).sCurrent.position) > (this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.CurrentBounds.Radius + (10 * this._obj_orbitalbodymanager.Get(_chase_active_planet).mScale)))
                        {
                            this._obj_cameras[_active_camera].CameraOffset -= new Vector3D(0, 0, (0.5f * ((float)this._obj_orbitalbodymanager.Get(_chase_active_planet).mScale * 50)) * dT);
                        }
                    }
                    else if (this.GlobalInput.IsPressed("CAMERA_ZOOM_OUT", this.ControllingPlayer))
                    {
                        this._obj_cameras[_active_camera].CameraOffset += new Vector3D(0, 0, (0.5f * ((float)this._obj_orbitalbodymanager.Get(_chase_active_planet).mScale * 50)) * dT);
                    }
                }

                if (_active_camera == CameraType.Top)
                {
                    if (this.GlobalInput.IsPressed("CAMERA_ZOOM_IN", this.ControllingPlayer))
                    {
                        if (Vector3D.Distance(this._obj_cameras[_active_camera].Position, this._obj_orbitalbodymanager.Get(_chase_active_planet).sCurrent.position) > (this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.CurrentBounds.Radius + (10 * this._obj_orbitalbodymanager.Get(_chase_active_planet).mScale)))
                        {
                            this._obj_cameras[_active_camera].Position = new Vector3D(this._obj_cameras[_active_camera].Position.X, this._obj_cameras[_active_camera].Position.Y - (100000 * dT), this._obj_cameras[_active_camera].Position.Z);
                        }
                    }
                    else if (this.GlobalInput.IsPressed("CAMERA_ZOOM_OUT", this.ControllingPlayer))
                    {
                        this._obj_cameras[_active_camera].Position = new Vector3D(this._obj_cameras[_active_camera].Position.X, this._obj_cameras[_active_camera].Position.Y + (100000 * dT), this._obj_cameras[_active_camera].Position.Z);
                    }
                }

                if (_active_camera == CameraType.Chase || _active_camera == CameraType.Top)
                {
                    if (this.GlobalInput.IsPressed("PLANET_CHASE_NEXT", this.ControllingPlayer))
                    {
                        if ((_chase_active_planet + 1) >= _obj_orbitalbodymanager.Size())
                            _chase_active_planet = 0;
                        else
                            _chase_active_planet += 1;

                        this._chaseWorld = this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.World;
                    }
                    else if (this.GlobalInput.IsPressed("PLANET_CHASE_PREV", this.ControllingPlayer))
                    {
                        if ((_chase_active_planet - 1) < 0)
                            _chase_active_planet = _obj_orbitalbodymanager.Size() - 1;
                        else
                            _chase_active_planet -= 1;

                        this._chaseWorld = this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.World;
                    }
                }

                if (this.GlobalInput.IsPressed("SIM_PAUSE", this.ControllingPlayer))
                    this._sim_pause = !this._sim_pause;

                if (this.GlobalInput.IsPressed("SIM_SPEED_INCREASE", this.ControllingPlayer))
                {
                    if (this._sim_speed + 1 <= this._sim_max_speed)
                        this._sim_speed += 1;
                }
                else if (this.GlobalInput.IsPressed("SIM_SPEED_DECREASE", this.ControllingPlayer))
                {
                    if (this._sim_speed - 1 >= 1)
                        this._sim_speed -= 1;
                }

                //Update GUI Components
                this._obj_gui.Size = new Squid.Point(this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);
                bool[] mouseButtons = this.GlobalInput.GetMouseButtons();
                int wheel = this.GlobalInput.CurrentMouseState.ScrollWheelValue > this.GlobalInput.PreviousMouseState.ScrollWheelValue ? -1 : (this.GlobalInput.CurrentMouseState.ScrollWheelValue < this.GlobalInput.PreviousMouseState.ScrollWheelValue ? 1 : 0);
                GuiHost.SetMouse(this.GlobalInput.CurrentMouseState.X, this.GlobalInput.CurrentMouseState.Y, this.GlobalInput.CurrentMouseState.ScrollWheelValue);
                GuiHost.SetButtons(mouseButtons);
                GuiHost.TimeElapsed = _timer.Milliseconds;
                this._obj_gui.Update();
                //--Update GUI Components
            }

            this._obj_orbitalbodymanager.Update(_timer, _totalTime, this._sim_pause, this._sim_speed); //Update the physics and data of the each entity.

            if (!this._obj_orbitalbodymanager._SIM_MODE)
            {
                if (this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel != null)
                    this._chaseWorld = this._obj_orbitalbodymanager.Get(_chase_active_planet).mModel.World;

                this._obj_cameras[_active_camera].HandleInput(_active_camera, this.ControllingPlayer); //Handle Camera Input
                this._obj_cameras[_active_camera].Update(_timer, this._chaseWorld); //Update the camera.
            }

            base.update();
        }

        public override void appRender()
        {
            if (!this._obj_orbitalbodymanager._SIM_MODE)
            {
                this.internResetRenderStatesFor3D(); //Reset states

                SamplerState ss = new SamplerState();
                ss.AddressU = TextureAddressMode.Clamp;
                ss.AddressV = TextureAddressMode.Clamp;
                ScreenManager.GraphicsDevice.SamplerStates[0] = ss;

                DepthStencilState dss = new DepthStencilState();
                dss.DepthBufferEnable = false;
                ScreenManager.GraphicsDevice.DepthStencilState = dss;
                this._obj_skybox.Draw(this._obj_cameras[_active_camera]); //Draw the skysphere (Skybox in sphere form)
                dss = new DepthStencilState();
                dss.DepthBufferEnable = true;
                ScreenManager.GraphicsDevice.DepthStencilState = dss;

                this._obj_orbitalbodymanager.Draw(this.ScreenManager.SpriteBatch, this._obj_cameras[_active_camera], true, this.mDrawInfo); //Draw the planets.

                this.internResetRenderStatesFor2D();
                this._obj_gui.Draw();
            }
            else
            {
                this._obj_orbitalbodymanager.Draw(this.ScreenManager.SpriteBatch, this._obj_cameras[_active_camera], true, this.mDrawInfo); //Draw the planets.
                this.ScreenManager.SpriteBatch.Begin();
                string _sim_msg = "-RUNNING SIMULATION ("+ this._obj_orbitalbodymanager._SIM_MODE_PLANET +")-";
                Vector2 _sim_msg_origin = this.ScreenManager.DefaultDebugFont.MeasureString(_sim_msg) / 2f;
                this.ScreenManager.SpriteBatch.DrawString(this.ScreenManager.DefaultDebugFont, _sim_msg, new Vector2(this.ScreenManager.GameViewport.Width / 2, this.ScreenManager.GameViewport.Height / 2), Color.White, 0f, _sim_msg_origin, 1f, SpriteEffects.None, 0f);
                this.ScreenManager.SpriteBatch.End();
                
            }
        }
    }
}