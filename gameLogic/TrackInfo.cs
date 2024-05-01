using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MKTimer {
    public class TrackInfo {
        private const string jsonPath = "./data/tracks.json";
        public Run? pb;
        public Run? sob;
        public int runCount;
        public int secondsPlayed;
        public Run[] runs;
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

            var runs = Array.Empty<Run>();

            string trackTimesPath = "./data/" + trackName + modeName + ".csv";
            if (File.Exists(trackTimesPath))
            {
                string[] runLines = File.ReadAllLines(trackTimesPath);
                foreach (string runLine in runLines)
                {
                    int i = 0;
                    foreach(string runTime in runLine.Split(';')) 
                    {
                        double?[] runTimes = new double?[3];
                        if (double.TryParse(runTime, out double time)) runTimes[i] = time;
                        else runTimes[i] = null;

                        _ = runs.Append(new Run(runTimes));
                        i++;
                    }
                }
            }

            this.runs = runs;
        }

        public void StoreInformation() 
        {
            var innerJObject = new JObject {
                { "lap_times", new JArray((pb?.laps ?? []).Select(lap => lap?.ToString())) },
                { "sob_laps", new JArray((sob?.laps ?? []).Select(lap => lap?.ToString())) },
                { "run_count", runCount + TimerGridView.RUN_COUNT },
                { "seconds_played", secondsPlayed + RunCountPanel.secondsCounter }
            };
            var jObject = new JObject {
                { mode.ToString(), innerJObject}
            };

            if (!File.Exists(jsonPath)) return;

            var storedObj = JObject.Parse(File.ReadAllText(jsonPath));
            
            if (!storedObj.ContainsKey(track.ToString())) {
                storedObj.Add(track.ToString(), jObject);
            }
            else if (storedObj[track.ToString()]?[mode.ToString()] == null) {
                ((JObject?) storedObj[track.ToString()])?.Add(jObject);
            } else {
                storedObj[track.ToString()]?[mode.ToString()]?.Replace(innerJObject);
            }

            File.WriteAllText(jsonPath, storedObj.ToString());
        }

        private static Run? ParseLapTimes(JArray? jsonArray) 
        {
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
                else if (token.Type == JTokenType.Float)
                {
                    double? doubleValue = (double?)token;
                    if (doubleValue != null) parsed_times[index] = doubleValue;
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