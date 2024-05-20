using System;
using System.IO;
using System.Windows.Forms;
using MKTimer.gameLogic;
using Newtonsoft.Json.Linq;

namespace MKTimer
{
    public partial class Form1 : Form
    {
        private static Form1? _activeForm;
        private static readonly string StartFile = Path.Combine("data", "start.json");

        public Form1()
        {   
            const string trackJsonKey = "track";
            const string modeJsonKey = "mode";

            var track = MK8DLXTrack.WiiMooMooMeadows;
            var mode = MK8DLXMode._200CC;

            if (File.Exists(StartFile))
            {
                string json = File.ReadAllText(StartFile);
                JObject jsonObject = JObject.Parse(json);

                if (jsonObject.ContainsKey(trackJsonKey)) 
                {
                    JValue? startupTrack = (JValue?) jsonObject.GetValue(trackJsonKey);
                    if (startupTrack != null) track = (MK8DLXTrack) Enum.Parse(typeof(MK8DLXTrack), startupTrack.ToString());
                    else track = MK8DLXTrack.WiiMooMooMeadows;
                } 

                if (jsonObject.ContainsKey(modeJsonKey)) 
                {
                    JValue? startupMode = (JValue?) jsonObject.GetValue(modeJsonKey);
                    if (startupMode != null) mode = (MK8DLXMode) Enum.Parse(typeof(MK8DLXMode), startupMode.ToString());
                    else mode = MK8DLXMode._200CC;
                } 
            }
            else Console.WriteLine("File not found.");

            trackInfo = new TrackInfo(track, mode);

            InitializeComponent();
            if (_activeForm != null) 
            {
                // activeForm.Close();
                _activeForm = null;
            }
            _activeForm = this;
        }

    }
}
