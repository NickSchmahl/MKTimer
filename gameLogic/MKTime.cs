namespace MKTimer.gameLogic
{
    public class MkTime
    {
        public int Minutes { get; }
        public int Seconds { get; }
        public int Millis { get; }
        public byte Xs { get; }

        public MkTime(string time)
        {
            Minutes = 0;
            Seconds = 0;
            Millis = 0;
            Xs = 0;

            if (!time.Contains(':')) time = "0:" + time;
            if (!time.Contains(',')) time += ",0";

            var parts = time.Split(':', ',');

            if (int.TryParse(parts[0], out var value)) Minutes = value;
            if (int.TryParse(parts[1], out var value2)) Seconds = value2;

            if (Seconds > 60)
            {
                Minutes += Seconds / 60;
                Seconds %= 60;
            }

            var startZeros = 0;
            if (parts[2].Length > 0 && parts[2][0].Equals('0')) startZeros++;
            if (parts[2].Length > 1 && parts[2][0].Equals('0') && parts[2][1].Equals('0')) startZeros++;
            if (int.TryParse(parts[2], out var value3))
            {
                if (value3 < 10 && startZeros == 0) value3 *= 100;
                if ((value3 < 100 && startZeros == 0) || (value3 < 10 && startZeros == 1)) value3 *= 10;
                Millis = value3;
            }
            else if (parts[2].Contains('x') || parts[2].Contains('X'))
            {
                if (parts[2][0] == 'x' || parts[2][0] == 'X') Xs = 0b00000111;
                else if (parts[2][1] == 'x' || parts[2][1] == 'X')
                {
                    Xs = 0b00000011;
                    Millis = int.Parse(parts[2][0].ToString()) * 100;
                }
                else if (parts[2][2] == 'x' || parts[2][2] == 'X')
                {
                    Xs = 0b00000001;
                    Millis = int.Parse(parts[2][0] + parts[2][1].ToString()) * 10;
                }
            }
        }

        public MkTime(double? time)
        {
            if (time == null) return;

            Minutes = (int)time / 60;
            Seconds = (int)time % 60;
            Millis = (int)(time * 1000) % 1000;
        }

        public override string ToString()
        {
            var millisString = Xs switch
            {
                7 => "xxx",
                3 => (Millis / 100) + "xx",
                1 => (Millis / 10) + "x",
                _ => Millis switch
                {
                    < 10 => "00" + Millis,
                    < 100 => "0" + Millis,
                    _ => Millis.ToString()
                }
            };

            var minutesString = Minutes == 0 ? "" : Minutes + ":";

            var secondsString = Seconds < 10 ? "0" + Seconds : Seconds.ToString();

            return minutesString + secondsString + "," + millisString;
        }

        public double ToDouble()
        {
            double res = Minutes * 60 + Seconds;
            res += Xs switch
            {
                7 => 0.999,
                3 => (Millis + 99) / 1000.0,
                1 => (Millis + 9) / 1000.0,
                _ => Millis / 1000.0,
            };

            return res;
        }

        public bool Matches(MkTime? that)
        {
            if (that == null) return false;
            if (Seconds != that.Seconds || Minutes != that.Minutes) return false;
            return Xs switch
            {
                0 => Millis == that.Millis,
                1 => Millis / 10 == that.Millis / 10,
                3 => Millis / 100 == that.Millis / 100,
                7 => true,
                _ => false
            };
        }
    }
}