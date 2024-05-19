using System.Diagnostics;

namespace MKTimer
{
    public class MKTime
    {
        public int minutes { get; }
        public int seconds { get; }
        public int millis { get; }
        public byte xs { get; }

        public MKTime(string time)
        {
            minutes = 0;
            seconds = 0;
            millis = 0;
            xs = 0;

            if (!time.Contains(':')) time = "0:" + time;
            if (!time.Contains(',')) time += ",0";

            string[] parts = time.Split(':', ',');

            if (int.TryParse(parts[0], out int value)) minutes = value;
            if (int.TryParse(parts[1], out int value2)) seconds = value2;

            if (seconds > 60)
            {
                minutes += seconds / 60;
                seconds %= 60;
            }

            int startZeros = 0;
            if (parts[2].Length > 0 && parts[2][0].Equals('0')) startZeros++;
            if (parts[2].Length > 1 && parts[2][0].Equals('0') && parts[2][1].Equals('0')) startZeros++;
            if (int.TryParse(parts[2], out int value3))
            {
                if (value3 < 10 && startZeros == 0) value3 *= 100;
                if ((value3 < 100 && startZeros == 0) || (value3 < 10 && startZeros == 1)) value3 *= 10;
                millis = value3;
            }
            else if (parts[2].Contains('x') || parts[2].Contains('X'))
            {
                if (parts[2][0] == 'x' || parts[2][0] == 'X') xs = 0b00000111;
                else if (parts[2][1] == 'x' || parts[2][1] == 'X')
                {
                    xs = 0b00000011;
                    millis = int.Parse(parts[2][0].ToString()) * 100;
                }
                else if (parts[2][2] == 'x' || parts[2][2] == 'X')
                {
                    xs = 0b00000001;
                    millis = int.Parse(parts[2][0].ToString() + parts[2][1].ToString()) * 10;
                }
            }
        }

        public MKTime(double? time)
        {
            if (time == null) return;

            minutes = (int)time / 60;
            seconds = (int)time % 60;
            millis = ((int)(time * 1000)) % 1000;
        }

        public override string ToString()
        {
            string millisString = xs switch
            {
                7 => "xxx",
                3 => (millis / 100) + "xx",
                1 => (millis / 10) + "x",
                _ => millis switch
                {
                    < 10 => "00" + millis,
                    < 100 => "0" + millis,
                    _ => millis.ToString()
                }
            };

            string minutesString = minutes == 0 ? "" : minutes.ToString() + ":";

            string secondsString = seconds < 10 ? "0" + seconds : seconds.ToString();

            return minutesString + secondsString + "," + millisString;
        }

        public double ToDouble()
        {
            double res = minutes * 60 + seconds;
            res += xs switch
            {
                7 => 0.999,
                3 => (millis + 99) / 1000.0,
                1 => (millis + 9) / 1000.0,
                _ => millis / 1000.0,
            };

            return res;
        }

        public bool Matches(MKTime? that)
        {
            if (that == null) return false;
            if (seconds != that.seconds || minutes != that.minutes) return false;
            return xs switch
            {
                0 => millis == that.millis,
                1 => millis / 10 == that.millis / 10,
                3 => millis / 100 == that.millis / 100,
                7 => true,
                _ => false
            };
        }
    }
}