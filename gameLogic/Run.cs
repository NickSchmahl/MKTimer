using System.Linq;

namespace MKTimer {
    public class Run {
        public int lap_count;
        public double?[] laps;

        public Run(int lap_count) {
            this.lap_count = lap_count;
            laps = new double?[lap_count];
        }

        public Run(double?[] laps) {
            lap_count = laps.Length;
            this.laps = laps;
        }

        public double? GetTotalTime() {
            if (laps.Contains(null)) return null;
            else return laps.Sum();
        }

        public Run Copy()
        {
            var newLaps = new double?[lap_count];
            laps.CopyTo(newLaps, 0);
            return new Run(newLaps);
        }
    }
}