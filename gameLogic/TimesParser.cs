using System;

namespace MKTimer {
    public class TimeParser {

        public static double ParseTime(string time) {
            int minutes = 0;
            int seconds = 0;
            int millis = 0;

            if (!time.Contains(':')) {
                time = "0:" + time;
            }
            string[] parts = time.Split(':', ',');

            if (time.Contains(':') && int.TryParse(parts[0], out int value)) {
                minutes = value;
            }

            if (!time.Contains(',')) {
                return 0;
            } else if (int.TryParse(parts[1], out int value2)) {
                seconds = value2;
            }

            int startZeros = 0;
            if (parts[2].Length > 0 && parts[2][0].Equals('0')) startZeros++;
            if (parts[2].Length > 1 && parts[2][0].Equals('0') && parts[2][1].Equals('0')) startZeros++;
            if (int.TryParse(parts[2], out int value3)) {
                if (value3 < 10 && startZeros == 0) value3 *= 100;
                if ((value3 < 100 && startZeros == 0) || (value3 < 10 && startZeros == 1)) value3 *= 10;
                millis = value3;
            }

            return minutes * 60 + seconds + millis / 1000.0;
        }

        public static string GetTimeString(double time) {
            string result = "";
            int minutes = (int) time / 60;
            if (minutes != 0) {
                result += "" + minutes + ':';
            }

            time -= minutes*60;

            int seconds = (int) time;
            if (seconds < 10 && minutes != 0) {
                result += "0" + seconds + ',';
            } else {
                result += "" + seconds + ',';
            }

            time -= seconds;

            int millis = (int) (Math.Round(time, 3) * 1000);
            if (millis < 10) {
                result += "00" + millis;
            } else if (millis < 100) {
                result += "0" + millis;
            } else {
                result += millis;
            }
            return result;
        }

    }
}