using DGJv3.NeteaseApi;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DGJv3
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    internal class MainConfig : INotifyPropertyChanged
    {
        public static string ConfigFullPath { get; }

        public static MainConfig Instance { get; }

        static MainConfig()
        {
            try
            {
                //string configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), @"弹幕姬\plugins\ExtendNetease");
                string configPath = Utilities.DataDirectoryPath;
                if (!Directory.Exists(configPath))
                {
                    Directory.CreateDirectory(configPath);
                }
                ConfigFullPath = Path.Combine(configPath, "neteasetoken.cfg");
                if (File.Exists(ConfigFullPath))
                {
                    string json = File.ReadAllText(ConfigFullPath);
                    Instance = JsonConvert.DeserializeObject<MainConfig>(json);
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Instance = new MainConfig();
        }

        [JsonProperty]
        public string Cookie
        {
            get => LoginSession.Session.GetCookieString("http://music.163.com/");
            set => LoginSession.Login(value);
        }

        [JsonProperty]
        public Quality Quality { get => _Quality; set { if (_Quality != value) { _Quality = value; OnPropertyChanged(); } } }

        public NeteaseSession LoginSession { get; } = new NeteaseSession();

        private Quality _Quality = Quality.HighQuality;

        public void SaveConfig()
        {
            File.WriteAllText(ConfigFullPath, JsonConvert.SerializeObject(this));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
