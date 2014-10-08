using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace POSystem
{
    public class ConfigManager
    {
        public bool _LOADED = false;
        private SystemSettings _obj_settings;
        private Dictionary<string, string> _obj_data_dirs = null;
        private List<SceneFileMin> _obj_data_sceneFiles = null;
        private List<SceneFile> _obj_data_scenes = null;

        public ConfigManager()
        {
            this._obj_settings = new SystemSettings();
            this._obj_data_dirs = new Dictionary<string, string>();
            this._obj_data_dirs.Add("game_app", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GameSettings.CONFIG_GAME_NAME_CLEAN));
            this._obj_data_dirs.Add("custom", Path.Combine(this._obj_data_dirs["game_app"], "custom"));
            this._obj_data_dirs.Add("custom_systems", Path.Combine(this._obj_data_dirs["custom"], "systems"));

            this.InitializeDirectories();
            this.ReadSettings();
            this.LoadSceneStore();
        }

        public SystemSettings CoreSettings
        {
            get { return this._obj_settings; }
        }

        private void InitializeDirectories()
        {
            try
            {
                foreach (KeyValuePair<string, string> tmpDirectory in this._obj_data_dirs)
                {
                    if (!Directory.Exists(tmpDirectory.Value))
                        Directory.CreateDirectory(tmpDirectory.Value);
                }
            }
            catch (Exception ex)
            {
                this._LOADED = false;
                Logger.log(Log_Type.ERROR, "FATAL: " + ex.Message);
                return;
            }
        }

        private void ReadSettings()
        {
            XmlSerializer serializer = null;
            Stream _stream = null;

            try
            {
                if (File.Exists(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SETTINGS_FILE))
                {
                    _stream = File.Open(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SETTINGS_FILE, FileMode.Open);
                    serializer = new XmlSerializer(typeof(SystemSettings));
                    _obj_settings = (SystemSettings)serializer.Deserialize(_stream);
                    _stream.Close();
                }
                else
                {
                    CreateNewSettings(); // Creates a new file.
                }
            }
            catch (InvalidOperationException ex)
            {
                Logger.log(Log_Type.WARNING, "Creating New Config File - " + ex.Message);
                if (_stream != null)
                    _stream.Close();

                CreateNewSettings();
            }
            catch (Exception ex)
            {
                Logger.log(Log_Type.ERROR, "FATAL: " + ex.Message);
                return;
            }
        }

        public void CreateNewSettings()
        {
            try
            {
                if (File.Exists(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SETTINGS_FILE))
                    File.Delete(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SETTINGS_FILE);

                FileStream _stream = File.Create(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SETTINGS_FILE);
                XmlSerializer serializer = new XmlSerializer(typeof(SystemSettings));
                serializer.Serialize(_stream, _obj_settings);
                _stream.Close();
                this._LOADED = true;
            }
            catch (Exception ex) 
            {
                Logger.log(Log_Type.ERROR, "FATAL: " + ex.Message);
                return;
            }
        }

        public void LoadSceneStore()
        {
            this._obj_data_sceneFiles = new List<SceneFileMin>();

            try
            {
                if (File.Exists(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SYSTEMS_FILE))
                {
                    string[] systemsFiles = File.ReadAllLines(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SYSTEMS_FILE);
                    SceneFileMin tmpSceneFile = null;

                    for (uint i = 0; i < systemsFiles.Length; i++)
                    {
                        tmpSceneFile = new SceneFileMin();
                        tmpSceneFile.SCENE_ID = i;
                        tmpSceneFile.FILE_NAME = systemsFiles[i];
                        this._obj_data_sceneFiles.Add(tmpSceneFile);
                    }
                }
                else
                {
                    using (var tmpFS = new FileStream(this._obj_data_dirs["game_app"] + "/" + GameSettings.ASSET_CONFIG_SYSTEMS_FILE, FileMode.Create, FileAccess.ReadWrite))
                    {
                        using (var tmpFW = new StreamWriter(tmpFS))
                        {
                            tmpFW.WriteLine("solar_system.scene");
                            tmpFW.Flush();
                            tmpFW.Close();
                        }
                    }
                    SceneFileMin tmpSceneFile = new SceneFileMin();
                    tmpSceneFile.FILE_NAME = "solar_system.scene";
                    tmpSceneFile.SCENE_ID = 0;
                    this._obj_data_sceneFiles.Add(tmpSceneFile);
                }
            }
            catch (Exception ex)
            {
                Logger.log(Log_Type.ERROR, "FATAL: " + ex.Message);
                return;
            }
        }

        public List<SceneFileMin> GetAllScenes()
        {
            if (this._obj_data_sceneFiles != null)
            {
                for (int i = 0; i < this._obj_data_sceneFiles.Count; i++)
                {
                    if (File.Exists(this._obj_data_dirs["custom_systems"] + "/" + this._obj_data_sceneFiles[i].FILE_NAME))
                        this._obj_data_sceneFiles[i].FILE_EXISTS = true;
                }

                return this._obj_data_sceneFiles;
            }

            return null;
        }

        public SceneFile LoadScene(int _id)
        {
            if (this._obj_data_scenes[_id] != null)
                return this._obj_data_scenes[_id];

            return null;
        }
    }
}
