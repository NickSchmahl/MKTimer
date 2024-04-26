using System.IO;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace MKTimer {
    public class PBParser {
        private static readonly string filePath = "./data/tracks.json";

        public static Run? GetPbRun(MK8DLXTrack track, MK8DLXMode mode) {

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(json);

                if (jsonObject.ContainsKey(track.ToString())) 
                {
                    JObject? trackInfo = (JObject?) jsonObject.GetValue(track.ToString());
                    if (trackInfo != null && trackInfo.ContainsKey(mode.ToString())) 
                    {
                        JObject? pbTimes = (JObject?) trackInfo.GetValue(mode.ToString());
                        if (pbTimes != null && pbTimes.ContainsKey("lap_times")) 
                        {
                            JArray? lap_times = (JArray?) pbTimes.GetValue("lap_times");
                            if (lap_times != null) return new Run(ParseLapTimes(lap_times));
                        } 
                        else Console.Write("No laps in PB registered");
                    }
                } else return null;
            }
            else Console.WriteLine("File not found.");

            return null;
        }

        public static Run? GetSobRun(MK8DLXTrack track, MK8DLXMode mode) {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(json);

                if (jsonObject.ContainsKey(track.ToString())) 
                {
                    JObject? trackInfo = (JObject?) jsonObject.GetValue(track.ToString());
                    if (trackInfo != null && trackInfo.ContainsKey(mode.ToString())) 
                    {
                        JObject? pbTimes = (JObject?) trackInfo.GetValue(mode.ToString());
                        if (pbTimes != null && pbTimes.ContainsKey("sob_laps")) 
                        {
                            JArray? lap_times = (JArray?) pbTimes.GetValue("sob_laps");
                            if (lap_times != null) return new Run(ParseLapTimes(lap_times));
                        }
                    }
                } else return null;
            }
            else Console.WriteLine("File not found.");

            return null;
        }

        public static void UpdatePbRun(TrackInfo newTrackInfo) {
            Run run = newTrackInfo.pb ?? new Run([]); 
            MK8DLXTrack track = newTrackInfo.track;
            MK8DLXMode mode = newTrackInfo.mode;

              // Check if the file exists
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(json);

                JObject jsonRun;
                double? total_time = run.GetTotalTime();
                if (run.laps != null && total_time != null) {
                    jsonRun= new JObject
                    {
                        { "total_time", new JValue(TimeParser.GetTimeString((double) total_time)) },
                        { "lap_times", new JArray(run.laps.Select(d => {
                                if (d == null) return null;
                                else return TimeParser.GetTimeString((double) d);
                            }).ToList()) 
                        }
                    };
                }
                else 
                { 
                    Console.WriteLine("No Run to save."); 
                    return; 
                }

                // Display the extracted data
                if (jsonObject.ContainsKey(track.ToString())) 
                {
                    JObject? trackInfo = (JObject?) jsonObject.GetValue(track.ToString());
                    if (trackInfo != null && trackInfo.ContainsKey(mode.ToString())) 
                    {
                        trackInfo[mode.ToString()] = jsonRun;
                    }
                    else trackInfo?.Add(mode.ToString(), jsonRun);
                } 
                else 
                {
                    JObject modeJObject = new JObject {
                        {mode.ToString(), jsonRun}
                    };
                    jsonObject.Add(track.ToString(), modeJObject);
                }
                File.WriteAllText(filePath, jsonObject.ToString());
            }
            else Console.WriteLine("File not found.");
        }

        public static void UpdateSobRun(TrackInfo newTrackInfo) {
            Run run = newTrackInfo.sob ?? new Run([]); 
            MK8DLXTrack track = newTrackInfo.track;
            MK8DLXMode mode = newTrackInfo.mode;

            if (run.laps == null || run.laps.Contains(null)) 
            {
                Console.WriteLine("No run to save");
                return;
            }

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(json);

                if (jsonObject.ContainsKey(track.ToString())) 
                {
                    JObject? trackInfo = (JObject?) jsonObject.GetValue(track.ToString());
                    if (trackInfo != null && trackInfo.ContainsKey(mode.ToString())) 
                    {
                        JObject? modeInfo = (JObject?) trackInfo.GetValue(mode.ToString());
                        if (modeInfo != null && modeInfo.ContainsKey("sob_laps"))
                        {
                            modeInfo["sob_laps"] = new JArray(run.laps.Select(x => {
                                if (x == null) return null;
                                else return TimeParser.GetTimeString((double) x);
                            }));
                        }
                        else if (modeInfo != null && !modeInfo.ContainsKey("sob_laps"))
                        {
                            modeInfo.Add("sob_laps", new JArray(run.laps.Select(x => {
                                if (x == null) return null;
                                else return TimeParser.GetTimeString((double) x);
                            })));
                        } 
                        else Console.Write("Somehow the track and mode is not there yet.");
                    }
                } 
                File.WriteAllText(filePath, jsonObject.ToString());
            }
            else Console.WriteLine("File not found.");
        }

        private static double?[] ParseLapTimes(JArray jsonArray) {
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

            return parsed_times;
        }

    }
}