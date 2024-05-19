using System;
using System.IO;
using System.Windows.Forms;
using MKTimer.gameLogic;
using Newtonsoft.Json.Linq;

namespace MKTimer
{
    public partial class Form1 : Form
    {
        private static Form1? activeForm;
        private static readonly string startFile = @"C:\Users\nschmahl\RiderProjects\MKTimer\data\start.json";

        public Form1()
        {   
            const string trackJsonKey = "track";
            const string modeJsonKey = "mode";

            var track = MK8DLXTrack.WiiMooMooMeadows;
            var mode = MK8DLXMode._200CC;

            if (File.Exists(startFile))
            {
                string json = File.ReadAllText(startFile);
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
            if (activeForm != null) 
            {
                // activeForm.Close();
                activeForm = null;
            }
            activeForm = this;
        }

    }
}
