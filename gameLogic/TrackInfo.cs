using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MKTimer {
    public class TrackInfo {
        private const string jsonPath = "./data/tracks.json";
        public readonly Run? pb;
        public readonly Run? sob;
        public readonly int runCount;
        public readonly int secondsPlayed;
        public readonly MK8DLXTrack track;
        public readonly MK8DLXMode mode;

        public TrackInfo(MK8DLXTrack track, MK8DLXMode mode)
        {
            this.track = track;
            this.mode = mode;

            string trackName = track.ToString();
            string modeName = mode.ToString();
            JObject jsonObject = JObject.Parse(File.ReadAllText(jsonPath));

            if (jsonObject.ContainsKey(trackName.ToString()) && jsonObject[trackName]?[modeName] != null)
            {
                JArray? lapTimesToken = (JArray?) jsonObject[trackName]?[modeName]?["lap_times"];
                JArray? sobLapsToken = (JArray?) jsonObject[trackName]?[modeName]?["sob_laps"];
                JValue? runCountToken = (JValue?) jsonObject[trackName]?[modeName]?["run_count"];
                JValue? secondsPlayedToken = (JValue?) jsonObject[trackName]?[modeName]?["seconds_played"];
                
                pb = ParseLapTimes(lapTimesToken);
                sob = ParseLapTimes(sobLapsToken);
                runCount = runCountToken?.Value<int>() ?? 0;
                secondsPlayed = secondsPlayedToken?.Value<int>() ?? 0;
            }
        }

        private Run? ParseLapTimes(JArray? jsonArray) {
            if (jsonArray == null) return null;

            // Create a list to store the parsed doubles
            double?[] parsed_times = new double?[3];

            // Iterate through the array, parse each item to a double, and add it to the list
            int index = 0;
            foreach (JToken token in jsonArray)
            {
                if (token.Type == JTokenType.String)
                {
                    string? stringValue = (string?)token;
                    if (stringValue != null) parsed_times[index] = TimeParser.ParseTime(stringValue);
                    else parsed_times[index] = null;
                }
                else
                {
                    Console.WriteLine($"Warning: Skipping non-string value: {token}");
                    parsed_times[index] = null;
                }
                index++;
            }

            return new Run(parsed_times);
        }
    }
}