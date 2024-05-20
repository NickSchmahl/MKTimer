using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace MKTimer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            StartupFileCheck();

            Application.Run(new Form1());
        }

        private static void StartupFileCheck()
        {
            if (!Directory.Exists("./data"))
            {
                Directory.CreateDirectory("./data");
            }

            var combine = Path.Combine("data", "runs");
            if (!Directory.Exists(combine))
            {
                Directory.CreateDirectory(combine);
            }
            
            var jsonPath = Path.Combine("data", "tracks.json");
            if (!File.Exists(jsonPath))
            {
                File.WriteAllText(jsonPath, new JObject().ToString());
            }

            jsonPath = Path.Combine("data", "start.json");
            if (!File.Exists(jsonPath))
            {
                var jObj = new JObject 
                {
                    ["track"] = MK8DLXTrack.MarioKartStadium.ToString(), 
                    ["mode"] = MK8DLXMode._150CC.ToString()
                };
                File.WriteAllText(jsonPath, jObj.ToString());
            }
        }
    }
}
