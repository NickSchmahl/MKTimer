using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MKTimer.gameLogic {
    public class TrackInfo {
        public static readonly string JsonPath = Path.Combine("data", "tracks.json");
        private readonly string _csvPath;
        public Run? Pb;
        public Run? Sob;
        public readonly Run[] Runs;
        public readonly int RunCount;
        public readonly int SecondsPlayed;
        public List<MkTime>[] Paces { get; }
        private int SavedRuns { get; set; }
        public readonly MK8DLXTrack Track;
        public readonly MK8DLXMode Mode;

        public TrackInfo(MK8DLXTrack track, MK8DLXMode mode)
        {
            Track = track;
            Mode = mode;
            _csvPath = Path.Combine("data", "runs", $"{track}{mode}.csv");

            var trackName = track.ToString();
            var modeName = mode.ToString();
            
            var jsonObject = JObject.Parse(File.ReadAllText(JsonPath));

            var secondsPlayedToken = (JValue?) jsonObject[trackName]?[modeName]?["seconds_played"];
            if (jsonObject.ContainsKey(trackName) && jsonObject[trackName]?[modeName] != null)
            {
                var lapTimesToken = (JArray?) jsonObject[trackName]?[modeName]?["lap_times"];
                var sobLapsToken = (JArray?) jsonObject[trackName]?[modeName]?["sob_laps"];
                var runCountToken = (JValue?) jsonObject[trackName]?[modeName]?["run_count"];
                var paces = (JArray?)jsonObject[trackName]?[modeName]?["paces"];

                Pb = ParseLapTimes(lapTimesToken);
                Sob = ParseLapTimes(sobLapsToken);
                RunCount = runCountToken?.Value<int>() ?? 0;
                SecondsPlayed = secondsPlayedToken?.Value<int>() ?? 0;
                
                if (paces != null) 
                {
                    Paces = new List<MkTime>[paces.Count];
                    for (var i = 0; i < paces.Count; i++) 
                    {
                        var pace = paces[i].Select(time => new MkTime(time.ToString())).ToList();
                        Paces[i] = pace;
                    }
                }
            }

            var runs = new List<Run>();

            if (File.Exists(_csvPath))
            {
                var runLines = File.ReadAllLines(_csvPath);
                foreach (var runLine in runLines)
                {
                    var runTimes = new MkTime?[3];
                    
                    var i = 0;
                    foreach (var runTime in runLine.Split(';'))
                    {
                        if (i < 3) runTimes[i] = new MkTime(runTime);
                        i++;
                    }
                    
                    runs.Add(new Run(runTimes));
                }
            }
            else
            {
                File.WriteAllText(_csvPath, "");
            }

            Runs = runs.ToArray();
        }

        public void StoreInformation(List<Run> runs) 
        {
            var innerJObject = new JObject {
                { "lap_times", new JArray((Pb?.laps ?? []).Select(lap => lap?.ToString())) },
                { "sob_laps", new JArray((Sob?.laps ?? []).Select(lap => lap?.ToString())) },
                { "run_count", RunCount + TimerGridView.RUN_COUNT },
                { "seconds_played", RunCountPanel.secondsCounter }
            };
            var jObject = new JObject {
                { Mode.ToString(), innerJObject}
            };

            if (!File.Exists(JsonPath)) return;

            var storedObj = JObject.Parse(File.ReadAllText(JsonPath));
            
            if (!storedObj.ContainsKey(Track.ToString())) {
                storedObj.Add(Track.ToString(), jObject);
            }
            else if (storedObj[Track.ToString()]?[Mode.ToString()] == null) {
                ((JObject?) storedObj[Track.ToString()])?.Add(jObject);
            } else {
                storedObj[Track.ToString()]?[Mode.ToString()]?.Replace(innerJObject);
            }

            File.WriteAllText(JsonPath, storedObj.ToString());

            if (!File.Exists(_csvPath)) return;
            
            var toWrite = "";
            for (int i = SavedRuns; i < runs.Count - 1; i++)
            {
                var run = runs[i];
                foreach (var time in run.laps)
                {
                    if (time == null) toWrite += "-";
                    else toWrite += time.ToString();
                    toWrite += ";";
                }
                toWrite += "\n";
            }

            File.AppendAllText(_csvPath, toWrite);
            Console.WriteLine($"Saved {runs.Count - SavedRuns - 1} runs to {_csvPath}.");
            SavedRuns = runs.Count - 1;
        }

        private static Run? ParseLapTimes(JArray? jsonArray) 
        {
            if (jsonArray == null) return null;

            // Create a list to store the parsed doubles
            var parsedTimes = new double?[3];

            // Iterate through the array, parse each item to a double, and add it to the list
            var index = 0;
            foreach (var token in jsonArray)
            {
                switch (token.Type)
                {
                    case JTokenType.String:
                    {
                        var stringValue = (string?)token;
                        if (stringValue != null) parsedTimes[index] = TimeParser.ParseTime(stringValue);
                        else parsedTimes[index] = null;
                        break;
                    }
                    case JTokenType.Float:
                    {
                        var doubleValue = (double?)token;
                        if (doubleValue != null) parsedTimes[index] = doubleValue;
                        else parsedTimes[index] = null;
                        break;
                    }
                    default:
                        Console.WriteLine($"Warning: Skipping non-string value: {token}");
                        parsedTimes[index] = null;
                        break;
                }

                index++;
            }

            return new Run(parsedTimes);
        }
    }
}