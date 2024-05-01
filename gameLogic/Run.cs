using System.Linq;
using System.Threading;

namespace MKTimer {
    public class Run {
        public int lap_count;
        public MKTime?[] laps;

        public Run(int lap_count) {
            this.lap_count = lap_count;
            laps = new MKTime?[lap_count];
        }

        public Run(double?[] laps) {
            lap_count = laps.Length;
            this.laps = laps.Select(lap => new MKTime(lap)).ToArray();
        }

        public Run(MKTime?[] laps) {
            lap_count = laps.Length;
            this.laps = laps;
        }

        public double? GetTotalTime() {
            if (laps.Contains(null)) return null;
            else return laps.Select(lap => lap?.ToDouble()).Sum();
        }

        public Run Copy()
        {
            var newLaps = new MKTime?[lap_count];
            laps.CopyTo(newLaps, 0);
            return new Run(newLaps.Select(lap => lap?.ToDouble()).ToArray());
        }
    }
}