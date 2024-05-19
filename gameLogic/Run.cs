using System.Linq;
using System.Threading;
using MKTimer.gameLogic;

namespace MKTimer {
    public class Run {
        public int lap_count;
        public MkTime?[] laps;

        public Run(int lap_count) {
            this.lap_count = lap_count;
            laps = new MkTime?[lap_count];
        }

        public Run(double?[] laps) {
            lap_count = laps.Length;
            this.laps = laps.Select(lap => new MkTime(lap)).ToArray();
        }

        public Run(MkTime?[] laps) {
            lap_count = laps.Length;
            this.laps = laps;
        }

        public double? GetTotalTime() {
            if (laps.Contains(null)) return null;
            else return laps.Select(lap => lap?.ToDouble()).Sum();
        }

        public Run Copy()
        {
            var newLaps = new MkTime?[lap_count];
            laps.CopyTo(newLaps, 0);
            return new Run(newLaps.Select(lap => lap?.ToDouble()).ToArray());
        }
    }
}