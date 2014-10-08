using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Management;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using POSystem;

namespace Configurator
{
    public partial class frmMain : Form
    {
        Dictionary<string, Resolution> allResolutions;
        private const string CONFIG_GAME_NAME_CLEAN = "PlanetaryOrbit";
        private const string CONFIG_SETTINGS_FILE = "base.config";
        private SystemSettings _obj_settings;
        private bool _LOADED = true;
        private string data_system_dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), CONFIG_GAME_NAME_CLEAN);

        #region "Resolutions"
        public struct Resolution
        {
            public int resWidth;
            public int resHeight;
        }

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
        const int ENUM_CURRENT_SETTINGS = -1;
        const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public void GetResolutions()
        {
            DEVMODE vDevMode = new DEVMODE();
            int i = 0;
            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                if (!this.allResolutions.ContainsKey(vDevMode.dmPelsWidth.ToString() + "x" + vDevMode.dmPelsHeight.ToString()))
                {
                    Resolution tmpRes = new Resolution();
                    tmpRes.resWidth = vDevMode.dmPelsWidth;
                    tmpRes.resHeight = vDevMode.dmPelsHeight;
                    this.allResolutions.Add(vDevMode.dmPelsWidth.ToString() + "x" + vDevMode.dmPelsHeight.ToString(), tmpRes);
                }
                i++;
            }
        }
        #endregion
        #region "Read and Create Config File"
        private void ReadFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));
            Stream _stream = null;

            try
            {
                if (!Directory.Exists(data_system_dir))
                {
                    Directory.CreateDirectory(data_system_dir);
                    CreateNewFile();
                }
                else
                {
                    if (File.Exists(data_system_dir + "/" + CONFIG_SETTINGS_FILE))
                    {
                        _stream = File.Open(data_system_dir + "/" + CONFIG_SETTINGS_FILE, FileMode.Open);
                        serializer = new XmlSerializer(typeof(SystemSettings));
                        _obj_settings = (SystemSettings)serializer.Deserialize(_stream);
                        _stream.Close();
                    }
                    else
                        CreateNewFile(); // Creates a new file.
                }
            }
            catch (InvalidOperationException ex) // Error reading and deserializing the file, so creates a new one.
            {
                if (_stream != null)
                    _stream.Close(); // Closes the stream if it exists.

                CreateNewFile(); // Creates a new file.
            }
            catch (Exception ex) // Catches any exception.
            {
                // Toggles the _LOADED variable.
                this._LOADED = false;
                MessageBox.Show("Error: " + ex.Message, "Error - PlanetaryOrbit Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void CreateNewFile()
        {
            try
            {
                // Since this function create's a new file, it checks if it already exists, and delete's it.
                if (File.Exists(data_system_dir + "/" + CONFIG_SETTINGS_FILE))
                    File.Delete(data_system_dir + "/" + CONFIG_SETTINGS_FILE);

                // Opens a file stream, when the file is created.
                FileStream _stream = File.Create(data_system_dir + "/" + CONFIG_SETTINGS_FILE);
                // Creates a new XML serializer object.
                XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));
                // Serializes the class to the file and closes the stream.
                serializer.Serialize(_stream, _obj_settings);
                _stream.Close();
            }
            catch (Exception ex) // Catches any type of exception.
            {
                // Toggles the _LOADED variable.
                this._LOADED = false;
                MessageBox.Show("Error: " + ex.Message, "Error - PlanetaryOrbit Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion
        #region "Buttons"
        private void btnStartGame_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("PlanetaryOrbit.exe"))
                {
                    Process _game = new Process();
                    _game.StartInfo.FileName = "PlanetaryOrbit.exe";
                    _game.Start();

                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("Game not found (PlanetaryOrbit.exe)", "Error - Planetary Orbit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error - Planetary Orbit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            DialogResult diagResult = MessageBox.Show("Are you sure you apply these settings?", "New Settings - Planetary Orbit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (diagResult == DialogResult.OK)
            {
                try
                {
                    string[] resSettings = cbResolution.SelectedItem.ToString().Split('x');
                    this._obj_settings.GAME_TRUESCALE = cbTrueScale.Checked;
                    int.TryParse(resSettings[0], out this._obj_settings.VIDEO_RES_WIDTH);
                    int.TryParse(resSettings[1], out this._obj_settings.VIDEO_RES_HEIGHT);
                    this._obj_settings.VIDEO_FULLSCREEN = cbFullScreen.Checked;
                    this._obj_settings.VIDEO_VSYNC = cbVsync.Checked;
                    this._obj_settings.VIDEO_ANTIALIASING = cbAA.Checked;
                    float.TryParse((numMasterVol.Value / 100).ToString(), out this._obj_settings.AUDIO_MAIN_VOLUME);
                    float.TryParse((numMusicVol.Value / 100).ToString(), out this._obj_settings.AUDIO_MUSIC_VOLUME);
                    float.TryParse((numSFXVol.Value / 100).ToString(), out this._obj_settings.AUDIO_SFX_VOLUME);  
                    this._obj_settings.VIDEO_OCULUS_ENABLED = cbOculus.Checked;
                    this._obj_settings.SIM_ENABLED = cbSimMode.Checked;
                    int.TryParse((numSimSpeed.Value).ToString(), out this._obj_settings.SIM_SPEED);
                    this._obj_settings.SIM_PLANET = txtSimPlanet.Text;
                    CreateNewFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error - Planetary Orbit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult diagResult = MessageBox.Show("Are you sure you want to cancel changing the settings?", "Exit - Planetary Orbit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (diagResult == DialogResult.OK)
            {
                Application.Exit();
            }
        }
        #endregion
        #region "Requirements"
        public enum RequirementsMet
        {
            Recommended,
            Minimum,
            None
        }

        public struct SystemRequirements
        {
            public static double minShaderVersion = 2.0;
            public static double recShaderVersion = 4.0;
            public static int minCPUCores = 2;
            public static int recCPUCores = 4;
            public static int minCPUfreqMHZ = 1800;
            public static int recCPUfreqMHZ = 2400;
            public static int minGPUmemMB = 256;
            public static int recGPUmemMB = 512;
            public static int minRAMmemMB = 128;
            public static int recRAMmemMB = 512;
        }

        public void CheckRequirements()
        {
            RequirementsMet systemTest = RequirementsMet.None;
            bool[] minSystemCheck = {false, false, false, false};
            bool[] recSystemCheck = {false, false, false, false};
            Invoke(new Action(() => pbRequirements.Value = 10));

            //GPU Info
            double adapterRAM = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get())
                adapterRAM += (long.Parse(item["AdapterRAM"].ToString()) / 1024f) / 1024f;

            Invoke(new Action(() => pbRequirements.Value = 20));

            //Processor Info
            int cpuFreq = 0;
            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                cpuFreq += int.Parse(item["CurrentClockSpeed"].ToString());
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }

            Invoke(new Action(() => pbRequirements.Value = 40));

            //System Data
            double systemRAM = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
                systemRAM += (long.Parse(item["TotalPhysicalMemory"].ToString()) / 1024f) / 1024f;

            Invoke(new Action(() => pbRequirements.Value = 50));

            minSystemCheck[0] = coreCount >= SystemRequirements.minCPUCores ? true : false;
            recSystemCheck[0] = coreCount >= SystemRequirements.recCPUCores ? true : false;
            minSystemCheck[1] = adapterRAM >= SystemRequirements.minGPUmemMB ? true : false;
            recSystemCheck[1] = adapterRAM >= SystemRequirements.recGPUmemMB ? true : false;
            minSystemCheck[2] = systemRAM >= SystemRequirements.minRAMmemMB ? true : false;
            recSystemCheck[2] = systemRAM >= SystemRequirements.recRAMmemMB ? true : false;
            minSystemCheck[3] = cpuFreq >= SystemRequirements.minCPUfreqMHZ ? true : false;
            recSystemCheck[3] = cpuFreq >= SystemRequirements.recCPUfreqMHZ ? true : false;

            Invoke(new Action(() => pbRequirements.Value = 60));

            if (minSystemCheck.Where(c => c).Count() == minSystemCheck.Length)
                systemTest = RequirementsMet.Minimum;

            if (recSystemCheck.Where(c => c).Count() == recSystemCheck.Length)
                systemTest = RequirementsMet.Recommended;

            Invoke(new Action(() => pbRequirements.Value = 80));

            switch (systemTest)
            {
                case RequirementsMet.Recommended:
                    Invoke(new Action(() => lblRequirementsMsg.Text = "OK: You meet the recommended requirements to run this game."));
                    Invoke(new Action(() => lblRequirementsMsg.ForeColor = Color.FromArgb(0, 192, 9)));
                    break;
                case RequirementsMet.Minimum:
                    Invoke(new Action(() => lblRequirementsMsg.Text = "OK: You meet the minimum requirements to run this game."));
                    Invoke(new Action(() => lblRequirementsMsg.ForeColor = Color.FromArgb(235, 144, 0)));
                    break;
                case RequirementsMet.None:
                    Invoke(new Action(() => lblRequirementsMsg.Text = "CAUTION: You do not meet minimum requirements to run this game."));
                    Invoke(new Action(() => lblRequirementsMsg.ForeColor = Color.FromArgb(255, 0, 0)));
                    break;
            }

            Invoke(new Action(() => pbRequirements.Value = 100));
            Invoke(new Action(() => pbRequirements.Visible = false));
            Invoke(new Action(() => lblReqLoading.Visible = false));
        }

        void StartRequirementsThread()
        {
            Thread chkReqThread = new Thread(new ThreadStart(CheckRequirements));
            chkReqThread.Start();
        }
        #endregion

        public frmMain()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this._obj_settings = new SystemSettings();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.allResolutions = new Dictionary<string, Resolution>();
            this.GetResolutions();
            List<Resolution> tmpRes = new List<Resolution>();
            tmpRes = this.allResolutions.Values.ToList();

            foreach (Resolution res in tmpRes)
                cbResolution.Items.Add(res.resWidth.ToString() + "x" + res.resHeight.ToString());

            ReadFile();
            lblRequirementsMsg.Text = "";
            StartRequirementsThread();

            if (this._LOADED)
            {
                cbResolution.SelectedIndex = cbResolution.FindStringExact(_obj_settings.VIDEO_RES_WIDTH.ToString() + "x" + _obj_settings.VIDEO_RES_HEIGHT.ToString());

                if (_obj_settings.VIDEO_FULLSCREEN)
                    cbFullScreen.Checked = true;

                if (_obj_settings.VIDEO_ANTIALIASING)
                    cbAA.Checked = true;

                if (_obj_settings.VIDEO_VSYNC)
                    cbVsync.Checked = true;

                if (_obj_settings.VIDEO_OCULUS_ENABLED)
                    cbOculus.Checked = true;

                if (_obj_settings.SIM_ENABLED)
                    cbSimMode.Checked = true;

                if (_obj_settings.GAME_TRUESCALE)
                    cbTrueScale.Checked = true;

                numMasterVol.Value = (decimal)(_obj_settings.AUDIO_MAIN_VOLUME * 100);
                numMusicVol.Value = (decimal)(_obj_settings.AUDIO_MUSIC_VOLUME * 100);
                numSFXVol.Value = (decimal)(_obj_settings.AUDIO_SFX_VOLUME * 100);
                txtSimPlanet.Text = _obj_settings.SIM_PLANET;
                numSimSpeed.Value = (decimal)(_obj_settings.SIM_SPEED);
            }
            else
            {
                DialogResult diagResult = MessageBox.Show("Could not load the config file, please make sure you have permission to use the %APPDATA% directory.", "Error Loading Config - Planetary Orbit", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (diagResult == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}
